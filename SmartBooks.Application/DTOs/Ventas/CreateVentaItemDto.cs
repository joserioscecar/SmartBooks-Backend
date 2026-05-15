namespace SmartBooks.Application.DTOs.Ventas
{
    public class CreateVentaItemDto
    {
        public int LibroId { get; set; }
        public int Lote { get; set; } 
        public int Cantidad { get; set; }
    }
}
