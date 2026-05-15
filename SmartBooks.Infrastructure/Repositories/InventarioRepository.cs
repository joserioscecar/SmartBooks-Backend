using Microsoft.EntityFrameworkCore;
using SmartBooks.Domain.Entities;
using SmartBooks.Domain.Interfaces;
using SmartBooks.Infrastructure.Persistence;

namespace SmartBooks.Infrastructure.Repositories;

public class InventarioRepository : IInventarioRepository
{
    private readonly SmartBookDbContext _db;

    public InventarioRepository(SmartBookDbContext db)
    {
        _db = db;
    }


    public async Task<Inventario?> GetLoteWithLibroAsync(int loteId)
    {
        return await _db.Inventarios
            .Include(l => l.Libro)
            .FirstOrDefaultAsync(l => l.Id == loteId);
    }

    public async Task AddAsync(Inventario inventario)
    {
        await _db.Inventarios.AddAsync(inventario);
    }


    public async Task<int> SumIngresosByLoteAsync(int libroId, int lote)
    {
        return await _db.Ingresos
            .Where(i => i.Lote == lote)
            .Where(v => v.Libro.Id == libroId)
            .SumAsync(i => i.Unidades);
    }


    public async Task<int> SumVentasByLoteAsync(int libroId,int lote)
    {
        return await _db.VentaItems
            .Where(v => v.Lote == lote)
            .Where(v=>v.Libro.Id== libroId)
            .SumAsync(v => v.Cantidad);
    }


    public async Task<Inventario?> GetByCodigoExactoAsync(int lote)
    {
        return await _db.Inventarios
            .Include(l => l.Libro)
            .FirstOrDefaultAsync(l => l.Lote == lote);
    }


    public async Task<Inventario?> GetByCodigoAndLibroAsync(int lote, int libroId)
    {
        return await _db.Inventarios
            .Include(l => l.Libro)
            .Where(l => l.Lote== lote) 
            .Where(l => l.LibroId == libroId)
            .OrderBy(l => l.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Inventario>> GetLotesByLoteAsync(int? lote)
    {
        var query = _db.Inventarios
            .Include(l => l.Libro)
            .AsQueryable();

        if (lote.HasValue)
        {
            query = query.Where(l => l.Lote == lote.Value);
        }

        return await query
            .OrderBy(l => l.Lote)
            .ToListAsync();
    }

    public IQueryable<Inventario> Queryable()
    {
        return _db.Inventarios.AsQueryable();
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
         
    }

    public async Task<Inventario?> FindByIdAsync(int id)
    {
        return await _db.Inventarios.FindAsync(id);
    }

}
