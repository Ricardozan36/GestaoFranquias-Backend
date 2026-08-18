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
        private readonly IRepository<Venda> _vendaRepository;

        public RelatoriosController(IRepository<Venda> vendaRepository)
        {
            _vendaRepository = vendaRepository;
        }

        [HttpGet("vendas/unidade/{unidadeId}/mes/{mes}/ano/{ano}")]
        public async Task<IActionResult> ObterRelatorioVendas(int unidadeId, int mes, int ano)
        {
            // 1. Vai no banco e busca TODAS as vendas
            // Usamos o método BuscarAsync que criamos hoje cedo no Repositório Genérico
            var vendas = await _vendaRepository.BuscarAsync(v => 
                v.UnidadeFranqueadaId == unidadeId && 
                v.DataVenda.Month == mes && 
                v.DataVenda.Year == ano);

            // 2. Calcula o faturamento total somando o valor de cada venda encontrada
            var faturamentoTotal = vendas.Sum(v => v.ValorTotal);

            // 3. Monta um "relatório" estruturado para devolver ao usuário
            var relatorio = new 
            {
                UnidadeId = unidadeId,
                Mes = mes,
                Ano = ano,
                TotalVendasRealizadas = vendas.Count(),
                FaturamentoTotal = faturamentoTotal,
                Vendas = vendas // Devolve também a lista detalhada para conferência
            };

            return Ok(relatorio);
        }
    }
}