using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities;

public class Transaccion
{
    [Key]
    public int IdTransaccion { get; set; }

    [Required, StringLength(100)]
    public string CodigoProducto { get; set; } = default!;

    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    // "ENTRADA" | "SALIDA"
    [Required, StringLength(20)]
    public string Tipo { get; set; } = default!;

    public int Cantidad { get; set; }

    public string? Referencia { get; set; } // p.ej., IdFactura

    [JsonIgnore] public Producto Producto { get; set; } = default!;
}