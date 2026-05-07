using SmartBooks.Application.DTOs.Ingresos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBooks.Application.Interfaces
{
    public interface IIngresoService
    {
        Task<IngresoResultDto> RegistrarAsync(CreateIngresoDto dto);
        Task<IngresoDetalleDto?> GetByIdAsync(int id);
        Task<IEnumerable<IngresoResultDto>> BuscarAsync(IngresoFilterDto dto);
        Task<string[]> GetLotesAsync();
    }
}
