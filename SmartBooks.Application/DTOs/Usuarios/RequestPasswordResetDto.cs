using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBooks.Application.DTOs.Usuarios;

public class RequestPasswordResetDto
{
    public string Email { get; set; } = default!;
}

