using Franquias.Api.Entities;

namespace Franquias.Api.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            // Garante que o banco de dados está criado
            context.Database.EnsureCreated();

            // 1. Verifica se já existem usuários.
            if (context.Usuarios.Any())
            {
                return;   
            }

            // 2. Popula Usuários
            var usuarios = new Usuario[]
            {
                new Usuario { Nome = "Ricardo (Admin)", Email = "admin@franquia.com", SenhaHash = "123456", Perfil = Perfil.Administrador, Ativo = true }
            };
            context.Usuarios.AddRange(usuarios);
            context.SaveChanges();

            // 3. Popula a Franqueadora (A MÃE DAS UNIDADES - Resolve o Erro 19)
            var franqueadora = new Franqueadora
            {
                Nome = "Matriz Gestão de Franquias",
                CNPJ = "00.000.000/0001-00",
                Endereco = "Av. Principal, 1000",
                PercentualRoyaltyPadrao = 5.0m
            };
            context.Franqueadoras.Add(franqueadora);
            context.SaveChanges();

            // 4. Popula Unidades (Agora com a Chave Estrangeira FranqueadoraId preenchida!)
            var unidades = new UnidadeFranqueada[]
            {
                new UnidadeFranqueada { Nome = "Franquia Matriz - São Paulo", CNPJ = "11.111.111/0001-11", Endereco = "Av Paulista", Responsavel = "Carlos", Status = StatusUnidade.Ativa, FranqueadoraId = franqueadora.Id, DataInicio = DateTime.Now.AddYears(-1) },
                new UnidadeFranqueada { Nome = "Franquia Sul - Curitiba", CNPJ = "22.222.222/0002-22", Endereco = "Rua XV", Responsavel = "Ana", Status = StatusUnidade.Ativa, FranqueadoraId = franqueadora.Id, DataInicio = DateTime.Now.AddMonths(-6) },
                new UnidadeFranqueada { Nome = "Franquia Nordeste - Salvador", CNPJ = "33.333.333/0003-33", Endereco = "Pelourinho", Responsavel = "João", Status = StatusUnidade.Inativa, FranqueadoraId = franqueadora.Id, DataInicio = DateTime.Now.AddMonths(-2) }
            };
            context.Unidades.AddRange(unidades); 
            context.SaveChanges();

            // 5. Popula Fornecedores
            var fornecedor = new Fornecedor
            {
                Nome = "Fornecedor Tech Nacional",
                CNPJ = "99.999.999/0001-99",
                Ativo = true
            };
            context.Fornecedores.Add(fornecedor);
            context.SaveChanges();

            // 6. Popula Produtos
            var produtos = new ProdutoServico[]
            {
                new ProdutoServico { Nome = "Licença Software Gestão PRO", Categoria = "Software", PrecoBase = 1500.00m, Ativo = true, Descricao = "Licença anual do sistema." },
                new ProdutoServico { Nome = "Terminal de Pagamento", Categoria = "Hardware", PrecoBase = 850.00m, Ativo = true, Descricao = "Maquininha." }
            };
            context.Produtos.AddRange(produtos); 
            context.SaveChanges();

            // 7. Popula Estoque
            var estoques = new Estoque[]
            {
                new Estoque { ProdutoServicoId = 1, UnidadeFranqueadaId = 1, Quantidade = 50 },
                new Estoque { ProdutoServicoId = 2, UnidadeFranqueadaId = 2, Quantidade = 5 } // Estoque Crítico
            };
            context.Estoques.AddRange(estoques);
            context.SaveChanges();
        }
    }
}