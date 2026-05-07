using SmartBooks.Domain.Entities;

namespace SmartBooks.Domain.Interfaces;

public interface IClienteRepository
{
    Task AddAsync(Cliente cliente);
    Task<Cliente?> GetByIdAsync(int id);
    Task<Cliente?> GetByIdentificacionAsync(string identificacion);
    Task<bool> AnyByIdentificacionAsync(string identificacion);
    Task<bool> AnyByEmailAsync(string email);
    Task<bool> AnyByCelularAsync(string celular);
    Task<IEnumerable<Cliente>> SearchByNombresAsync(string? nombres);
    Task<int> GetCountAsync();
    Task SaveChangesAsync();
}
