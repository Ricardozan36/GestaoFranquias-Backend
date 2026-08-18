using System.Collections.Generic;

namespace Franquias.Api.DTOs
{
    public class NovaVendaDTO
    {
        public int UnidadeFranqueadaId { get; set; }
        public List<ItemVendaDTO> Itens { get; set; } = new();
    }

    public class ItemVendaDTO
    {
        public int ProdutoServicoId { get; set; }
        public int Quantidade { get; set; }
    }
}