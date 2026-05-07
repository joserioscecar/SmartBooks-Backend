using Microsoft.EntityFrameworkCore;
using SmartBooks.Domain.Entities;
using SmartBooks.Domain.Enums;
using SmartBooks.Domain.Interfaces;
using SmartBooks.Infrastructure.Persistence;

namespace SmartBooks.Infrastructure.Repositories;

public class LibroRepository : ILibroRepository
{
    private readonly SmartBookDbContext _db;
    public LibroRepository(SmartBookDbContext db) => _db = db;

    public async Task AddAsync(Libro libro)
    {
        await _db.Libros.AddAsync(libro);
    }

    public async Task<Libro?> GetByIdAsync(int id)
    {
        return await _db.Libros.FindAsync(id);
    }

    public async Task<int> GetCountAsync()
    {
        return await _db.Libros.CountAsync();
    }

    public IQueryable<Libro> Queryable()
    {
        return _db.Libros.AsQueryable();
    }

    public async Task<bool> AnyDuplicateAsync(string nombre, string nivel, TipoLibro tipo, string edicion, int? exceptId = null)
    {
        var q = _db.Libros.AsQueryable();
        if (exceptId.HasValue)
            q = q.Where(l => l.Id != exceptId.Value);

        return await q.AnyAsync(l => l.Nombre == nombre && l.Nivel == nivel && l.Tipo == tipo && l.Edicion == edicion);
    }

    public async Task<bool> FindDuplicateAsync(string nombre, string nivel, TipoLibro tipo, string edicion, int? exceptId = null)
    {
        var q = _db.Libros.AsQueryable();
        if (exceptId.HasValue)
            q = q.Where(l => l.Id != exceptId.Value);

        return await q.AnyAsync(l => l.Nombre == nombre && l.Nivel == nivel && l.Tipo == tipo && l.Edicion == edicion);
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}
