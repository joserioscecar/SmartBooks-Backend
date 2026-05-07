namespace SmartBooks.Domain.Interfaces;

public interface IVentaRepository
{
    Task AddAsync(Venta venta);

    Task<Venta?> GetByIdWithDetailsAsync(int id);
    IQueryable<Venta> Queryable();
    IQueryable<Venta> QueryableWithDetails();

    Task<int> CountAsync();
    Task SaveChangesAsync();
    Task<int> CountMonthAsync(DateOnly date);
    Task<decimal> TotalMonthAsync(DateOnly date);
    Task<IEnumerable<Venta>> GetByDateAsync(DateOnly date);

}
