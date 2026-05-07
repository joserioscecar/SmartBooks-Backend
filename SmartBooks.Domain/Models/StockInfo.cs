using SmartBooks.Domain.Enums;

namespace SmartBooks.Domain.Models;

public class StockInfo
{
    public string Libro { get; set; } = default!;
    public string Nivel { get; set; } = default!;
    public TipoLibro Tipo { get; set; }
    public int StockTotal { get; set; }
}
