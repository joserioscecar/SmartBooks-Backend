using Microsoft.EntityFrameworkCore;
using SmartBooks.Domain.Entities;
using SmartBooks.Domain.Interfaces;
using SmartBooks.Infrastructure.Persistence;

namespace SmartBooks.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly SmartBookDbContext _db;
    public UsuarioRepository(SmartBookDbContext db) => _db = db;

    public async Task AddAsync(Usuario usuario)
    {
        await _db.Usuarios.AddAsync(usuario);
    }

    public async Task<Usuario?> GetByIdAsync(int id)
    {
        return await _db.Usuarios.FindAsync(id);
    }

    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        return await _db.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<Usuario?> GetByEmailVerificationTokenAsync(string token)
    {
        return await _db.Usuarios.FirstOrDefaultAsync(u => u.EmailVerificationToken == token);
    }

    public async Task<Usuario?> GetByResetTokenAsync(string token)
    {
        return await _db.Usuarios.FirstOrDefaultAsync(u => u.ResetCodePassword == token);
    }

    public IQueryable<Usuario> Queryable() => _db.Usuarios.AsQueryable();

    public async Task<IEnumerable<Usuario>> SearchAsync(string? nombres, int? rol)
    {
        var q = _db.Usuarios.AsQueryable();
        if (!string.IsNullOrWhiteSpace(nombres))
            q = q.Where(u => u.Nombres.Contains(nombres));

        if (rol.HasValue)
            q = q.Where(u => (int)u.Rol == rol.Value);

        return await q.ToListAsync();
    }

    public async Task<int> SaveChangesAsync()
    {
       return await _db.SaveChangesAsync();
    }
}
