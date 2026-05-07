using AutoMapper;
using SmartBooks.Application.DTOs.Ventas;

namespace SmartBooks.Application.Mapping;

public class VentaProfile : Profile
{
    public VentaProfile()
    {
        CreateMap<Venta, VentaResultDto>();
        CreateMap<Venta, VentaDetalleDto>();
        CreateMap<Venta, VentaListDto>();
    }
}