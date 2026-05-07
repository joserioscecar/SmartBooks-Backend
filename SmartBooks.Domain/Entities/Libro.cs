using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SmartBooks.Domain.Enums;

namespace SmartBooks.Domain.Entities;

public class Libro
{
    public int Id { get; set; }
    public string Nombre { get; set; } = default!;
    public string Nivel { get; set; } = default!;
    public TipoLibro Tipo { get; set; }  
    public string Edicion { get; set; } = default!;
    public List<Inventario> Inventarios { get; set; } = new();
    public List<Ingreso> Ingresos { get; set; } = new();
}
