using SmartBooks.Domain.Enums;

namespace SmartBooks.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string Identificacion { get; set; } = default!;
    public string Nombres { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public RolUsuario Rol { get; set; }
    public bool Activo { get; set; } = true;

    public string? EmailVerificationToken { get; set; }
    public DateTime? EmailTokenExpiresAt { get; set; }

    public string? ResetCodePassword { get; set; }
    public DateTime? ResetCodePasswordExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}
