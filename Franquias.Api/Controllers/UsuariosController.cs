using Franquias.Api.Entities;
using Franquias.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Franquias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IRepository<Usuario> _repository;

        public UsuariosController(IRepository<Usuario> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var usuarios = await _repository.ObterTodosAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var usuario = await _repository.ObterPorIdAsync(id);
            if (usuario == null) 
                return NotFound(new { mensagem = "Usuário não encontrado." });
                
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] Usuario usuario)
        {
            await _repository.AdicionarAsync(usuario);
            return CreatedAtAction(nameof(ObterPorId), new { id = usuario.Id }, usuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] Usuario usuarioAtualizado)
        {
            if (id != usuarioAtualizado.Id) 
                return BadRequest(new { mensagem = "O ID da URL não corresponde ao ID do corpo da requisição." });

            await _repository.AtualizarAsync(usuarioAtualizado);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var usuario = await _repository.ObterPorIdAsync(id);
            if (usuario == null) 
                return NotFound(new { mensagem = "Usuário não encontrado." });

            await _repository.RemoverAsync(usuario);
            return NoContent();
        }
    }
}