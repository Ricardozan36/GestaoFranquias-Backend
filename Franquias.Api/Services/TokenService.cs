using Franquias.Api.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Franquias.Api.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GerarToken(Usuario usuario)
        {
            // 1. Pega as configurações lá do appsettings.json
            var jwtConfig = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtConfig["Key"] ?? "");

            // 2. Coloca os dados do usuário (o "crachá") dentro do token
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email),
                // Aqui nós chamamos a propriedade "Perfil" que você criou no seu enum!
                new Claim(ClaimTypes.Role, usuario.Perfil.ToString()) 
            };

            // 3. Monta e criptografa o Token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8), // O Token vale por 8 horas
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtConfig["Issuer"],
                Audience = jwtConfig["Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // 4. Devolve o Token pronto como uma string gigante
            return tokenHandler.WriteToken(token);
        }
    }
}