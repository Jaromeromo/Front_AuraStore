using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities;

public class Proveedor
{
    [Key, StringLength(100)]
    public string Nit { get; set; } = default!;

    [Required, StringLength(200)]
    public string Nombre { get; set; } = default!;

    [StringLength(200)]
    public string? Contacto { get; set; }

    [JsonIgnore] public ICollection<ProductoProveedor> ProductoProveedores { get; set; } = new List<ProductoProveedor>();
}