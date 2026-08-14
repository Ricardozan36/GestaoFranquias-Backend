using Franquias.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Franquias.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Franqueadora> Franqueadoras { get; set; }
        public DbSet<UnidadeFranqueada> Unidades { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<ProdutoServico> ProdutosServicos { get; set; }
        public DbSet<Estoque> Estoques { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<ItemVenda> ItensVenda { get; set; }
        public DbSet<Royalty> Royalties { get; set; }
        public DbSet<ChamadoSuporte> Chamados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cumprindo as regras de negócio obrigatórias do trabalho (Validações Únicas)
            modelBuilder.Entity<Usuario>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<UnidadeFranqueada>().HasIndex(u => u.CNPJ).IsUnique();
        }
    }
}