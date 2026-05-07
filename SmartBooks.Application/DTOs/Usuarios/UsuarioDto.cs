namespace SmartBooks.Application.DTOs.Usuarios
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Identificacion { get; set; } = default!;
        public string Nombres { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Rol { get; set; } = default!;
        public bool Activo { get; set; }
    }
}
