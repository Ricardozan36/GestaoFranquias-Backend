using System;
using System.Collections.Generic;

namespace Franquias.Api.Entities
{
    public class Venda
    {
        public int Id { get; set; }
        public DateTime DataVenda { get; set; } = DateTime.Now;
        public decimal ValorTotal { get; set; }
        
        public int UnidadeFranqueadaId { get; set; }
        public UnidadeFranqueada? Unidade { get; set; }
        
        public List<ItemVenda> Itens { get; set; } = new();
    }

    public class ItemVenda
    {
        public int Id { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        
        public int VendaId { get; set; }
        public Venda? Venda { get; set; }
        
        public int ProdutoServicoId { get; set; }
        public ProdutoServico? Produto { get; set; }
    }

    public class Royalty
    {
        public int Id { get; set; }
        public int MesRef { get; set; }
        public int AnoRef { get; set; }
        public decimal FaturamentoBase { get; set; }
        public decimal ValorCobrado { get; set; }
        public bool Pago { get; set; }

        public int UnidadeFranqueadaId { get; set; }
        public UnidadeFranqueada? Unidade { get; set; }
    }

    public class ChamadoSuporte
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Prioridade { get; set; } = string.Empty;
        public StatusChamado Status { get; set; } = StatusChamado.Aberto;
        public DateTime DataAbertura { get; set; } = DateTime.Now;
        public DateTime? DataFechamento { get; set; }

        public int UnidadeFranqueadaId { get; set; }
        public UnidadeFranqueada? Unidade { get; set; }
    }
}