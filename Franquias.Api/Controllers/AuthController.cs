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
            // 1. Busca o usuário no banco pelo e-mail
            var usuarios = await _usuarioRepository.BuscarAsync(u => u.Email == login.Email);
            var usuarioDb = usuarios.FirstOrDefault();

            // 2. Valida se o usuário existe e se a senha está correta
            // NOTA: Em produção, usaríamos BCrypt para comparar hashes. 
            // Para o escopo acadêmico, comparamos diretamente com a SenhaHash armazenada.
            if (usuarioDb == null || usuarioDb.SenhaHash != login.Senha)
            {
                return Unauthorized(new { mensagem = "E-mail ou senha inválidos." });
            }

            // 3. Verifica se o usuário está ativo (Regra de negócio)
            if (!usuarioDb.Ativo)
            {
                return BadRequest(new { mensagem = "Usuário inativo. Procure o administrador." });
            }

            // 4. Gera o Token JWT usando o nosso TokenService
            var tokenString = _tokenService.GerarToken(usuarioDb);

            // 5. Retorna o Token e o nível de acesso para o Front-End
            return Ok(new 
            { 
                token = tokenString, 
                perfil = usuarioDb.Perfil.ToString() 
            });
        }
    }
}