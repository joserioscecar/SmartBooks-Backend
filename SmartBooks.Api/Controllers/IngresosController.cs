using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBooks.Application.DTOs.Ingresos;
using SmartBooks.Application.Interfaces;

namespace SmartBooks.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Vendedor")]
public class IngresosController : ControllerBase
{
    private readonly IIngresoService _service;

    public IngresosController(IIngresoService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Registrar([FromBody] CreateIngresoDto dto)
    {
        try
        {
            var result = await _service.RegistrarAsync(dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var ingreso = await _service.GetByIdAsync(id);
        return ingreso is null ? NotFound(new { mensaje = "Ingreso no encontrado." }) : Ok(ingreso);
    }

    [HttpGet]
    public async Task<IActionResult> Buscar([FromQuery] IngresoFilterDto dto)
    {
        var resultados = await _service.BuscarAsync(dto);
        return Ok(resultados);
    }

    [HttpGet("lotes")]
    public async Task<IActionResult> Lotes()
    {
        var resultados = await _service.GetLotesAsync();
        return Ok(resultados);
    }

}
