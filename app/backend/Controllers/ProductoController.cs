using Domain.DTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace app.backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductoController : ControllerBase
{
    private readonly IProductoService _service;

    public ProductoController(IProductoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductoDTO>>> Listar(CancellationToken ct)
    {
        var productos = await _service.ListarAsync(ct);
        return Ok(productos);
    }

    [HttpGet("{codigo}")]
    public async Task<ActionResult<ProductoDTO>> Obtener(string codigo, CancellationToken ct)
    {
        var producto = await _service.ObtenerAsync(codigo, ct);
        if (producto is null) return NotFound();
        return Ok(producto);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] ProductoDTO dto, CancellationToken ct)
    {
        await _service.CrearAsync(dto, ct);
        return CreatedAtAction(nameof(Obtener), new { codigo = dto.CodigoProducto }, dto);
    }

    [HttpPut("{codigo}")]
    public async Task<IActionResult> Actualizar(string codigo, [FromBody] ProductoDTO dto, CancellationToken ct)
    {
        var ok = await _service.ActualizarAsync(codigo, dto, ct);
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpDelete("{codigo}")]
    public async Task<IActionResult> Eliminar(string codigo, CancellationToken ct)
    {
        var ok = await _service.EliminarAsync(codigo, ct);
        if (!ok) return NotFound();
        return NoContent();
    }
}