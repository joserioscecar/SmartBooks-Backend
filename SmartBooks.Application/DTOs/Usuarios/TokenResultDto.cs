namespace SmartBooks.Application.DTOs.Usuarios
{
    public class TokenResultDto
    {
        public string Token { get; set; } = default!;
        public UsuarioDto Usuario { get; set; } = new UsuarioDto();
    }
}
