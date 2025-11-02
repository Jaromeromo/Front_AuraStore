using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Proveedor> Proveedores => Set<Proveedor>();
    public DbSet<ProductoProveedor> ProductoProveedores => Set<ProductoProveedor>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Factura> Facturas => Set<Factura>();
    public DbSet<FacturaProducto> FacturaProductos => Set<FacturaProducto>();
    public DbSet<Transaccion> Transacciones => Set<Transaccion>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        // ProductoProveedor (compuesta)
        mb.Entity<ProductoProveedor>()
          .HasKey(pp => new { pp.Nit, pp.CodigoProducto });

        mb.Entity<ProductoProveedor>()
          .HasOne(pp => pp.Proveedor)
          .WithMany(p => p.ProductoProveedores)
          .HasForeignKey(pp => pp.Nit);

        mb.Entity<ProductoProveedor>()
          .HasOne(pp => pp.Producto)
          .WithMany(p => p.ProductoProveedores)
          .HasForeignKey(pp => pp.CodigoProducto);

        // FacturaProducto (compuesta)
        mb.Entity<FacturaProducto>()
          .HasKey(fp => new { fp.IdFactura, fp.CodigoProducto });

        mb.Entity<FacturaProducto>()
          .HasOne(fp => fp.Factura)
          .WithMany(f => f.FacturaProductos)
          .HasForeignKey(fp => fp.IdFactura);

        mb.Entity<FacturaProducto>()
          .HasOne(fp => fp.Producto)
          .WithMany(p => p.FacturaProductos)
          .HasForeignKey(fp => fp.CodigoProducto);

        // Transaccion → Producto
        mb.Entity<Transaccion>()
          .HasOne(t => t.Producto)
          .WithMany(p => p.Transacciones)
          .HasForeignKey(t => t.CodigoProducto);

        // Indices útiles
        mb.Entity<Producto>()
          .HasIndex(p => p.Nombre);

        mb.Entity<Factura>()
          .HasIndex(f => f.Fecha);
    }
}