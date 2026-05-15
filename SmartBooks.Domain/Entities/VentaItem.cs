namespace SmartBooks.Domain.Entities;

public class VentaItem
{
    public int Id { get; set; }

    public int VentaId { get; set; }
    public Venta Venta { get; set; } = default!;

    public int LibroId { get; set; }
    public Libro Libro { get; set; } = default!;

    public int Lote { get; set; } = default!;

    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }

    public decimal Subtotal => Cantidad * PrecioUnitario;
}
