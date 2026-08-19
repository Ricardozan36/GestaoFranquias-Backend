using Franquias.Api.Entities;
using Franquias.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Franquias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoyaltiesController : ControllerBase
    {
        private readonly IRepository<Royalty> _repository;

        public RoyaltiesController(IRepository<Royalty> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            return Ok(await _repository.ObterTodosAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var royalty = await _repository.ObterPorIdAsync(id);
            if (royalty == null) 
                return NotFound(new { mensagem = "Royalty não encontrado." });
                
            return Ok(royalty);
        }

        [HttpGet("unidade/{unidadeId}")]
        public async Task<IActionResult> ObterPorUnidade(int unidadeId)
        {
            var royalties = await _repository.BuscarAsync(r => r.UnidadeFranqueadaId == unidadeId);
            return Ok(royalties);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] Royalty royalty)
        {
            await _repository.AdicionarAsync(royalty);
            return CreatedAtAction(nameof(ObterPorId), new { id = royalty.Id }, royalty);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] Royalty royaltyAtualizado)
        {
            if (id != royaltyAtualizado.Id) 
                return BadRequest(new { mensagem = "O ID da URL não corresponde ao do corpo." });

            await _repository.AtualizarAsync(royaltyAtualizado);
            return NoContent();
        }
    }
}