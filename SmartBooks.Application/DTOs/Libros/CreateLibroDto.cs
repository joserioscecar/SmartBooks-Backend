using SmartBooks.Domain.Enums;

namespace SmartBooks.Application.DTOs.Libros;

public class CreateLibroDto
{
    public string Nombre { get; set; } = default!;
    public string Nivel { get; set; } = default!;
    public TipoLibro Tipo { get; set; }
    public string Edicion { get; set; } = default!;

    public int Unidades { get; set; }
    public int Lote { get; set; } = default!;
    public decimal ValorCompra { get; set; } = default!;
    public decimal ValorVentaPublico{ get; set; } = default!;

}


