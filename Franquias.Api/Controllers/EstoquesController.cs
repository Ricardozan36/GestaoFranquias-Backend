using Franquias.Api.DTOs;
using Franquias.Api.Entities;
using Franquias.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Franquias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstoquesController : ControllerBase
    {
        private readonly IRepository<Estoque> _repository;

        public EstoquesController(IRepository<Estoque> repository)
        {
            _repository = repository;
        }

        [HttpGet("unidade/{unidadeId}")]
        public async Task<IActionResult> ObterEstoqueDaUnidade(int unidadeId)
        {
            var estoques = await _repository.BuscarAsync(e => e.UnidadeFranqueadaId == unidadeId);
            return Ok(estoques);
        }

        [HttpPost("movimentar")]
        public async Task<IActionResult> MovimentarEstoque([FromBody] NovoEstoqueDTO dto)
        {
            if (dto.Quantidade <= 0) 
                return BadRequest(new { mensagem = "A quantidade de entrada deve ser maior que zero." });

            var estoques = await _repository.BuscarAsync(e => 
                e.UnidadeFranqueadaId == dto.UnidadeFranqueadaId && 
                e.ProdutoServicoId == dto.ProdutoServicoId);
            
            var estoqueExistente = estoques.FirstOrDefault();

            if (estoqueExistente != null)
            {
                estoqueExistente.Quantidade += dto.Quantidade;
                await _repository.AtualizarAsync(estoqueExistente);
            }
            else
            {
                var novoEstoque = new Estoque
                {
                    UnidadeFranqueadaId = dto.UnidadeFranqueadaId,
                    ProdutoServicoId = dto.ProdutoServicoId,
                    Quantidade = dto.Quantidade
                };
                await _repository.AdicionarAsync(novoEstoque);
            }

            return Ok(new { mensagem = "Estoque atualizado com sucesso!" });
        }
    }
}