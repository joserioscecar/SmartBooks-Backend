using SmartBooks.Application.DTOs.Clientes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBooks.Application.Interfaces
{
    public interface IClientesService
    {
        Task<ClienteDto> CreateAsync(CreateClienteDto dto);
        Task<ClienteDto?> GetByIdentificacionAsync(string identificacion);
        Task<IEnumerable<ClienteDto>> SearchAsync(string? nombres);
        Task UpdateAsync(string identificacion, UpdateClienteDto dto);
    }
}
