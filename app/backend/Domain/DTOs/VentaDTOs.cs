namespace Domain.DTOs;

public record ItemVentaDTO(string CodigoProducto, int Cantidad, decimal PrecioUnitario);

public record CrearVentaDTO(int? IdCliente, List<ItemVentaDTO> Items);

public record ItemFacturaDTO(string CodigoProducto, string Nombre, int Cantidad, decimal PrecioUnitario, decimal Importe);

public record FacturaDetalleDTO(
    string IdFactura,
    DateTime Fecha,
    int? IdCliente,
    decimal Subtotal,
    decimal Iva,
    decimal Total,
    List<ItemFacturaDTO> Items
);
