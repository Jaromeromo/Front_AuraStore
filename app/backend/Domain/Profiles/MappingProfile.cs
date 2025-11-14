using AutoMapper;
using Domain.DTOs;
using Domain.Entities;

namespace Domain.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Producto
        CreateMap<Producto, ProductoDTO>().ReverseMap();

        // Proveedor
        CreateMap<Proveedor, ProveedorDTO>().ReverseMap();

        CreateMap<FacturaProducto, ItemFacturaDTO>()
            .ForCtorParam("CodigoProducto", o => o.MapFrom(s => s.CodigoProducto))
            .ForCtorParam("Nombre", o => o.MapFrom(s => s.Producto.Nombre))
            .ForCtorParam("Cantidad", o => o.MapFrom(s => s.Cantidad))
            .ForCtorParam("PrecioUnitario", o => o.MapFrom(s => s.PrecioUnitario))
            .ForCtorParam("Importe", o => o.MapFrom(s => s.Cantidad * s.PrecioUnitario));
    }
}