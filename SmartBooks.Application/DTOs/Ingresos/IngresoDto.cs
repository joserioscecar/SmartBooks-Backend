
namespace SmartBooks.Application.DTOs.Ingresos;

public class IngresoDto
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public string CodigoLote { get; set; } = default!;
    public int LibroId { get; set; }
    public int Unidades { get; set; }
    public decimal ValorCompra { get; set; }
    public decimal ValorVentaPublico { get; set; }
}
