using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBooks.Application.DTOs.Usuarios
{
    public class RegisterUsuarioDto
    {
        public string Identificacion { get; set; } = default!;
        public string Nombres { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public int Rol { get; set; }
    }
}


