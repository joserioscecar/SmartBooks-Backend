using Microsoft.EntityFrameworkCore;
using SmartBooks.Domain.Entities;
using SmartBooks.Domain.Interfaces;
using SmartBooks.Infrastructure.Persistence;

namespace SmartBooks.Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly SmartBookDbContext _db;
    public ClienteRepository(SmartBookDbContext db) => _db = db;

    public async Task AddAsync(Cliente cliente)
    {
        await _db.Clientes.AddAsync(cliente);
    }

    public async Task<Cliente?> GetByIdAsync(int id)
    {
        return await _db.Clientes.FindAsync(id);
    }

    public async Task<Cliente?> GetByIdentificacionAsync(string identificacion)
    {
        return await _db.Clientes.FirstOrDefaultAsync(c => c.Identificacion == identificacion);
    }

    public async Task<bool> AnyByIdentificacionAsync(string identificacion)
    {
        return await _db.Clientes.AnyAsync(c => c.Identificacion == identificacion);
    }

    public async Task<bool> AnyByEmailAsync(string email)
    {
        return await _db.Clientes.AnyAsync(c => c.Email == email);
    }

    public async Task<bool> AnyByCelularAsync(string celular)
    {
        return await _db.Clientes.AnyAsync(c => c.Celular == celular);
    }

    public async Task<IEnumerable<Cliente>> SearchByNombresAsync(string? nombres)
    {
        var q = _db.Clientes.AsQueryable();
        if (!string.IsNullOrWhiteSpace(nombres))
            q = q.Where(c => c.Nombres.Contains(nombres));

        return await q.ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }

    public async Task<int> GetCountAsync()
    {
       return await _db.Clientes.CountAsync();
    }
}
