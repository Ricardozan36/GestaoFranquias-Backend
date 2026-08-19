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

    

    
}