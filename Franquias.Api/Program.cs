using Franquias.Api.Data;
using Franquias.Api.Repositories;
using Franquias.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. INJEÇÃO DE DEPENDÊNCIA DO BANCO DE DADOS (SQLITE)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. INJEÇÃO DO REPOSITÓRIO GENÉRICO E DOS SERVIÇOS
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IVendasService, VendasService>();
builder.Services.AddScoped<TokenService>();

// ---- INÍCIO DA CONFIGURAÇÃO DO JWT ----
var jwtConfig = builder.Configuration.GetSection("Jwt");
var secretKey = jwtConfig["Key"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Como é local (dev), pode ser false
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey ?? "")),
        ValidateIssuer = true,
        ValidIssuer = jwtConfig["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtConfig["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});
// ---- FIM DA CONFIGURAÇÃO DO JWT ----

// 3. CONTROLLERS E TRATAMENTO DE CICLO INFINITO JSON
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

// 4. CONFIGURAÇÃO DO CORS (Libera acesso para qualquer Front-End)
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTudo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 5. SWAGGER SIMPLES (Restaurado para compilar sem erros)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 6. ATIVA O CORS ANTES DA AUTORIZAÇÃO
app.UseCors("PermitirTudo");

// 7. PROTEÇÃO DAS ROTAS E NÍVEL DE ACESSO
app.UseAuthentication(); // <-- Confere se a pessoa tem o Token (Identidade)
app.UseAuthorization();  // <-- Confere o perfil da pessoa (Acesso)

// ---- INÍCIO DO SEMEADOR DE DADOS (SEED) ----
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<Franquias.Api.Data.AppDbContext>();
        Franquias.Api.Data.DbInitializer.Initialize(context);
        Console.WriteLine("Banco de dados verificado e populado com sucesso!");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Ocorreu um erro ao popular o banco de dados: " + ex.Message);
        if (ex.InnerException != null)
        {
            Console.WriteLine("O CULPADO É: " + ex.InnerException.Message);
        }
    }
}
// ---- FIM DO SEMEADOR DE DADOS ----

app.MapControllers();
app.Run();