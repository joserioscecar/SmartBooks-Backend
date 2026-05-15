using SmartBooks.Domain.Entities;

namespace SmartBooks.Domain.Interfaces;

public interface IIngresoRepository
{
    Task AddAsync(Ingreso ingreso);
    Task<Ingreso?> GetByIdAsync(int id);
    IQueryable<Ingreso> Queryable();
    Task SaveChangesAsync();

    Task<int[]> GetLotesAsync();
}
