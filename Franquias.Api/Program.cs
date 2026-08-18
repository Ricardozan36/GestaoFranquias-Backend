using Franquias.Api.Data;
using Franquias.Api.Repositories; // <-- Esta é a linha mágica que estava faltando!
using Microsoft.EntityFrameworkCore;
using Franquias.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. INJEÇÃO DE DEPENDÊNCIA DO BANCO DE DADOS (SQLITE)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. INJEÇÃO DO REPOSITÓRIO GENÉRICO
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IVendasService, VendasService>();

// 3. Adiciona o suporte para a arquitetura de Controllers exigida no trabalho
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

// 4. Adiciona o Swagger para testarmos a API visualmente
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// 5. Avisa o sistema para procurar as nossas rotas dentro da pasta Controllers
app.MapControllers();

app.Run();