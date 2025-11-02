using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities;

public class Producto
{
    [Key, StringLength(100)]
    public string CodigoProducto { get; set; } = default!;

    [Required, StringLength(200)]
    public string Nombre { get; set; } = default!;

    [StringLength(500)]
    public string? Descripcion { get; set; }

    public decimal PrecioUnitario { get; set; }
    public int Stock { get; set; }

    [JsonIgnore] public ICollection<FacturaProducto> FacturaProductos { get; set; } = new List<FacturaProducto>();
    [JsonIgnore] public ICollection<ProductoProveedor> ProductoProveedores { get; set; } = new List<ProductoProveedor>();
    [JsonIgnore] public ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
}