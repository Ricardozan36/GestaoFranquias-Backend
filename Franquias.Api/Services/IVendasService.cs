using Franquias.Api.DTOs;
using Franquias.Api.Entities;

namespace Franquias.Api.Services
{
    public interface IVendasService
    {
        Task<Venda> RegistrarVendaAsync(NovaVendaDTO dto);
    }
}