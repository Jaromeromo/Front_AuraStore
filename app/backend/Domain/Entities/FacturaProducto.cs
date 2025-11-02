using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities;

public class FacturaProducto
{
    [StringLength(100)]
    public string IdFactura { get; set; } = default!;
    [StringLength(100)]
    public string CodigoProducto { get; set; } = default!;

    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }

    [JsonIgnore] public Factura Factura { get; set; } = default!;
    [JsonIgnore] public Producto Producto { get; set; } = default!;
}