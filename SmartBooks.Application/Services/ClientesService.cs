using AutoMapper;
using SmartBooks.Application.DTOs.Clientes;
using SmartBooks.Application.Extensions;
using SmartBooks.Application.Interfaces;
using SmartBooks.Domain.Entities;
using SmartBooks.Domain.Exceptions;
using SmartBooks.Domain.Interfaces;

namespace SmartBooks.Application.Services;

public class ClientesService : IClientesService
{
    private readonly IClienteRepository _repo;
    private readonly IMapper _mapper;

    public ClientesService(IClienteRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ClienteDto> CreateAsync(CreateClienteDto dto)
    {
        if (await _repo.AnyByIdentificacionAsync(dto.Identificacion))
            throw new RuleBusinessException("Identificación ya registrada");

        if (await _repo.AnyByEmailAsync(dto.Email))
            throw new RuleBusinessException("Email ya registrado");

        if (await _repo.AnyByCelularAsync(dto.Celular))
            throw new RuleBusinessException("Celular ya registrado");


        var entity = _mapper.Map<Cliente>(dto); 


        await _repo.AddAsync(entity);
        await _repo.SaveChangesAsync();

        return _mapper.Map<ClienteDto>(entity);
    }

    public async Task<ClienteDto?> GetByIdentificacionAsync(string identificacion)
    {
        var c = await _repo.GetByIdentificacionAsync(identificacion);
        return c is null ? null : _mapper.Map<ClienteDto>(c);
    }

    public async Task<IEnumerable<ClienteDto>> SearchAsync(string? nombres)
    {
        var items = await _repo.SearchByNombresAsync(nombres);
        return _mapper.Map<IEnumerable<ClienteDto>>(items);
    }

    public async Task UpdateAsync(string identificacion, UpdateClienteDto dto)
    {

        dto.Nombres = dto.Nombres.Trim().RemoveAccents();

        var c = await _repo.GetByIdentificacionAsync(identificacion);
        if (c is null)
            throw new KeyNotFoundException("Cliente no encontrado");

        c.Nombres = StringSanitizer.WithoutDiacritics(dto.Nombres.Trim());
        c.Email = dto.Email.ToLowerInvariant();
        c.Celular = dto.Celular;
        c.FechaNacimiento = dto.FechaNacimiento;
        c.FechaActualizacion = DateTime.UtcNow;

        await _repo.SaveChangesAsync();
    }
}
