using AutoMapper;
using SmartBooks.Application.DTOs.Usuarios;
using SmartBooks.Domain.Entities;

namespace SmartBooks.Application.Mapping;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<Usuario, UsuarioDto>()
            .ForMember(dest => dest.Rol, opt => opt.MapFrom(src => src.Rol.ToString()));

        CreateMap<Usuario, UsuarioListDto>()
            .ForMember(dest => dest.Rol, opt => opt.MapFrom(src => src.Rol.ToString()));

        CreateMap<Usuario, UserProfileDto>()
            .ForMember(dest => dest.Rol, opt => opt.MapFrom(src => src.Rol.ToString()));
    }
}