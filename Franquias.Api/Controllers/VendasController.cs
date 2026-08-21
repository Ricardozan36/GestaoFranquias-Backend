using Franquias.Api.DTOs;
using Franquias.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Franquias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendasController : ControllerBase
    {
        private readonly IVendasService _vendasService;

        public VendasController(IVendasService vendasService)
        {
            _vendasService = vendasService;
        }

        
        [HttpPost]
        public async Task<IActionResult> RegistrarVenda([FromBody] NovaVendaDTO dto)
        {
            try
            {
                var vendaRealizada = await _vendasService.RegistrarVendaAsync(dto);
                return Created("", vendaRealizada);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        
        [HttpGet]
        public async Task<IActionResult> ListarVendas()
        {
            try
            {
                
                var vendas = await _vendasService.ObterTodasAsync(); 
                return Ok(vendas);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}