using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class ProductoServiceEf : IProductoService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;
    public ProductoServiceEf(AppDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<List<ProductoDTO>> ListarAsync(CancellationToken ct = default) =>
        await _db.Productos.AsNoTracking().Select(p => _mapper.Map<ProductoDTO>(p)).ToListAsync(ct);

    public async Task<ProductoDTO?> ObtenerAsync(string codigo, CancellationToken ct = default)
    {
        var p = await _db.Productos.AsNoTracking().FirstOrDefaultAsync(x => x.CodigoProducto == codigo, ct);
        return p is null ? null : _mapper.Map<ProductoDTO>(p);
    }

    public async Task CrearAsync(ProductoDTO dto, CancellationToken ct = default)
    {
        if (await _db.Productos.AnyAsync(x => x.CodigoProducto == dto.CodigoProducto, ct))
            throw new InvalidOperationException("El producto ya existe.");
        var entity = _mapper.Map<Producto>(dto);
        _db.Productos.Add(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<bool> ActualizarAsync(string codigo, ProductoDTO dto, CancellationToken ct = default)
    {
        var p = await _db.Productos.FirstOrDefaultAsync(x => x.CodigoProducto == codigo, ct);
        if (p is null) return false;
        p.Nombre = dto.Nombre;
        p.Descripcion = dto.Descripcion;
        p.PrecioUnitario = dto.PrecioUnitario;
        p.Stock = dto.Stock;
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> EliminarAsync(string codigo, CancellationToken ct = default)
    {
        var p = await _db.Productos.FirstOrDefaultAsync(x => x.CodigoProducto == codigo, ct);
        if (p is null) return false;
        _db.Productos.Remove(p);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}