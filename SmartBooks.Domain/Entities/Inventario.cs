namespace SmartBooks.Domain.Entities;

public class Inventario
{
    public int Id { get; set; }

    public string Lote { get; set; } = string.Empty;

    public int LibroId { get; set; }
    public Libro Libro { get; set; } = null!;

    public int StockDisponible { get; set; }
    public DateTime FechaIngreso { get; set; }
}
 