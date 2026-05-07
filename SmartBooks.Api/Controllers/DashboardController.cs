using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBooks.Domain.Interfaces;
namespace SmartBooks.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{

    private readonly ILibroRepository _libroRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IVentaRepository _ventaRepository;

    public DashboardController(ILibroRepository libroRepository, IClienteRepository clienteRepository, IVentaRepository ventaRepository)
    {
        _libroRepository = libroRepository;
        _clienteRepository = clienteRepository;
        _ventaRepository = ventaRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var hoy = DateOnly.FromDateTime(DateTime.Now);
        var totalLibros = await _libroRepository.GetCountAsync();
        var totalClientes = await _clienteRepository.GetCountAsync();
        var cantVentasMes = await _ventaRepository.CountMonthAsync(hoy);
        var totalVentasMes = await _ventaRepository.TotalMonthAsync(hoy);
        var ventasHoy = await _ventaRepository.GetByDateAsync(hoy);

        var ventasDetalleHoy = ventasHoy.Select(v => new
        {
            NumeroRecibo = v.NumeroRecibo,
            Total = v.Total,
            Fecha = v.Fecha
        }).ToList();

        return Ok(new
        {
            TotalLibros = totalLibros,
            TotalClientes = totalClientes,
            CantVentasMes = cantVentasMes,
            TotalVentasMes = totalVentasMes,
            VentasHoy = ventasDetalleHoy
        });
    }

}
