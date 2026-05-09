namespace SmartBooks.Application.DTOs.Libros;

public class UpdateLibroDto
{
    public string Nombre { get; set; } = default!;
    public string Nivel { get; set; } = default!;
    public int Tipo { get; set; }
    public string Edicion { get; set; } = default!;
}
