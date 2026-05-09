using AutoMapper;
using SmartBooks.Application.DTOs.Libros;
using SmartBooks.Application.Interfaces;
using SmartBooks.Domain.Entities;
using SmartBooks.Domain.Enums;
using SmartBooks.Domain.Exceptions;
using SmartBooks.Domain.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace SmartBooks.Application.Services;

public class LibroService : ILibroService
{
    private readonly ILibroRepository _libroRepository;
    private readonly IIngresoRepository _ingresoReposotory;
    private readonly IInventarioRepository _inventarioRepository;
    private readonly ILoteRepository _loteRepository;
    private readonly IMapper _mapper;

    public LibroService(ILibroRepository repo, IIngresoRepository libroRepository, IInventarioRepository ingresoReposotory, ILoteRepository loteRepository, IMapper mapper)
    {
        _libroRepository = repo;
        _ingresoReposotory = libroRepository;
        _inventarioRepository = ingresoReposotory;
        _loteRepository = loteRepository;

        _mapper = mapper;
    }


    public async Task<int> CreateAsync(CreateLibroDto dto)
    {

        Ingreso ingresoRegistrado;

        if (!await _loteRepository.ExisteLoteAsync(dto.Lote))
        {
            throw new RuleBusinessException($"El lote {dto.Lote} no existe");
        }

        var libroId = _libroRepository.Queryable()
            .Where(l => l.Nombre == dto.Nombre
                     && l.Nivel == dto.Nivel
                     && l.Tipo == dto.Tipo
                     && l.Edicion == dto.Edicion)
            .Select(l => l.Id)
            .FirstOrDefault();

        if (libroId != 0)
        {
            var existeEnLote = _inventarioRepository.Queryable().Any(i => i.Lote == dto.Lote && i.LibroId == libroId);

            if (existeEnLote)
            {
                throw new RuleBusinessException($"El libro {dto.Nombre} ya existe en el lote {dto.Lote}");
            }
            else
            {
 
                ingresoRegistrado = _mapper.Map<Ingreso>(dto);
                ingresoRegistrado.LibroId = libroId;
                await _ingresoReposotory.AddAsync(ingresoRegistrado);
                await _ingresoReposotory.SaveChangesAsync();

                return libroId; 
            }
        }

        var entity = _mapper.Map<Libro>(dto);
        await _libroRepository.AddAsync(entity);
        await _libroRepository.SaveChangesAsync();

        ingresoRegistrado = _mapper.Map<Ingreso>(dto); 
        var inventarios = _mapper.Map<Inventario>(dto);

        ingresoRegistrado.LibroId = entity.Id;
        inventarios.LibroId = entity.Id;

        await _inventarioRepository.AddAsync(inventarios);
        await _inventarioRepository.SaveChangesAsync();

        await _ingresoReposotory.AddAsync(ingresoRegistrado);
        await _ingresoReposotory.SaveChangesAsync();

        return entity.Id; 
    }



    public async Task<LibroDto?> GetByIdAsync(int id)
    {
        var l = await _libroRepository.GetByIdAsync(id);
        return l is null ? null : _mapper.Map<LibroDto>(l);
    }

    public async Task<IEnumerable<LibroDto>> SearchAsync(BookFilterDto dto)
    {
        var q = _libroRepository.Queryable();

        if (!string.IsNullOrWhiteSpace(dto.Nombre))
            q = q.Where(l => l.Nombre.Contains(dto.Nombre));

        if (!string.IsNullOrWhiteSpace(dto.Nivel))
            q = q.Where(l => l.Nivel.Contains(dto.Nivel));

        if (dto.Tipo.HasValue)
            q = q.Where(l => (int)l.Tipo == dto.Tipo.Value);

        if (!string.IsNullOrWhiteSpace(dto.Edicion))
            q = q.Where(l => l.Edicion.Contains(dto.Edicion));

        var items = await Task.FromResult(q.Select(l => l).ToList());
        return _mapper.Map<IEnumerable<LibroDto>>(items);
    }

    public async Task UpdateAsync(int id, UpdateLibroDto dto)
    {
        var libro = await _libroRepository.GetByIdAsync(id);
        if (libro is null)
            throw new KeyNotFoundException("Libro no encontrado.");

        if (await _libroRepository.AnyDuplicateAsync(dto.Nombre, dto.Nivel, (TipoLibro)dto.Tipo, dto.Edicion, id))
            throw new RuleBusinessException("Ya existe otro libro con los mismos datos.");

        _mapper.Map(dto, libro);

        await _libroRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<StockDto>> GetStockGroupedAsync(BookFilterDto dto)
    {

        var stock = _libroRepository.Queryable();


        if (!string.IsNullOrWhiteSpace(dto.Nombre))
            stock = stock.Where(l => l.Nombre.Contains(dto.Nombre));

        if (!string.IsNullOrWhiteSpace(dto.Nivel))
            stock = stock.Where(l => l.Nivel.Contains(dto.Nivel));  
        if (dto.Tipo.HasValue)
            stock = stock.Where(l => (int)l.Tipo == dto.Tipo.Value);

        if (!string.IsNullOrWhiteSpace(dto.Edicion))
            stock = stock.Where(l => l.Edicion.Contains(dto.Edicion));

        var result = stock
            .SelectMany(l => l.Inventarios)

            .GroupBy(l => new { l.Libro.Nombre, l.Libro.Nivel, l.Libro.Tipo, l.Lote, l.LibroId })
            .Select(g => new StockDto
            {
                Id = g.Key.LibroId,
                Nombre = g.Key.Nombre,
                Nivel = g.Key.Nivel,
                Tipo = g.Key.Tipo.ToString(),
                Lote = g.Key.Lote,
                Edicion = g.First().Libro.Edicion,
                StockTotal = g.Sum(l => l.StockDisponible)

            })
            .ToList();

        return result;

    }
}
