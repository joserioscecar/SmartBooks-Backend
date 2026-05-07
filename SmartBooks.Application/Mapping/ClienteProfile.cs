using AutoMapper;
using SmartBooks.Application.DTOs.Clientes;
using SmartBooks.Application.Extensions;
using SmartBooks.Domain.Entities;

namespace SmartBooks.Application.Mapping;

public class ClienteProfile : Profile
{
    public ClienteProfile()
    {
        CreateMap<CreateClienteDto, Cliente>()
            .ForMember(dest => dest.Identificacion,
                opt => opt.MapFrom(src => src.Identificacion))
            .ForMember(dest => dest.Nombres,
                opt => opt.MapFrom(src => StringSanitizer.WithoutDiacritics(src.Nombres.Trim())))
            .ForMember(dest => dest.FechaNacimiento,
                opt => opt.MapFrom(src => src.FechaNacimiento))
            .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => src.Email.Trim().ToLowerInvariant()))
            .ForMember(dest => dest.Celular,
                opt => opt.MapFrom(src => src.Celular));

        CreateMap<Cliente, ClienteDto>();
    }
}