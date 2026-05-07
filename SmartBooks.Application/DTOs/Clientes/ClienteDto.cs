namespace SmartBooks.Application.DTOs.Clientes;

public class ClienteDto
{
    public int Id { get; set; }
    public string Identificacion { get; set; } = default!;
    public string Nombres { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Celular { get; set; } = default!;
    public DateOnly FechaNacimiento { get; set; }
}
