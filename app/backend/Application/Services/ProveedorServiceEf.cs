using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class ProveedorServiceEf : IProveedorService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public ProveedorServiceEf(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<List<ProveedorDTO>> ListarAsync(CancellationToken ct = default)
    {
        var proveedores = await _db.Proveedores.AsNoTracking().ToListAsync(ct);
        return _mapper.Map<List<ProveedorDTO>>(proveedores);
    }

    public async Task<ProveedorDTO?> ObtenerAsync(string nit, CancellationToken ct = default)
    {
        var proveedor = await _db.Proveedores
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Nit == nit, ct);

        return proveedor is null ? null : _mapper.Map<ProveedorDTO>(proveedor);
    }

    public async Task CrearAsync(ProveedorDTO dto, CancellationToken ct = default)
    {
        var existe = await _db.Proveedores.AnyAsync(p => p.Nit == dto.Nit, ct);
        if (existe)
            throw new InvalidOperationException($"El proveedor con NIT {dto.Nit} ya existe.");

        var entity = _mapper.Map<Proveedor>(dto);
        _db.Proveedores.Add(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<bool> ActualizarAsync(string nit, ProveedorDTO dto, CancellationToken ct = default)
    {
        var proveedor = await _db.Proveedores.FirstOrDefaultAsync(p => p.Nit == nit, ct);
        if (proveedor is null) return false;

        proveedor.Nombre = dto.Nombre;
        proveedor.Contacto = dto.Contacto;

        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> EliminarAsync(string nit, CancellationToken ct = default)
    {
        var proveedor = await _db.Proveedores.FirstOrDefaultAsync(p => p.Nit == nit, ct);
        if (proveedor is null) return false;

        _db.Proveedores.Remove(proveedor);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}