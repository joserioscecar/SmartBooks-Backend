using Microsoft.EntityFrameworkCore;
using SmartBooks.Domain.Entities;
using SmartBooks.Domain.Interfaces;
using SmartBooks.Infrastructure.Persistence;

namespace SmartBooks.Infrastructure.Repositories;

public class IngresoRepository : IIngresoRepository
{
    private readonly SmartBookDbContext _db;
    public IngresoRepository(SmartBookDbContext db) => _db = db;

    public async Task AddAsync(Ingreso ingreso)
    {
        await _db.Ingresos.AddAsync(ingreso);
    }

    public async Task<Ingreso?> GetByIdAsync(int id)
    {
        return await _db.Ingresos.FindAsync(id);
    }

    public IQueryable<Ingreso> Queryable() => _db.Ingresos.AsQueryable();

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }

    public async Task<int[]> GetLotesAsync()
    {

        return await _db.Inventarios.AsNoTracking()
            .Select(l => l.Lote).Distinct()
            .ToArrayAsync();
    }
}
