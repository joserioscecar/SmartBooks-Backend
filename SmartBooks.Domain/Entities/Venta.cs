using SmartBooks.Domain.Entities;

public class Venta
{
    public int Id { get; set; }

    public string NumeroRecibo { get; set; } = default!;

    public string NumeroComprobante { get; set; } = default!;

    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; } = default!;

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = default!;

    public string? Observaciones { get; set; }

    public decimal Total { get; set; }

    public List<VentaItem> Items { get; set; } = new();
}
