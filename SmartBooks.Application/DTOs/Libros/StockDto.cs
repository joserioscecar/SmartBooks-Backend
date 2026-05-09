namespace SmartBooks.Application.DTOs.Libros;

public class StockDto
{
    public int Id { get; set; } = default!;
    public string Nombre { get; set; } = default!;
    public string Nivel { get; set; } = default!;
    public string Tipo { get; set; } = default!;
    public string Lote { get; set; } = default!;
    public string Edicion { get; set; } = default!;
    public int StockTotal { get; set; }

    public decimal ValorCompa { get; set; }

    public decimal ValorVentaPulico { get; set; }
}
