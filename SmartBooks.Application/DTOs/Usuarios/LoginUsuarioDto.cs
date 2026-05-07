using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBooks.Application.DTOs.Usuarios;

public class LoginUsuarioDto
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}

