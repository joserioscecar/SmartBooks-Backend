using SmartBooks.Application.DTOs.Libros;
using SmartBooks.Application.DTOs.Inventario;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBooks.Application.Interfaces
{
    public interface ILibroService
    {
        Task<int> CreateAsync(CreateLibroDto dto);
        Task<LibroDto?> GetByIdAsync(int id);
        Task<IEnumerable<LibroDto>> SearchAsync(string? nombre, string? nivel, int? tipo, string? edicion);
        Task UpdateAsync(int id, UpdateLibroDto dto);
        Task<IEnumerable<StockDto>> GetStockGroupedAsync();
    }
}
