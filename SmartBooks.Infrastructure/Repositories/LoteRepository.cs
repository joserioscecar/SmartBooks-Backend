using Microsoft.EntityFrameworkCore;
using SmartBooks.Domain.Entities;
using SmartBooks.Domain.Interfaces;
using SmartBooks.Infrastructure.Persistence;

namespace SmartBooks.Infrastructure.Repositories;

public class LoteRepository : ILoteRepository
{

    private readonly SmartBookDbContext _db;
    public LoteRepository(SmartBookDbContext db) => _db = db;

    public async Task AddAsync(Lote lote)
    {


        await _db.Lotes.ExecuteUpdateAsync(setters => setters.SetProperty(l => l.Actual, false));


        await _db.Lotes.AddAsync(lote);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<Lote>> GetLotesAsync()
    {
        return _db.Lotes.AsEnumerable().OrderByDescending(l=>l.Codigo);
    }

    public async Task<bool> ExisteLoteAsync(int codigo)
    {
        return await _db.Lotes
            .AnyAsync(l => l.Codigo == codigo);
    }
}
