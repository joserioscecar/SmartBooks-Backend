using AutoMapper;
using SmartBooks.Application.DTOs.Clientes;
using SmartBooks.Application.DTOs.Ingresos;
using SmartBooks.Application.DTOs.Libros;
using SmartBooks.Application.DTOs.Lotes;
using SmartBooks.Application.DTOs.Usuarios;
using SmartBooks.Application.DTOs.Ventas;
using SmartBooks.Domain.Entities;
using SmartBooks.Domain.Enums;

namespace SmartBooks.Application.Mapping
{
    public class LibroProfile : Profile
    {
        public LibroProfile()
        {
            CreateMap<Libro, LibroDto>().ReverseMap();

            CreateMap<CreateLibroDto, Libro>()
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre != null ? src.Nombre.Trim() : src.Nombre))
                .ForMember(dest => dest.Nivel, opt => opt.MapFrom(src => src.Nivel != null ? src.Nivel.Trim() : src.Nivel))
                .ForMember(dest => dest.Edicion, opt => opt.MapFrom(src => src.Edicion != null ? src.Edicion.Trim() : src.Edicion))
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => (TipoLibro)src.Tipo));

            CreateMap<UpdateLibroDto, Libro>()
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre != null ? src.Nombre.Trim() : src.Nombre))
                .ForMember(dest => dest.Nivel, opt => opt.MapFrom(src => src.Nivel != null ? src.Nivel.Trim() : src.Nivel))
                .ForMember(dest => dest.Edicion, opt => opt.MapFrom(src => src.Edicion != null ? src.Edicion.Trim() : src.Edicion))
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => (TipoLibro)src.Tipo));
        }
    }
}

