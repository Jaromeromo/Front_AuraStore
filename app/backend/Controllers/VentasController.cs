using Domain.DTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class VentasController : ControllerBase
{
    private readonly IVentaService _ventaService;
    public VentasController(IVentaService ventaService) => _ventaService = ventaService;

    /// <summary>Crea una venta (Factura + Detalle + Transacciones).</summary>
    [HttpPost]
    [ProducesResponseType(typeof(FacturaDetalleDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<FacturaDetalleDTO>> Post([FromBody] CrearVentaDTO dto, CancellationToken ct)
    {
        var result = await _ventaService.CrearVentaAsync(dto, ct);
        return Ok(result);
    }
}