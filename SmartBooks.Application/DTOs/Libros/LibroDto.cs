using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBooks.Application.DTOs.Libros;

public class LibroDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = default!;
    public string Nivel { get; set; } = default!;
    public int Tipo { get; set; } = default!;
    public string Edicion { get; set; }
}
