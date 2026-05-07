namespace SmartBooks.Application.DTOs.Usuarios
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Nombres { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Rol { get; set; } = default!;
    }
}
