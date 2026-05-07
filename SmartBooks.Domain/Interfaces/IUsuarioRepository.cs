using SmartBooks.Domain.Entities;

namespace SmartBooks.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task AddAsync(Usuario usuario);
        Task<Usuario?> GetByIdAsync(int id);
        Task<Usuario?> GetByEmailAsync(string email);
        Task<Usuario?> GetByEmailVerificationTokenAsync(string token);
        Task<Usuario?> GetByResetTokenAsync(string token);
        IQueryable<Usuario> Queryable();
        Task<IEnumerable<Usuario>> SearchAsync(string? nombres, int? rol);
        Task<int> SaveChangesAsync();
    }
}
