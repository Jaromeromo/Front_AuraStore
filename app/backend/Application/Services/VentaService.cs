using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class VentaService : IVentaService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public VentaService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<FacturaDetalleDTO> CrearVentaAsync(CrearVentaDTO dto, CancellationToken ct = default)
    {
        if (dto.Items is null || dto.Items.Count == 0)
            throw new InvalidOperationException("La venta debe contener al menos un ítem.");

        await using var tx = await _db.Database.BeginTransactionAsync(ct);

        // 1) Crear Factura (Id generado simple; usa GUID/seq según tu estándar)
        var idFactura = $"FAC-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
        var factura = new Factura
        {
            IdFactura = idFactura,
            Fecha = DateTime.UtcNow,
            IdCliente = dto.IdCliente
        };
        _db.Facturas.Add(factura);
        await _db.SaveChangesAsync(ct);

        decimal subtotal = 0m;

        // 2) Por cada item: valida stock, descuenta, agrega FacturaProducto y Transaccion (SALIDA)
        foreach (var item in dto.Items)
        {
            var producto = await _db.Productos
                .FirstOrDefaultAsync(p => p.CodigoProducto == item.CodigoProducto, ct)
                ?? throw new InvalidOperationException($"Producto {item.CodigoProducto} no existe.");

            if (item.Cantidad <= 0)
                throw new InvalidOperationException($"Cantidad inválida para {item.CodigoProducto}.");

            if (item.PrecioUnitario <= 0)
                throw new InvalidOperationException($"PrecioUnitario inválido para {item.CodigoProducto}.");

            if (producto.Stock < item.Cantidad)
                throw new InvalidOperationException($"Stock insuficiente para {producto.CodigoProducto}. Disponible: {producto.Stock}");

            // Descuenta stock
            producto.Stock -= item.Cantidad;

            // Inserta detalle
            var fp = new FacturaProducto
            {
                IdFactura = idFactura,
                CodigoProducto = item.CodigoProducto,
                Cantidad = item.Cantidad,
                PrecioUnitario = item.PrecioUnitario
            };
            _db.FacturaProductos.Add(fp);

            // Transacción de inventario (SALIDA)
            var t = new Transaccion
            {
                CodigoProducto = item.CodigoProducto,
                Tipo = "SALIDA",
                Cantidad = item.Cantidad,
                Referencia = idFactura,
                Fecha = DateTime.UtcNow
            };
            _db.Transacciones.Add(t);

            subtotal += item.Cantidad * item.PrecioUnitario;
        }

        // 3) Totales
        var iva = Math.Round(subtotal * 0.19m, 2); // ajusta regla
        var total = subtotal + iva;

        factura.Subtotal = subtotal;
        factura.Iva = iva;
        factura.Total = total;

        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);

        // 4) Materializa DTO de respuesta
        var items = await _db.FacturaProductos
            .Where(x => x.IdFactura == idFactura)
            .Include(x => x.Producto)
            .AsNoTracking()
            .ToListAsync(ct);

        var itemsDto = items.Select(fp => _mapper.Map<ItemFacturaDTO>(fp)).ToList();

        return new FacturaDetalleDTO(
            factura.IdFactura,
            factura.Fecha,
            factura.IdCliente,
            factura.Subtotal,
            factura.Iva,
            factura.Total,
            itemsDto
        );
    }
}