using Franquias.Api.Data;
using Franquias.Api.Repositories;
using Franquias.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. INJEÇÃO DE DEPENDÊNCIA DO BANCO DE DADOS (SQLITE)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. INJEÇÃO DO REPOSITÓRIO GENÉRICO E DOS SERVIÇOS
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IVendasService, VendasService>();

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

// 5. SWAGGER
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

app.UseAuthorization();
app.MapControllers();


app.Run();