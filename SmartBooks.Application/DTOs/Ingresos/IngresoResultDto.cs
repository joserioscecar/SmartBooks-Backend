namespace SmartBooks.Application.DTOs.Ingresos;

public class IngresoResultDto
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public int Lote { get; set; }
    public int Unidades { get; set; }
    public decimal ValorCompra { get; set; }
    public decimal ValorVentaPublico { get; set; }
}
