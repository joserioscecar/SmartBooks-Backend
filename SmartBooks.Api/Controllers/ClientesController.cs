using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBooks.Application.DTOs.Clientes;
using SmartBooks.Application.Interfaces;

namespace SmartBooks.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Vendedor")]

public class ClientesController : ControllerBase
{
    private readonly IClientesService _service;

    public ClientesController(IClientesService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Create(CreateClienteDto dto)
    {
        try
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByIdent), new { identificacion = created.Identificacion }, created);
        }
        catch (InvalidOperationException ex)
        {
            return UnprocessableEntity(new { message = ex.Message });
        }
    }

    [HttpGet("{identificacion}")]
    public async Task<IActionResult> GetByIdent(string identificacion)
    {
        var c = await _service.GetByIdentificacionAsync(identificacion);
        return c is null ? NotFound(new { message = "Cliente no encontrado" }) : Ok(c);
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string? nombres)
    {
        var items = await _service.SearchAsync(nombres);
        return Ok(items);
    }

    [HttpPut("{identificacion}")]
    public async Task<IActionResult> Update(string identificacion, UpdateClienteDto dto)
    {
        try
        {
            await _service.UpdateAsync(identificacion, dto);
            return Ok(new { message = "Cliente actualizado correctamente" });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Cliente no encontrado" });
        }
    }
}
