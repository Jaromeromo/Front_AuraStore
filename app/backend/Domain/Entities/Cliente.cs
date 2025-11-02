using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Cliente
{
    [Key]
    public int IdCliente { get; set; }

    [Required, StringLength(200)]
    public string Nombre { get; set; } = default!;

    [StringLength(200)]
    public string? Email { get; set; }

    [StringLength(20)]
    public string? Telefono { get; set; }

    // [JsonIgnore] public ICollection<Factura> Facturas { get; set; } = new List<Factura>();
}