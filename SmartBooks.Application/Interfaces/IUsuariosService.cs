using SmartBooks.Application.DTOs.Usuarios;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBooks.Application.Interfaces
{
    public interface IUsuariosService
    {
        Task<IEnumerable<UsuarioListDto>> SearchAsync(string? nombres, int? rol);
        Task<UsuarioDto?> GetByIdAsync(int id);
        Task UpdateAsync(int id, UpdateUsuarioDto dto);
        Task ChangeStateAsync(int id, bool activo);
        Task<UserProfileDto?> GetProfileAsync(int id);
        Task RegisterAsync(RegisterUsuarioDto dto);
        Task VerifyEmailAsync(string token);
        Task<TokenResultDto> LoginAsync(LoginUsuarioDto dto);
        Task RequestPasswordResetAsync(RequestPasswordResetDto dto);
        Task ResetPasswordAsync(ResetPasswordDto dto);
    }
}
