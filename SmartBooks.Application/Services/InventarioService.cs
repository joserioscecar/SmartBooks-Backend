using SmartBooks.Application.DTOs.Inventario;
using SmartBooks.Application.DTOs.Libros;
using SmartBooks.Application.Interfaces;
using SmartBooks.Domain.Interfaces;

namespace SmartBooks.Application.Services;

public class InventarioService : IInventarioService
{
    private readonly IInventarioRepository _repo;

    public InventarioService(IInventarioRepository repo)
    {
        _repo = repo;
    }




    public async Task<IEnumerable<InventarioDto>> GetLotesByLoteAsync(string? lote)
    {
        var lotes = await _repo.GetLotesByLoteAsync(lote);

        var result = new List<InventarioDto>();

        foreach (var i in lotes)
        {
            var cantidadIngresada = await _repo.SumIngresosByLoteAsync(i.Libro.Id, i.Lote);
            var cantidadVendida = await _repo.SumVentasByLoteAsync(i.Libro.Id, i.Lote);


            result.Add(new InventarioDto
            {
                NombreLibro = i.Libro.Nombre,
                NivelLibro = i.Libro.Nivel,
                EdicionLibro = i.Libro.Edicion,
                TipoLibro = i.Libro.Tipo.ToString(),
                CantidadIngresada = cantidadIngresada,
                CantidadVendida = cantidadVendida,
                StockDisponible = i.StockDisponible,
                Lote = i.Lote
            });
        }

        return result;
    }


    public async Task<IEnumerable<StockDto>> GetStockGroupedAsync()
    {

        var stock = _repo.Queryable();

        var result = stock
            .SelectMany(l => l.Libro.Inventarios)
            .GroupBy(l => new { l.Libro.Nombre, l.Libro.Nivel, l.Libro.Tipo })
            .Select(g => new StockDto
            {
                Nombre = g.Key.Nombre,
                Nivel = g.Key.Nivel,
                Tipo = g.Key.Tipo.ToString(),
                StockTotal = g.Sum(l => l.StockDisponible)
            })
            .ToList();

        return result;

    }
}
