using AutoMapper;
using SmartBooks.Application.DTOs.Ingresos;
using SmartBooks.Application.DTOs.Inventario;
using SmartBooks.Application.DTOs.Libros;
using SmartBooks.Domain.Entities;

namespace SmartBooks.Application.Mapping;

public class InventarioProfile : Profile
{
    public InventarioProfile()
    {

        CreateMap<CreateLibroDto, Inventario>().ForMember(dest => dest.StockDisponible, opt => opt.MapFrom(src => src.Unidades));

        CreateMap<Inventario, InventarioDto>();

    }
}