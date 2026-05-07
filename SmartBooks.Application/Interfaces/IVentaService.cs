using SmartBooks.Application.DTOs.Ventas;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace SmartBooks.Application.Interfaces
{
    public interface IVentaService
    {
        Task<VentaResultDto> CreateVentaAsync(CreateVentaDto dto, int usuarioId);
        Task<VentaDetalleDto?> GetByIdAsync(int id);
        Task<IEnumerable<VentaListDto>> SearchAsync(DateTime? desde, DateTime? hasta, string? cliente, int? libro);
    }
}
