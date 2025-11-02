using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities;

public class ProductoProveedor
{
    [StringLength(100)]
    public string Nit { get; set; } = default!;
    [StringLength(100)]
    public string CodigoProducto { get; set; } = default!;

    [JsonIgnore] public Proveedor Proveedor { get; set; } = default!;
    [JsonIgnore] public Producto Producto { get; set; } = default!;
}