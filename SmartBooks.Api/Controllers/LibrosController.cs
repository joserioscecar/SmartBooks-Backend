using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBooks.Application.DTOs.Libros;
using SmartBooks.Application.Interfaces;
using SmartBooks.Domain.Exceptions;
namespace SmartBooks.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Vendedor")]
public class LibrosController : ControllerBase
{

    private readonly ILibroService _libroService;

    public LibrosController(ILibroService libroService)
    {
        _libroService = libroService;

    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CreateLibroDto dto)
    {

        var id = await _libroService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id },
            new { Message = "Libro registrado de forma exitosa" }
        );

    }



    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {

        var libro = await _libroService.GetByIdAsync(id);
        if (libro is null)
            return NotFound(new { mensaje = "Libro no encontrado." });

        return Ok(libro);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] UpdateLibroDto dto)
    {

        await _libroService.UpdateAsync(id, dto);


        return Ok(new { mensaje = "Libro actualizado correctamente." });
    }


    [HttpGet]
    public async Task<IActionResult> VerStockPorLibro([FromQuery] BookFilterDto dto)
    {

        var stock = await _libroService.GetStockGroupedAsync(dto);

        return Ok(stock);
    }
}
