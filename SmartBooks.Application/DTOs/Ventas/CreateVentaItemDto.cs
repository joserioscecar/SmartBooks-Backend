namespace SmartBooks.Application.DTOs.Ventas
{
    public class CreateVentaItemDto
    {
        public int LibroId { get; set; }
        public string Lote { get; set; } = string.Empty;
        public int Cantidad { get; set; }
    }
}
