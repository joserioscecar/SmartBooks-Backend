using AutoMapper;
using SmartBooks.Application.DTOs.Ingresos;
using SmartBooks.Application.Interfaces;
using SmartBooks.Domain.Entities;
using SmartBooks.Domain.Interfaces;

namespace SmartBooks.Application.Services;

public class IngresoService : IIngresoService
{
    private readonly IIngresoRepository _repo;
    private readonly ILibroRepository _libroRepo;
    private readonly IMapper _mapper;

    public IngresoService(IIngresoRepository repo, ILibroRepository libroRepo, IMapper mapper)
    {
        _repo = repo;
        _libroRepo = libroRepo;
        _mapper = mapper;
    }

    public async Task<IngresoResultDto> RegistrarAsync(CreateIngresoDto dto)
    {
        if (dto.ValorCompra <= 0 || dto.ValorVentaPublico <= 0)
            throw new ArgumentException("Los valores deben ser mayores que cero.");

        var libro = await _libroRepo.GetByIdAsync(dto.LibroId);
        if (libro is null)
            throw new KeyNotFoundException("Libro no encontrado.");


        var lote = new Inventario
        {
            Lote = dto.Lote,
            LibroId = dto.LibroId,
            StockDisponible = dto.Unidades,
            FechaIngreso = DateTime.UtcNow
        };

        var ingreso = new Ingreso
        {
            Fecha = DateTime.UtcNow,
            Lote = dto.Lote,
            LibroId = dto.LibroId,
            Unidades = dto.Unidades,
            ValorCompra = dto.ValorCompra,
            ValorVentaPublico = dto.ValorVentaPublico
        };

        await _repo.AddAsync(ingreso);
        await _repo.SaveChangesAsync();

        return new IngresoResultDto
        {
            Id = ingreso.Id,
            Fecha = ingreso.Fecha,
            Lote = lote.Lote,
            Unidades = ingreso.Unidades,
            ValorCompra = ingreso.ValorCompra,
            ValorVentaPublico = ingreso.ValorVentaPublico
        };
    }

    public Task<IngresoDetalleDto?> GetByIdAsync(int id)
    {
        var ingreso = _repo.Queryable().Where(i => i.Id == id).Select(i => new IngresoDetalleDto
        {
            Id = i.Id,
            Lote = i.Lote,
            Fecha = i.Fecha,
            LibroNombre = i.Libro.Nombre,
            Nivel = i.Libro.Nivel,
            Tipo = i.Libro.Tipo.ToString(),
            Unidades = i.Unidades,
            ValorCompra = i.ValorCompra,
            ValorVentaPublico = i.ValorVentaPublico
        }).FirstOrDefault();

        return Task.FromResult(ingreso);
    }

    public Task<IEnumerable<IngresoResultDto>> BuscarAsync(IngresoFilterDto dto)
    {
        var query = _repo.Queryable();
        if (dto.libroId.HasValue)
            query = query.Where(i => i.LibroId == dto.libroId);

        if (dto.Desde.HasValue)
            query = query.Where(i => i.Fecha >= dto.Desde.Value);

        if (dto.Hasta.HasValue)
            query = query.Where(i => i.Fecha <= dto.Hasta.Value);

        if (dto.Lote.HasValue)
            query = query.Where(i => i.Lote==dto.Lote.Value);


        var resultados = query.OrderByDescending(i => i.Fecha)
            .Select(i => new IngresoResultDto
            {
                Id = i.Id,
                Fecha = i.Fecha,
                Lote = i.Lote,
                Unidades = i.Unidades,
                ValorCompra = i.ValorCompra,
                ValorVentaPublico = i.ValorVentaPublico
            })
            .ToList();

        return Task.FromResult((IEnumerable<IngresoResultDto>)resultados);
    }

    public Task<int[]> GetLotesAsync()
    {
        return _repo.GetLotesAsync();
    }
}
