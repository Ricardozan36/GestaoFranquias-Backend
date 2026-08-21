using Franquias.Api.DTOs;
using Franquias.Api.Entities;
using Franquias.Api.Repositories;
using Franquias.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Franquias.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRepository<Usuario> _usuarioRepository;
        private readonly TokenService _tokenService;

        public AuthController(IRepository<Usuario> usuarioRepository, TokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            
            var usuarios = await _usuarioRepository.BuscarAsync(u => u.Email == login.Email);
            var usuarioDb = usuarios.FirstOrDefault();

            
            if (usuarioDb == null || usuarioDb.SenhaHash != login.Senha)
            {
                return Unauthorized(new { mensagem = "E-mail ou senha inválidos." });
            }

            
            if (!usuarioDb.Ativo)
            {
                return BadRequest(new { mensagem = "Usuário inativo. Procure o administrador." });
            }

            
            var tokenString = _tokenService.GerarToken(usuarioDb);

            
            return Ok(new 
            { 
                token = tokenString, 
                perfil = usuarioDb.Perfil.ToString() 
            });
        }
    }
}