using SmartBooks.Application.DTOs.Ingresos;

namespace SmartBooks.Application.Interfaces
{
    public interface IIngresoService
    {
        Task<IngresoResultDto> RegistrarAsync(CreateIngresoDto dto);
        Task<IngresoDetalleDto?> GetByIdAsync(int id);
        Task<IEnumerable<IngresoResultDto>> BuscarAsync(IngresoFilterDto dto);
        Task<int[]> GetLotesAsync();
    }
}
