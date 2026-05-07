using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult> PostLotes([FromBody] string lote)
    {

        await _service.AddAsync(lote);

        return CreatedAtAction(nameof(GetLotes), new { lote }, lote);


    }


}
