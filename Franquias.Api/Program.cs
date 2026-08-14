using Franquias.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. INJEÇÃO DE DEPENDÊNCIA DO BANCO DE DADOS (SQLITE)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Adiciona o suporte para a arquitetura de Controllers exigida no trabalho
builder.Services.AddControllers();

// 3. Adiciona o Swagger para testarmos a API visualmente
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

// 4. Avisa o sistema para procurar as nossas rotas dentro da pasta Controllers
app.MapControllers();

app.Run();