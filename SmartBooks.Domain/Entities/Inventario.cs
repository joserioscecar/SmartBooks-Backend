namespace SmartBooks.Domain.Entities;

public class Inventario
{
    public int Id { get; set; }

    public int Lote { get; set; }

    public int LibroId { get; set; }
    public Libro Libro { get; set; } = null!;

    public int StockDisponible { get; set; }
    public DateTime FechaIngreso { get; set; }
}
 