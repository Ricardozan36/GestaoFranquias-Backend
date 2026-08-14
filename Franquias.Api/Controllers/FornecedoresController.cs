using Franquias.Api.Entities;
using Franquias.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Franquias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FornecedoresController : ControllerBase
    {
        private readonly IRepository<Fornecedor> _repository;

        public FornecedoresController(IRepository<Fornecedor> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var fornecedores = await _repository.ObterTodosAsync();
            return Ok(fornecedores);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var fornecedor = await _repository.ObterPorIdAsync(id);
            if (fornecedor == null) return NotFound(new { mensagem = "Fornecedor não encontrado." });
            return Ok(fornecedor);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] Fornecedor fornecedor)
        {
            await _repository.AdicionarAsync(fornecedor);
            return CreatedAtAction(nameof(ObterPorId), new { id = fornecedor.Id }, fornecedor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] Fornecedor fornecedorAtualizado)
        {
            if (id != fornecedorAtualizado.Id) 
                return BadRequest(new { mensagem = "IDs incompatíveis." });

            await _repository.AtualizarAsync(fornecedorAtualizado);
            return NoContent();
        }
    }
}