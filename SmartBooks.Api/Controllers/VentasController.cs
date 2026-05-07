using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBooks.Application.Interfaces;

namespace SmartBooks.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Vendedor")]
public class VentasController : ControllerBase
{
    private readonly IVentaService _ventaService;

    public VentasController(IVentaService ventaService)
    {
        _ventaService = ventaService;
    }

    [HttpPost]
    public async Task<IActionResult> CrearVenta(CreateVentaDto dto)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        if (userIdClaim == null)
            return Unauthorized(new { mensaje = "El token no contiene el ID del usuario." });

        if (!int.TryParse(userIdClaim, out var userId))
            return BadRequest(new { mensaje = "ID de usuario inválido en token." });

        try
        {
            var result = await _ventaService.CreateVentaAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { mensaje = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var venta = await _ventaService.GetByIdAsync(id);
        return venta is null ? NotFound(new { mensaje = "Venta no encontrada." }) : Ok(venta);
    }

    [HttpGet]
    public async Task<IActionResult> Buscar([FromQuery] DateTime? desde, [FromQuery] DateTime? hasta, [FromQuery] string? cliente, [FromQuery] int? libro)
    {
        var resultados = await _ventaService.SearchAsync(desde, hasta, cliente, libro);
        return Ok(resultados);
    }
}
