namespace Franquias.Api.Entities
{
    public class Fornecedor
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CNPJ { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
    }

    public class ProdutoServico
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal PrecoBase { get; set; }
        public bool Ativo { get; set; } = true;
    }

    public class Estoque
    {
        public int Id { get; set; }
        public int Quantidade { get; set; }
        
        // Relacionamentos
        public int ProdutoServicoId { get; set; }
        public ProdutoServico? Produto { get; set; }
        
        public int UnidadeFranqueadaId { get; set; }
        public UnidadeFranqueada? Unidade { get; set; }
    }
}