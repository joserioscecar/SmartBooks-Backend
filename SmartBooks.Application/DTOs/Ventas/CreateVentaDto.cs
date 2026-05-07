using SmartBooks.Application.DTOs.Ventas;

public class CreateVentaDto
{
    public string IdentificacionCliente { get; set; } = string.Empty;

    public string NumeroComprobante { get; set; } = string.Empty; 

    public string Observaciones { get; set; } = string.Empty;

    public List<CreateVentaItemDto> Items { get; set; } = new();
}
