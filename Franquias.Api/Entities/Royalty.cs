namespace Franquias.Api.Entities
{
    public class Royalty
    {
        public int Id { get; set; }
        public int UnidadeFranqueadaId { get; set; }
        public UnidadeFranqueada? Unidade { get; set; }
        
        public int Mes { get; set; }
        public int Ano { get; set; }
        
        public decimal ValorFaturamento { get; set; }
        public decimal PercentualAplicado { get; set; }
        public decimal ValorCobrado { get; set; }
        
        
        public string StatusPagamento { get; set; } = "Pendente";
        
        public DateTime DataGeracao { get; set; } = DateTime.Now;
        public DateTime DataVencimento { get; set; }
        public DateTime? DataPagamento { get; set; }
    }
}