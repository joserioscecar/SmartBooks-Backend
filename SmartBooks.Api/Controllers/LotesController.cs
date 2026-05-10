using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBooks.Application.DTOs.Lotes;
using SmartBooks.Application.Interfaces;

namespace SmartBooks.Api.Controllers;

[Route("api/[controller]")]
[ApiController]

[Authorize(Roles = "Admin,Vendedor")]
public class LotesController : ControllerBase
{

    private readonly ILoteService _service;

    public LotesController(ILoteService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult> GetLotes()
    {
        return Ok(await _service.GetLotesAsync());
    }


    [HttpPost]
    public async Task<ActionResult> PostLotes(CrearLoteDto dto)
    {

        await _service.AddAsync(dto.Lote);

        return CreatedAtAction(nameof(GetLotes), new { dto.Lote });


    }


}
