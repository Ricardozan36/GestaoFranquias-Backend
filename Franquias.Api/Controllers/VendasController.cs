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

        // --- PORTA DE ENTRADA (Cria novas vendas) ---
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

        // --- PORTA DE SAÍDA (Envia as vendas para o Dashboard) ---
        [HttpGet]
        public async Task<IActionResult> ListarVendas()
        {
            try
            {
                // ATENÇÃO: O nome do método abaixo (ObterTodasAsync) pode estar um pouquinho diferente 
                // no seu IVendasService (ex: ListarVendasAsync, BuscarTodas, etc). 
                // Se ficar com sublinhado vermelho, apenas troque para o nome correto que está no seu Service!
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