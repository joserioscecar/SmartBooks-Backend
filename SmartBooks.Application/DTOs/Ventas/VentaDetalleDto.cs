namespace SmartBooks.Application.DTOs.Ventas
{
    public class VentaDetalleDto
    {
        public int Id { get; set; }
        public string NumeroRecibo { get; set; } = string.Empty;
        public string NumeroComprobante { get; set; } = string.Empty;

        public decimal Total { get; set; }

        public DateTime Fecha { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
    }
}
