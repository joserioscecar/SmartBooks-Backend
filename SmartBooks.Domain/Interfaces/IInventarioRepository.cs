using SmartBooks.Domain.Entities;

namespace SmartBooks.Domain.Interfaces;

public interface IInventarioRepository
{
    Task AddAsync(Inventario inventario);
    Task<Inventario?> GetLoteWithLibroAsync(int loteId);
    Task<int> SumIngresosByLoteAsync(int libroId,string lote);
    Task<int> SumVentasByLoteAsync(int libroId, string lote);
    Task<Inventario?> GetByCodigoExactoAsync(string codigo);
    Task<Inventario?> GetByCodigoAndLibroAsync(string codigoBase, int libroId);
    Task<IEnumerable<Inventario>> GetLotesByLoteAsync(string? lote);
    IQueryable<Inventario> Queryable();

    Task<Inventario?> FindByIdAsync(int id);
    Task SaveChangesAsync();

}
