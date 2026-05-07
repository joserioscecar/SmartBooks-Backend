namespace SmartBooks.Application.DTOs.Clientes;
public class UpdateClienteDto
{
    public string Nombres { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Celular { get; set; } = default!;
    public DateOnly FechaNacimiento { get; set; }
}
