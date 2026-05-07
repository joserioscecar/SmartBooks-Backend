using AutoMapper;
using SmartBooks.Application.DTOs.Ingresos;
using SmartBooks.Application.DTOs.Libros;
using SmartBooks.Domain.Entities;

namespace SmartBooks.Application.Mapping;

public class IngresoProfile : Profile
{
    public IngresoProfile()
    {
        CreateMap<Ingreso, IngresoDetalleDto>()
            .ForMember(dest => dest.CodigoLote, opt => opt.MapFrom(src => src.Lote))
            .ForMember(dest => dest.LibroNombre, opt => opt.MapFrom(src => src.Libro.Nombre))
            .ForMember(dest => dest.Nivel, opt => opt.MapFrom(src => src.Libro.Nivel))
            .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Libro.Tipo.ToString()));

        CreateMap<Ingreso, IngresoListDto>();
        CreateMap<IngresoDto, Ingreso>();
        CreateMap<IngresoRegistroDto, Ingreso>();
        CreateMap<CreateLibroDto, Ingreso>();

    }
}