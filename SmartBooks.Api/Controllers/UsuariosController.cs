using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBooks.Application.DTOs.Usuarios;
using SmartBooks.Application.Interfaces;
using SmartBooks.Application.Services;

namespace SmartBooks.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsuariosController : ControllerBase
{
    private readonly IUsuariosService _service;

    public UsuariosController(IUsuariosService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RegistrarUsuario([FromBody] RegisterUsuarioDto dto)
    {


        await _service.RegisterAsync(dto);
        return Ok(new { mensaje = "Usuario registrado. Revisa tu correo para verificar tu cuenta." });


    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> BuscarUsuarios([FromQuery] string? nombres, [FromQuery] int? rol)
    {
        var usuarios = await _service.SearchAsync(nombres, rol);
        return Ok(usuarios);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var usuario = await _service.GetByIdAsync(id);
        return usuario is null ? NotFound(new { mensaje = "Usuario no encontrado." }) : Ok(usuario);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ActualizarUsuario(int id, [FromBody] UpdateUsuarioDto dto)
    {

        await _service.UpdateAsync(id, dto);
        return Ok(new { mensaje = "Usuario actualizado correctamente." });

    }


    [HttpPatch("{id}/estado")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CambiarEstado(int id, [FromQuery] bool activo)
    {

        await _service.ChangeStateAsync(id, activo);
        var estado = activo ? "activado" : "inactivado";
        return Ok(new { mensaje = $"Usuario {estado} correctamente." });

    }


    [HttpGet("perfil")]
    [Authorize]
    public async Task<IActionResult> Perfil()
    {
        var idClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

        if (string.IsNullOrEmpty(idClaim))
            return Unauthorized(new { mensaje = "Token inválido." });

        if (!int.TryParse(idClaim, out int userId))
            return BadRequest(new { mensaje = "ID de usuario inválido." });

        var perfil = await _service.GetProfileAsync(userId);
        if (perfil is null)
            return NotFound(new { mensaje = "Usuario no encontrado." });

        return Ok(perfil);
    }

}
