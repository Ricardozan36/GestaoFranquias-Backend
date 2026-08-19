namespace Franquias.Api.Entities
{
    public class ChamadoSuporte
    {
        public int Id { get; set; }
        public int UnidadeFranqueadaId { get; set; }
        public UnidadeFranqueada? Unidade { get; set; }
        
        public string Categoria { get; set; } = string.Empty; // Ex: Sistema, Infraestrutura, Dúvida
        public string Prioridade { get; set; } = string.Empty; // Baixa, Média, Alta
        public string Descricao { get; set; } = string.Empty;
        
        // Status do chamado: Aberto, Em Andamento, Resolvido, Cancelado
        public string Status { get; set; } = "Aberto"; 
        
        public DateTime DataAbertura { get; set; } = DateTime.Now;
        public DateTime? DataEncerramento { get; set; }
        
        public string? RespostaFranqueadora { get; set; }
    }
}