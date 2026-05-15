using SmartBooks.Domain.Entities;

namespace SmartBooks.Domain.Interfaces;

public interface IInventarioRepository
{
    Task AddAsync(Inventario inventario);
    Task<Inventario?> GetLoteWithLibroAsync(int loteId);
    Task<int> SumIngresosByLoteAsync(int libroId,int lote);
    Task<int> SumVentasByLoteAsync(int libroId, int lote);
    Task<Inventario?> GetByCodigoExactoAsync(int lote);
    Task<Inventario?> GetByCodigoAndLibroAsync(int lote, int libroId);
    Task<IEnumerable<Inventario>> GetLotesByLoteAsync(int? lote);
    IQueryable<Inventario> Queryable();

    Task<Inventario?> FindByIdAsync(int id);
    Task SaveChangesAsync();

}
