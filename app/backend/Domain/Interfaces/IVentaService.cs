using Domain.DTOs;

namespace Domain.Interfaces;

public interface IVentaService
{
    Task<FacturaDetalleDTO> CrearVentaAsync(CrearVentaDTO dto, CancellationToken ct = default);
}