using SmartBooks.Domain.Entities;
using SmartBooks.Domain.Enums;

namespace SmartBooks.Domain.Interfaces;

public interface ILibroRepository
{
    Task AddAsync(Libro libro);
    Task<Libro?> GetByIdAsync(int id);
    Task<bool> AnyDuplicateAsync(string nombre, string nivel, TipoLibro tipo, string edicion, int? exceptId = null);
    IQueryable<Libro> Queryable();
    Task SaveChangesAsync();

    Task<int> GetCountAsync();

    Task<bool> FindDuplicateAsync(string nombre, string nivel, TipoLibro tipo, string edicion, int? exceptId = null);
}
