using Franquias.Api.Entities;
using Franquias.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Franquias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelatoriosController : ControllerBase
    {
        // Injetando múltiplos repositórios para cruzar os dados
        private readonly IRepository<Venda> _vendaRepository;
        private readonly IRepository<UnidadeFranqueada> _unidadeRepository;
        private readonly IRepository<Royalty> _royaltyRepository;
        private readonly IRepository<Estoque> _estoqueRepository;
        private readonly IRepository<ProdutoServico> _produtoRepository;
        private readonly IRepository<ChamadoSuporte> _chamadoRepository;

        public RelatoriosController(
            IRepository<Venda> vendaRepository,
            IRepository<UnidadeFranqueada> unidadeRepository,
            IRepository<Royalty> royaltyRepository,
            IRepository<Estoque> estoqueRepository,
            IRepository<ProdutoServico> produtoRepository,
            IRepository<ChamadoSuporte> chamadoRepository)
        {
            _vendaRepository = vendaRepository;
            _unidadeRepository = unidadeRepository;
            _royaltyRepository = royaltyRepository;
            _estoqueRepository = estoqueRepository;
            _produtoRepository = produtoRepository;
            _chamadoRepository = chamadoRepository;
        }

        // 1. FATURAMENTO MENSAL DA UNIDADE (O que já tínhamos feito)
        [HttpGet("vendas/unidade/{unidadeId}/mes/{mes}/ano/{ano}")]
        public async Task<IActionResult> ObterFaturamentoUnidade(int unidadeId, int mes, int ano)
        {
            var vendas = await _vendaRepository.BuscarAsync(v => 
                v.UnidadeFranqueadaId == unidadeId && 
                v.DataVenda.Month == mes && 
                v.DataVenda.Year == ano);

            return Ok(new {
                UnidadeId = unidadeId,
                Mes = mes,
                Ano = ano,
                TotalVendasRealizadas = vendas.Count(),
                FaturamentoTotal = vendas.Sum(v => v.ValorTotal)
            });
        }

        // 2. RANKING DE UNIDADES POR FATURAMENTO
        [HttpGet("ranking-faturamento")]
        public async Task<IActionResult> ObterRankingFaturamento()
        {
            var vendas = await _vendaRepository.ObterTodosAsync();
            var unidades = await _unidadeRepository.ObterTodosAsync();

            var ranking = vendas
                .GroupBy(v => v.UnidadeFranqueadaId)
                .Select(g => new 
                {
                    UnidadeId = g.Key,
                    NomeUnidade = unidades.FirstOrDefault(u => u.Id == g.Key)?.Nome ?? "Desconhecida",
                    FaturamentoTotal = g.Sum(v => v.ValorTotal),
                    QuantidadeVendas = g.Count()
                })
                .OrderByDescending(r => r.FaturamentoTotal) // Ordena do maior pro menor
                .ToList();

            return Ok(ranking);
        }

        // 3. TOTAL DE ROYALTIES GERADOS PELA REDE
        [HttpGet("royalties-gerados")]
        public async Task<IActionResult> ObterTotalRoyalties()
        {
            var royalties = await _royaltyRepository.ObterTodosAsync();
            
            return Ok(new {
                TotalArrecadado = royalties.Sum(r => r.ValorCobrado),
                QuantidadeCobrancas = royalties.Count(),
                Detalhes = royalties.Select(r => new { r.UnidadeFranqueadaId, r.Mes, r.Ano, r.ValorCobrado })
            });
        }

        // 4. ESTOQUE CRÍTICO (Abaixo de 10 unidades)
        [HttpGet("estoque-critico")]
        public async Task<IActionResult> ObterEstoqueCritico()
        {
            // Busca apenas estoques com quantidade perigosa
            var estoques = await _estoqueRepository.BuscarAsync(e => e.Quantidade < 10);
            var produtos = await _produtoRepository.ObterTodosAsync();
            var unidades = await _unidadeRepository.ObterTodosAsync();

            var resultado = estoques.Select(e => new 
            {
                Unidade = unidades.FirstOrDefault(u => u.Id == e.UnidadeFranqueadaId)?.Nome ?? "Desconhecida",
                Produto = produtos.FirstOrDefault(p => p.Id == e.ProdutoServicoId)?.Nome ?? "Desconhecido",
                QuantidadeAtual = e.Quantidade,
                Status = "CRÍTICO"
            }).ToList();

            return Ok(resultado);
        }

        // 5. QUANTIDADE DE CHAMADOS POR STATUS
        [HttpGet("chamados-por-status")]
        public async Task<IActionResult> ObterStatusChamados()
        {
            var chamados = await _chamadoRepository.ObterTodosAsync();
            
            var resumo = chamados
                .GroupBy(c => c.Status)
                .Select(g => new 
                {
                    Status = g.Key,
                    Quantidade = g.Count()
                }).ToList();

            return Ok(resumo);
        }
    }
}