using AutoMapper;
using SmartBooks.Application.DTOs.Lotes;
using SmartBooks.Domain.Entities;

namespace SmartBooks.Application.Mapping;

public class LoteProfile : Profile
{
    public LoteProfile()
    {
        CreateMap<Lote, LoteDto>();
    }
}