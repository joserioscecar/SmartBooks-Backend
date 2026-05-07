namespace SmartBooks.Application.DTOs.Ingresos;

public class IngresoFilterDto
{
    public DateTime? Desde { get; set; }
    public DateTime? Hasta { get; set; }
    public string? Lote { get; set; }
    public int? libroId { get; set; }
}
