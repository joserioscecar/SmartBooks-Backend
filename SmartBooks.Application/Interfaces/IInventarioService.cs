using SmartBooks.Application.DTOs.Inventario;
using SmartBooks.Application.DTOs.Libros;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBooks.Application.Interfaces
{
    public interface IInventarioService
    {
        Task<IEnumerable<InventarioDto>> GetLotesByLoteAsync(string? lote);
        Task<IEnumerable<StockDto>> GetStockGroupedAsync();


    }
}
