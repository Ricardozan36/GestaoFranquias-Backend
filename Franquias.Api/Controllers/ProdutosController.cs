using Franquias.Api.Entities;
using Franquias.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Franquias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IRepository<ProdutoServico> _repository;

        public ProdutosController(IRepository<ProdutoServico> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var produtos = await _repository.ObterTodosAsync();
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var produto = await _repository.ObterPorIdAsync(id);
            if (produto == null) return NotFound(new { mensagem = "Produto não encontrado." });
            return Ok(produto);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] ProdutoServico produto)
        {
            await _repository.AdicionarAsync(produto);
            return CreatedAtAction(nameof(ObterPorId), new { id = produto.Id }, produto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] ProdutoServico produtoAtualizado)
        {
            if (id != produtoAtualizado.Id) 
                return BadRequest(new { mensagem = "O ID da URL não corresponde ao ID do corpo da requisição." });

            await _repository.AtualizarAsync(produtoAtualizado);
            return NoContent(); 
        }
    }
}