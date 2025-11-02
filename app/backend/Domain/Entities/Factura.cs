using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities;

public class Factura
{
    [Key, StringLength(100)]
    public string IdFactura { get; set; } = default!;

    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    public int? IdCliente { get; set; }
    public Cliente? Cliente { get; set; }

    public decimal Subtotal { get; set; }
    public decimal Iva { get; set; }
    public decimal Total { get; set; }

    [JsonIgnore] public ICollection<FacturaProducto> FacturaProductos { get; set; } = new List<FacturaProducto>();
}