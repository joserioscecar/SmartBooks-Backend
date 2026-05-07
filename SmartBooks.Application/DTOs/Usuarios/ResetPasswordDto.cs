namespace SmartBooks.Application.DTOs.Usuarios;

public class ResetPasswordDto
{
    public string Codigo { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
}
