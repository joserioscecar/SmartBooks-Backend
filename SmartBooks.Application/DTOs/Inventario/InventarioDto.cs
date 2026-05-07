namespace SmartBooks.Application.DTOs.Inventario;

public class InventarioDto
{
    public string NivelLibro { get; set; } = default!;
    public string NombreLibro { get; set; } = default!;
    public string EdicionLibro { get; set; } = default!;
    public string TipoLibro { get; set; } = default!;
    public int CantidadIngresada { get; set; }
    public int CantidadVendida { get; set; }
    public int StockDisponible { get; set; }
    public string Lote { get; set; } = default!;
}
