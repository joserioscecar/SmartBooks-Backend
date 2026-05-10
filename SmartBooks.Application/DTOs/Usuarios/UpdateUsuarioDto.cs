namespace SmartBooks.Application.DTOs.Usuarios;

public class UpdateUsuarioDto
{
    public string Nombres { get; set; } = default!;
    public string Email { get; set; } = default!;
    public int Rol { get; set; }
    public bool Activo { get; set; }
}
