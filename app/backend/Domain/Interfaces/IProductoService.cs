using Domain.DTOs;

namespace Domain.Interfaces;

public interface IProductoService
{
    Task<List<ProductoDTO>> ListarAsync(CancellationToken ct = default);
    Task<ProductoDTO?> ObtenerAsync(string codigo, CancellationToken ct = default);
    Task CrearAsync(ProductoDTO dto, CancellationToken ct = default);
    Task<bool> ActualizarAsync(string codigo, ProductoDTO dto, CancellationToken ct = default);
    Task<bool> EliminarAsync(string codigo, CancellationToken ct = default);
}