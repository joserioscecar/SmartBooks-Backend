namespace SmartBooks.Application.DTOs.Ingresos;

public class IngresoDetalleDto
{
    public int Id { get; set; }
    public int Lote { get; set; }
    public DateTime Fecha { get; set; }
    public string LibroNombre { get; set; } = default!;
    public string Nivel { get; set; } = default!;
    public string Tipo { get; set; } = default!;
    public int Unidades { get; set; }
    public decimal ValorCompra { get; set; }
    public decimal ValorVentaPublico { get; set; }
}
