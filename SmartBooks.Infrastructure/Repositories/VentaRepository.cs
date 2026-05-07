using Microsoft.EntityFrameworkCore;
using SmartBooks.Domain.Interfaces;
using SmartBooks.Infrastructure.Persistence;

namespace SmartBooks.Infrastructure.Repositories;

public class VentaRepository : IVentaRepository
{
    private readonly SmartBookDbContext _db;
    public VentaRepository(SmartBookDbContext db) => _db = db;

    public async Task AddAsync(Venta venta)
    {
        await _db.Ventas.AddAsync(venta);
    }

    public async Task<Venta?> GetByIdWithDetailsAsync(int id)
    {
        return await _db.Ventas
            .Include(v => v.Cliente)
            .Include(v => v.Usuario)
            .Include(v => v.Items).ThenInclude(i => i.Libro)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public IQueryable<Venta> QueryableWithDetails()
    {
        return _db.Ventas
           .Include(v => v.Cliente)
            .Include(v => v.Items)
           .ThenInclude(i => i.Libro)
             .Include(v => v.Items)
            .AsQueryable();
    }

    public IQueryable<Venta> Queryable() => _db.Ventas.AsQueryable();

    public async Task<int> CountAsync()
    {
        return await _db.Ventas.CountAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }

    public Task<int> CountMonthAsync(DateOnly date)
    {
        return _db.Ventas
            .Where(v => v.Fecha.Year == date.Year && v.Fecha.Month == date.Month)
            .CountAsync();
    }


    public Task<decimal> TotalMonthAsync(DateOnly date)
    {
        return _db.Ventas
            .Where(v => v.Fecha.Year == date.Year && v.Fecha.Month == date.Month)
            .SumAsync(v => v.Total);
    }

    public async Task<IEnumerable<Venta>> GetByDateAsync(DateOnly date)
    {
        return await _db.Ventas.AsNoTracking()
            .Where(v => DateOnly.FromDateTime(v.Fecha) == date).ToListAsync();
    }
}
