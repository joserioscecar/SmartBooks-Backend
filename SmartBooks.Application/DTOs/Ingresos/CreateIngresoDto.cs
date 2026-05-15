namespace SmartBooks.Application.DTOs.Ingresos;

public class CreateIngresoDto
{
    public int LibroId { get; set; }
    public int Unidades { get; set; }

    public int Lote { get; set; }

    public decimal ValorCompra { get; set; }
    public decimal ValorVentaPublico { get; set; }
}


