using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBooks.Application.Interfaces;

namespace SmartBooks.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Vendedor")]
public class InventariosController : ControllerBase
{
    private readonly IInventarioService _service;

    public InventariosController(IInventarioService service)
    {
        _service = service;
    }



    [HttpGet]
    public async Task<IActionResult> GetByLote(string? lote)
    {
        var lotes = await _service.GetLotesByLoteAsync(lote);

        if (lotes == null || !lotes.Any())
            return NotFound(new { mensaje = $"No existen lotes para lote '{lote}'." });

        return Ok(lotes);
    }

    /*
    [HttpGet("stock")]
    public async Task<IActionResult> VerStockPorLibro()
    {

        var stock = await _service.GetStockGroupedAsync();

        return Ok(stock);
    }

    */

}
