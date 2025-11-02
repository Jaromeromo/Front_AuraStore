namespace Domain.DTOs;

public record ProductoDTO(
    string CodigoProducto,
    string Nombre,
    string? Descripcion,
    decimal PrecioUnitario,
    int Stock
);