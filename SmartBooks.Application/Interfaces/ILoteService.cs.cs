using SmartBooks.Application.DTOs.Lotes;
using SmartBooks.Domain.Entities;

namespace SmartBooks.Application.Interfaces;

public interface ILoteService
{
   Task<IEnumerable<LoteDto>> GetLotesAsync();

   Task AddAsync(string lote);
}
