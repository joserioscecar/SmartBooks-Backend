using SmartBooks.Application.DTOs.Ventas;
using SmartBooks.Application.Interfaces;
using SmartBooks.Domain.Entities;
using SmartBooks.Domain.Exceptions;
using SmartBooks.Domain.Interfaces;

namespace SmartBooks.Application.Services;

public class VentaService : IVentaService
{
    private readonly IVentaRepository _ventaRepo;
    private readonly IClienteRepository _clienteRepo;
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IIngresoRepository _ingresoRepo;
    private readonly IInventarioRepository _inventarioRepo;
    private readonly IPdfGenerator _pdf;
    private readonly IEmailService _email;
    private readonly ILibroRepository  _libroRepository;

    public VentaService(
        IVentaRepository ventaRepo,
        IClienteRepository clienteRepo,
        IUsuarioRepository usuarioRepo,
        IIngresoRepository ingresoRepo,
        IInventarioRepository inventarioRepo,
        IPdfGenerator pdf,
        IEmailService email,
        ILibroRepository libroRepository
        )
    {
        _ventaRepo = ventaRepo;
        _clienteRepo = clienteRepo;
        _usuarioRepo = usuarioRepo;
        _ingresoRepo = ingresoRepo;
        _inventarioRepo = inventarioRepo;
        _pdf = pdf;
        _email = email;
        _libroRepository = libroRepository;
    }

    private async Task<string> GenerarNumeroReciboAsync()
    {
        int count = await _ventaRepo.CountAsync();
        int next = count + 1;
        return $"FAC-{DateTime.UtcNow.Year}-{next:D6}";
    }

    public async Task<VentaResultDto> CreateVentaAsync(CreateVentaDto dto, int usuarioId)
    {

        var usuario = await _usuarioRepo.GetByIdAsync(usuarioId);
        if (usuario is null)
            throw new UnauthorizedAccessException("Usuario no válido.");

        var cliente = await _clienteRepo.GetByIdentificacionAsync(dto.IdentificacionCliente);
        if (cliente is null)
            throw new RuleBusinessException("Cliente no existe.");

        if (string.IsNullOrWhiteSpace(dto.NumeroComprobante))
            throw new RuleBusinessException("Debe ingresar el número de comprobante.");

        if (dto.Items == null || dto.Items.Count == 0)
            throw new RuleBusinessException("Debe incluir al menos un libro.");

        foreach (var item in dto.Items)
        {
            var lote = await _inventarioRepo.GetByCodigoAndLibroAsync(item.Lote, item.LibroId);

            if (lote is null)
                throw new RuleBusinessException($"No existe lote '{item.Lote}' para el libro {item.LibroId}.");

            if (item.Cantidad <= 0)
                throw new RuleBusinessException("Cantidad inválida.");

            if (lote.StockDisponible < item.Cantidad)
                throw new RuleBusinessException($"Stock insuficiente en lote {lote.Lote}.");
        }


        var venta = new Venta
        {
            NumeroRecibo = await GenerarNumeroReciboAsync(),
            NumeroComprobante = dto.NumeroComprobante,
            ClienteId = cliente.Id,
            UsuarioId = usuario.Id,
            Observaciones = dto.Observaciones,
            Fecha = DateTime.UtcNow
        };

        decimal total = 0;

       
        foreach (var item in dto.Items)
        {
            var inventario = await _inventarioRepo.GetByCodigoAndLibroAsync(item.Lote, item.LibroId);

            var libro = await _libroRepository.GetByIdAsync(item.LibroId);


            var valorVentaPublico = _ingresoRepo.Queryable().Where(l => l.LibroId == item.LibroId && l.Lote == item.Lote).Select(p=>p.ValorVentaPublico).FirstOrDefault();

            if (valorVentaPublico ==0 )
                throw new ArgumentException($"No existe ingreso registrado para el lote {inventario!.Lote}.");


            var subtotal = valorVentaPublico * item.Cantidad;
            total += subtotal;

            inventario.StockDisponible -= item.Cantidad;

            await _clienteRepo.SaveChangesAsync();
            

            venta.Items.Add(new VentaItem
            {
                LibroId = item.LibroId,
                Lote = item.Lote,
                Cantidad = item.Cantidad,
                PrecioUnitario = valorVentaPublico
            });
        }

        venta.Total = total;

        await _ventaRepo.AddAsync(venta);
        await _ventaRepo.SaveChangesAsync();

        var ventaCompleta = await _ventaRepo.GetByIdWithDetailsAsync(venta.Id);

        if (ventaCompleta != null)
        {
            var pdfBytes = _pdf.GenerarFacturaPdf(ventaCompleta);

            await _email.SendEmailWithAttachmentAsync(
                cliente.Email,
                $"Factura - {ventaCompleta.NumeroRecibo}",
                "<p>Gracias por su compra.</p>",
                pdfBytes,
                $"Factura_{ventaCompleta.NumeroRecibo}.pdf"
            );
        }

        return new VentaResultDto
        {
            Id = venta.Id,
            NumeroRecibo = venta.NumeroRecibo,
            NumeroComprobante = venta.NumeroComprobante,
            Total = venta.Total,
            Fecha = venta.Fecha,
            ClienteNombre = cliente.Nombres
        };
    }

    public async Task<VentaDetalleDto?> GetByIdAsync(int id)
    {
        var v = await _ventaRepo.GetByIdWithDetailsAsync(id);
        if (v == null) return null;

        return new VentaDetalleDto
        {
            Id = v.Id,
            NumeroRecibo = v.NumeroRecibo,
            NumeroComprobante = v.NumeroComprobante,
            Total = v.Total,
            Fecha = v.Fecha,
            ClienteNombre = v.Cliente.Nombres
        };
    }

    public Task<IEnumerable<VentaListDto>> SearchAsync(DateTime? desde, DateTime? hasta, string? cliente, int? libro)
    {
        var q = _ventaRepo.QueryableWithDetails();

        if (desde.HasValue)
            q = q.Where(v => v.Fecha >= desde.Value);

        if (hasta.HasValue)
            q = q.Where(v => v.Fecha <= hasta.Value);

        if (!string.IsNullOrWhiteSpace(cliente))
            q = q.Where(v => v.Cliente.Identificacion.Contains(cliente));

        if (libro.HasValue)
            q = q.Where(v => v.Items.Any(i => i.LibroId == libro.Value));

        var list = q.ToList();

        var result = list.Select(v => new VentaListDto
        {
            Id = v.Id,
            NumeroRecibo = v.NumeroRecibo,
            NumeroComprobante = v.NumeroComprobante,
            Total = v.Total,
            Fecha = v.Fecha,
            ClienteNombre = v.Cliente.Nombres
        });

        return Task.FromResult(result);
    }
}
