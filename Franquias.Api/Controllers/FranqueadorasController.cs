using Franquias.Api.Entities;
using Franquias.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Franquias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FranqueadorasController : ControllerBase
    {
        private readonly IRepository<Franqueadora> _repository;

        
        public FranqueadorasController(IRepository<Franqueadora> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodas()
        {
            var franqueadoras = await _repository.ObterTodosAsync();
            return Ok(franqueadoras);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var franqueadora = await _repository.ObterPorIdAsync(id);
            if (franqueadora == null) 
                return NotFound(new { mensagem = "Franqueadora não encontrada." });
                
            return Ok(franqueadora);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] Franqueadora franqueadora)
        {
            await _repository.AdicionarAsync(franqueadora);
            return CreatedAtAction(nameof(ObterPorId), new { id = franqueadora.Id }, franqueadora);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var franqueadora = await _repository.ObterPorIdAsync(id);
            if (franqueadora == null) 
                return NotFound(new { mensagem = "Franqueadora não encontrada." });

            await _repository.RemoverAsync(franqueadora);
            return NoContent();
        }
    }
}