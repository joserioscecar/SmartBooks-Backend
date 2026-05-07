using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBooks.Application.DTOs.Usuarios;
using SmartBooks.Application.Interfaces;

namespace SmartBooks.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeguridadController : ControllerBase
{
    private readonly IUsuariosService _usuariosService;
    private readonly IClientContext _clientContext;

    public SeguridadController(IUsuariosService usuariosService, IClientContext clientContext)
    {
        _usuariosService = usuariosService;
        _clientContext = clientContext;
    }

    [HttpGet("verificar-correo")]
    [AllowAnonymous]
    public async Task<IActionResult> VerificarCorreo([FromQuery] string token)
    {
        try
        {
            await _usuariosService.VerifyEmailAsync(token);
            return Ok(new { mensaje = "Correo verificado correctamente." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPost("iniciar-sesion")]
    [AllowAnonymous]
    public async Task<IActionResult> IniciarSesion(LoginUsuarioDto dto)
    {
        try
        {

            if (_clientContext.IsMobile) { 
            
            
            }

                var result = await _usuariosService.LoginAsync(dto);
            return Ok(new { token = result.Token, usuario = result.Usuario });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { mensaje = ex.Message });
        }
    }

    [HttpPost("solicitar-restablecimiento")]
    [AllowAnonymous]
    public async Task<IActionResult> SolicitarRestablecimiento(RequestPasswordResetDto dto)
    {
        try
        {
            await _usuariosService.RequestPasswordResetAsync(dto);
            return Ok(new { mensaje = "Se ha enviado un enlace de recuperación a tu correo." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
    }

    [HttpPost("restablecer-contrasena")]
    [AllowAnonymous]
    public async Task<IActionResult> RestablecerContrasena(ResetPasswordDto dto)
    {
        try
        {
            await _usuariosService.ResetPasswordAsync(dto);
            return Ok(new { mensaje = "Contraseña restablecida correctamente." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }


}
