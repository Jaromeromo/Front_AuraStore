using Domain.DTOs;

namespace Domain.Interfaces;

public interface IProveedorService
{
    Task<List<ProveedorDTO>> ListarAsync(CancellationToken ct = default);
    Task<ProveedorDTO?> ObtenerAsync(string nit, CancellationToken ct = default);
    Task CrearAsync(ProveedorDTO dto, CancellationToken ct = default);
    Task<bool> ActualizarAsync(string nit, ProveedorDTO dto, CancellationToken ct = default);
    Task<bool> EliminarAsync(string nit, CancellationToken ct = default);
}