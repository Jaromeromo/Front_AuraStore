using Domain.DTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace app.backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProveedorController : ControllerBase
{
    private readonly IProveedorService _service;

    public ProveedorController(IProveedorService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProveedorDTO>>> Listar(CancellationToken ct)
    {
        var proveedores = await _service.ListarAsync(ct);
        return Ok(proveedores);
    }

    [HttpGet("{nit}")]
    public async Task<ActionResult<ProveedorDTO>> Obtener(string nit, CancellationToken ct)
    {
        var proveedor = await _service.ObtenerAsync(nit, ct);
        if (proveedor is null) return NotFound();
        return Ok(proveedor);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] ProveedorDTO dto, CancellationToken ct)
    {
        await _service.CrearAsync(dto, ct);
        return CreatedAtAction(nameof(Obtener), new { nit = dto.Nit }, dto);
    }

    [HttpPut("{nit}")]
    public async Task<IActionResult> Actualizar(string nit, [FromBody] ProveedorDTO dto, CancellationToken ct)
    {
        var ok = await _service.ActualizarAsync(nit, dto, ct);
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpDelete("{nit}")]
    public async Task<IActionResult> Eliminar(string nit, CancellationToken ct)
    {
        var ok = await _service.EliminarAsync(nit, ct);
        if (!ok) return NotFound();
        return NoContent();
    }
}