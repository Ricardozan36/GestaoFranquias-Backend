using Franquias.Api.Entities;
using Franquias.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Franquias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChamadosController : ControllerBase
    {
        private readonly IRepository<ChamadoSuporte> _repository;

        public ChamadosController(IRepository<ChamadoSuporte> repository)
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
            var chamado = await _repository.ObterPorIdAsync(id);
            if (chamado == null) 
                return NotFound(new { mensagem = "Chamado não encontrado." });
                
            return Ok(chamado);
        }

        [HttpGet("unidade/{unidadeId}")]
        public async Task<IActionResult> ObterPorUnidade(int unidadeId)
        {
            var chamados = await _repository.BuscarAsync(c => c.UnidadeFranqueadaId == unidadeId);
            return Ok(chamados);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] ChamadoSuporte chamado)
        {
            await _repository.AdicionarAsync(chamado);
            return CreatedAtAction(nameof(ObterPorId), new { id = chamado.Id }, chamado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] ChamadoSuporte chamadoAtualizado)
        {
            if (id != chamadoAtualizado.Id) 
                return BadRequest(new { mensagem = "O ID da URL não corresponde ao do corpo." });

            await _repository.AtualizarAsync(chamadoAtualizado);
            return NoContent();
        }
    }
}