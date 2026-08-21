using Franquias.Api.Entities;
using Franquias.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using Microsoft.AspNetCore.Authorization;

namespace Franquias.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UnidadesController : ControllerBase
    {
        private readonly IRepository<UnidadeFranqueada> _repository;

        public UnidadesController(IRepository<UnidadeFranqueada> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodas()
        {
            var unidades = await _repository.ObterTodosAsync();
            return Ok(unidades);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var unidade = await _repository.ObterPorIdAsync(id);
            if (unidade == null) 
                return NotFound(new { mensagem = "Unidade não encontrada." });
                
            return Ok(unidade);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] UnidadeFranqueada unidade)
        {
            try
            {
                
                await _repository.AdicionarAsync(unidade);
                return CreatedAtAction(nameof(ObterPorId), new { id = unidade.Id }, unidade);
            }
            
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UNIQUE constraint failed") == true)
            {
                return BadRequest(new { mensagem = "Erro de validação: Já existe uma unidade cadastrada com este CNPJ." });
            }
            
            catch (Exception)
            {
                return StatusCode(500, new { mensagem = "Ocorreu um erro interno ao tentar salvar a unidade." });
            }
        }
    }
}