using Franquias.Api.DTOs;
using Franquias.Api.Entities;
using Franquias.Api.Repositories;

namespace Franquias.Api.Services
{
    public class VendasService : IVendasService
    {
        private readonly IRepository<Venda> _vendaRepository;
        private readonly IRepository<ProdutoServico> _produtoRepository;
        private readonly IRepository<Estoque> _estoqueRepository;
        private readonly IRepository<UnidadeFranqueada> _unidadeRepository;

        // Injetando 4 repositórios diferentes para orquestrar a regra de negócio
        public VendasService(
            IRepository<Venda> vendaRepository,
            IRepository<ProdutoServico> produtoRepository,
            IRepository<Estoque> estoqueRepository,
            IRepository<UnidadeFranqueada> unidadeRepository)
        {
            _vendaRepository = vendaRepository;
            _produtoRepository = produtoRepository;
            _estoqueRepository = estoqueRepository;
            _unidadeRepository = unidadeRepository;
        }

        public async Task<Venda> RegistrarVendaAsync(NovaVendaDTO dto)
        {
            // Regra 1 e 2: Validar unidade e presença de itens
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

            // Regra 3 e 4: Calcular total e abater estoque
            foreach (var itemDto in dto.Itens)
            {
                var produto = await _produtoRepository.ObterPorIdAsync(itemDto.ProdutoServicoId);
                if (produto == null || !produto.Ativo)
                    throw new Exception($"Produto ID {itemDto.ProdutoServicoId} não encontrado ou inativo.");

                // Vai no banco buscar o estoque específico daquele produto naquela franquia
                var estoqueList = await _estoqueRepository.BuscarAsync(e => 
                    e.ProdutoServicoId == itemDto.ProdutoServicoId && 
                    e.UnidadeFranqueadaId == dto.UnidadeFranqueadaId);
                    
                var estoque = estoqueList.FirstOrDefault();

                // Trava de segurança absoluta do Estoque Negativo
                if (estoque == null || estoque.Quantidade < itemDto.Quantidade)
                    throw new Exception($"Estoque insuficiente para o produto '{produto.Nome}'. Saldo atual: {estoque?.Quantidade ?? 0}, Tentativa de venda: {itemDto.Quantidade}.");

                // Abate a quantidade vendida do estoque e atualiza
                estoque.Quantidade -= itemDto.Quantidade;
                await _estoqueRepository.AtualizarAsync(estoque);

                // Calcula o preço baseado no valor do catálogo (evitando que o operador passe o preço errado)
                novaVenda.ValorTotal += (produto.PrecoBase * itemDto.Quantidade);
                
                novaVenda.Itens.Add(new ItemVenda
                {
                    ProdutoServicoId = produto.Id,
                    Quantidade = itemDto.Quantidade,
                    PrecoUnitario = produto.PrecoBase
                });
            }

            // Salva a venda e os itens dela de uma vez só
            await _vendaRepository.AdicionarAsync(novaVenda);
            
            return novaVenda;
        }
    }
}