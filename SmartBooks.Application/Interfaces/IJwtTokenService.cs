namespace SmartBooks.Application.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(int usuarioId, string nombre, string rol);
    }
}
