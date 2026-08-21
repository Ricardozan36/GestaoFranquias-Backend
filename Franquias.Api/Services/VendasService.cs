using Franquias.Api.DTOs;
using Franquias.Api.Entities;
using Franquias.Api.Repositories;
using Franquias.Api.Data; // Adicionado para enxergar o AppDbContext
using Microsoft.EntityFrameworkCore; // Adicionado para destrancar o comando .Include()

namespace Franquias.Api.Services
{
    public class VendasService : IVendasService
    {
        private readonly IRepository<Venda> _vendaRepository;
        private readonly IRepository<ProdutoServico> _produtoRepository;
        private readonly IRepository<Estoque> _estoqueRepository;
        private readonly IRepository<UnidadeFranqueada> _unidadeRepository;
        private readonly AppDbContext _context; // A nossa "chave-mestra"

        // Construtor atualizado para receber o AppDbContext
        public VendasService(
            IRepository<Venda> vendaRepository,
            IRepository<ProdutoServico> produtoRepository,
            IRepository<Estoque> estoqueRepository,
            IRepository<UnidadeFranqueada> unidadeRepository,
            AppDbContext context) 
        {
            _vendaRepository = vendaRepository;
            _produtoRepository = produtoRepository;
            _estoqueRepository = estoqueRepository;
            _unidadeRepository = unidadeRepository;
            _context = context;
        }

        public async Task<Venda> RegistrarVendaAsync(NovaVendaDTO dto)
        {
            // --- SUA LÓGICA BLINDADA ESTÁ MANTIDA INTACTA ---
            var unidade = await _unidadeRepository.ObterPorIdAsync(dto.UnidadeFranqueadaId);
            if (unidade == null || unidade.Status == StatusUnidade.Inativa)
                throw new Exception("Unidade não encontrada ou a unidade está Inativa. Operação cancelada.");

            if (!dto.Itens.Any())
                throw new Exception("Uma venda deve possuir pelo menos um item.");

            var novaVenda = new Venda
            {
                UnidadeFranqueadaId = dto.UnidadeFranqueadaId,
                DataVenda = DateTime.Now,
                ValorTotal = 0,
                Itens = new List<ItemVenda>()
            };

            foreach (var itemDto in dto.Itens)
            {
                var produto = await _produtoRepository.ObterPorIdAsync(itemDto.ProdutoServicoId);
                if (produto == null || !produto.Ativo)
                    throw new Exception($"Produto ID {itemDto.ProdutoServicoId} não encontrado ou inativo.");

                var estoqueList = await _estoqueRepository.BuscarAsync(e => 
                    e.ProdutoServicoId == itemDto.ProdutoServicoId && 
                    e.UnidadeFranqueadaId == dto.UnidadeFranqueadaId);
                    
                var estoque = estoqueList.FirstOrDefault();

                if (estoque == null || estoque.Quantidade < itemDto.Quantidade)
                    throw new Exception($"Estoque insuficiente para o produto '{produto.Nome}'. Saldo atual: {estoque?.Quantidade ?? 0}, Tentativa de venda: {itemDto.Quantidade}.");

                estoque.Quantidade -= itemDto.Quantidade;
                await _estoqueRepository.AtualizarAsync(estoque);

                novaVenda.ValorTotal += (produto.PrecoBase * itemDto.Quantidade);
                
                novaVenda.Itens.Add(new ItemVenda
                {
                    ProdutoServicoId = produto.Id,
                    Quantidade = itemDto.Quantidade,
                    PrecoUnitario = produto.PrecoBase
                });
            }

            await _vendaRepository.AdicionarAsync(novaVenda);
            
            return novaVenda;
        }

        // ====================================================================
        // --- O SEGREDO DESVENDADO AQUI ---
        // ====================================================================
        public async Task<IEnumerable<Venda>> ObterTodasAsync()
        {
            // Ao invés de usar o Repositório Genérico, nós usamos a chave-mestra (_context)
            // para ir no banco e "puxar" as tabelas filhas (Itens) e os netos (Produto)
            return await _context.Vendas
                .Include(v => v.Itens)                   // Puxa a lista de itens daquela venda
                    .ThenInclude(i => i.Produto)         // Para cada item, puxa o cadastro do Produto
                .ToListAsync();                          // Transforma tudo numa lista
        }
    }
}