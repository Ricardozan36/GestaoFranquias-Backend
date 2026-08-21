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
        public DbSet<ProdutoServico> Produtos { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Estoque> Estoques { get; set; }
        public DbSet<Venda> Vendas { get; set; }

        
        public DbSet<ChamadoSuporte> Chamados { get; set; }
        public DbSet<Royalty> Royalties { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UnidadeFranqueada>()
                .HasIndex(u => u.CNPJ)
                .IsUnique();
        }
    }
}