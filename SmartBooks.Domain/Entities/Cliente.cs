namespace SmartBooks.Domain.Entities;

public class Cliente
{
    public int Id { get; set; }
    public string Identificacion { get; set; } = default!;
    public string Nombres { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Celular { get; set; } = default!;
    public DateOnly FechaNacimiento { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime? FechaActualizacion { get; set; }
}

