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

    public ProductoServiceEf(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<List<ProductoDTO>> ListarAsync(CancellationToken ct = default)
    {
        var productos = await _db.Productos.AsNoTracking().ToListAsync(ct);
        return _mapper.Map<List<ProductoDTO>>(productos);
    }

    public async Task<ProductoDTO?> ObtenerAsync(string codigo, CancellationToken ct = default)
    {
        var producto = await _db.Productos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.CodigoProducto == codigo, ct);

        return producto is null ? null : _mapper.Map<ProductoDTO>(producto);
    }

    public async Task CrearAsync(ProductoDTO dto, CancellationToken ct = default)
    {
        var existe = await _db.Productos.AnyAsync(p => p.CodigoProducto == dto.CodigoProducto, ct);
        if (existe)
            throw new InvalidOperationException($"El producto {dto.CodigoProducto} ya existe.");

        var entity = _mapper.Map<Producto>(dto);
        _db.Productos.Add(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<bool> ActualizarAsync(string codigo, ProductoDTO dto, CancellationToken ct = default)
    {
        var producto = await _db.Productos.FirstOrDefaultAsync(p => p.CodigoProducto == codigo, ct);
        if (producto is null) return false;

        producto.Nombre = dto.Nombre;
        producto.Descripcion = dto.Descripcion;
        producto.PrecioUnitario = dto.PrecioUnitario;
        producto.Stock = dto.Stock;

        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> EliminarAsync(string codigo, CancellationToken ct = default)
    {
        var producto = await _db.Productos.FirstOrDefaultAsync(p => p.CodigoProducto == codigo, ct);
        if (producto is null) return false;

        _db.Productos.Remove(producto);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}