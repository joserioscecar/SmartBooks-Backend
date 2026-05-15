using SmartBooks.Domain.Entities;

namespace SmartBooks.Domain.Interfaces;

public interface ILoteRepository
{
    Task<IEnumerable<Lote>> GetLotesAsync();

    Task AddAsync(Lote lote);

    Task<bool> ExisteLoteAsync(int codigo);
}
