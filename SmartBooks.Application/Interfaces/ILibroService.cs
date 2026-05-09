using SmartBooks.Application.DTOs.Libros;

namespace SmartBooks.Application.Interfaces;

public interface ILibroService
{
    Task<int> CreateAsync(CreateLibroDto dto);
    Task<LibroDto?> GetByIdAsync(int id);
    Task<IEnumerable<LibroDto>> SearchAsync(BookFilterDto dto);
    Task UpdateAsync(int id, UpdateLibroDto dto);
    Task<IEnumerable<StockDto>> GetStockGroupedAsync(BookFilterDto dto);
}
