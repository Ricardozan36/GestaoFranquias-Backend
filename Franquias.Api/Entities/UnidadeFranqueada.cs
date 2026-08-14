using System;

namespace Franquias.Api.Entities
{
    public class Franqueadora
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CNPJ { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public decimal PercentualRoyaltyPadrao { get; set; }
    }

    public class UnidadeFranqueada
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CNPJ { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string Responsavel { get; set; } = string.Empty; 
        public DateTime DataInicio { get; set; }
        public StatusUnidade Status { get; set; } = StatusUnidade.Ativa;
        
        // Chave Estrangeira e Relacionamento
        public int FranqueadoraId { get; set; }
        public Franqueadora? Franqueadora { get; set; }
    }
}