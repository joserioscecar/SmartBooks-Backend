using AutoMapper;
using SmartBooks.Application.DTOs.Usuarios;
using SmartBooks.Application.Interfaces;
using SmartBooks.Domain.Interfaces;
using SmartBooks.Domain.Entities;
using SmartBooks.Domain.Enums;
using Microsoft.Extensions.Configuration;

namespace SmartBooks.Application.Services;

public class UsuariosService : IUsuariosService
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IEmailService _email;
    private readonly IJwtTokenService _jwt;
    private readonly IPasswordHasher _hasher;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;

    public UsuariosService(IUsuarioRepository usuarioRepo, IEmailService email, IJwtTokenService jwt, IPasswordHasher hasher, IMapper mapper,IConfiguration config)
    {
        _usuarioRepo = usuarioRepo;
        _email = email;
        _jwt = jwt;
        _hasher = hasher;
        _mapper = mapper;
        _config = config;
    }

    public async Task<IEnumerable<UsuarioListDto>> SearchAsync(string? nombres, int? rol)
    {
        var users = await _usuarioRepo.SearchAsync(nombres, rol);
        return _mapper.Map<IEnumerable<UsuarioListDto>>(users);
    }

    public async Task<UsuarioDto?> GetByIdAsync(int id)
    {
        var u = await _usuarioRepo.GetByIdAsync(id);
        return u is null ? null : _mapper.Map<UsuarioDto>(u);
    }

    public async Task UpdateAsync(int id, UpdateUsuarioDto dto)
    {
        var usuario = await _usuarioRepo.GetByIdAsync(id);
        if (usuario is null)
            throw new KeyNotFoundException("Usuario no encontrado.");

        if (!dto.Email.EndsWith(_config.GetValue<string>("Dominio")!))
            throw new ArgumentException($"Solo se permiten correos institucionales (@{_config.GetValue<string>("Dominio")!}).");

        usuario.Nombres = dto.Nombres;
        usuario.Email = dto.Email.ToLowerInvariant();
        usuario.Rol = (RolUsuario)dto.Rol;
        usuario.Activo = dto.Activo;

        await _usuarioRepo.SaveChangesAsync();
    }

    public async Task<bool> ChangeStateAsync(int id)
    {
        var usuario = await _usuarioRepo.GetByIdAsync(id);
        if (usuario is null)
            throw new KeyNotFoundException("Usuario no encontrado.");

        usuario.Activo = !usuario.Activo;
        await _usuarioRepo.SaveChangesAsync();
        return usuario.Activo;
    }

    public async Task<UserProfileDto?> GetProfileAsync(int id)
    {
        var u = await _usuarioRepo.GetByIdAsync(id);
        return u is null ? null : _mapper.Map<UserProfileDto>(u);
    }

    public async Task RegisterAsync(RegisterUsuarioDto dto)
    {
        if (await _usuarioRepo.GetByEmailAsync(dto.Email) != null)
            throw new ArgumentException("El correo ya está registrado.");

        if (!dto.Email.EndsWith(_config.GetValue<string>("Dominio")!))
            throw new ArgumentException($"Solo se permiten correos institucionales (@{_config.GetValue<string>("Dominio")!}).");

        var usuario = new Usuario
        {
            Identificacion = dto.Identificacion,
            Nombres = dto.Nombres,
            Email = dto.Email.ToLowerInvariant(),
            PasswordHash = _hasher.HashPassword(dto.Password),
            Rol = (RolUsuario)dto.Rol,
            EmailVerificationToken = Guid.NewGuid().ToString("N"),
            EmailTokenExpiresAt = DateTime.UtcNow.AddHours(1),
            Activo = false
        };

        await _usuarioRepo.AddAsync(usuario);
        await _usuarioRepo.SaveChangesAsync();

        var enlaceVerificacion = $"{_config.GetValue<string>("EnlaceVerificacionCorreo")}{usuario.EmailVerificationToken}";
        await _email.SendEmailAsync(usuario.Email, "Verificación de correo - SmartBook", $"<p>Hola {usuario.Nombres},</p><p>Haz clic <a href='{enlaceVerificacion}'>aquí</a> para verificar tu cuenta.</p>");
    }

    public async Task VerifyEmailAsync(string token)
    {
        var usuario = await _usuarioRepo.GetByEmailVerificationTokenAsync(token);
        if (usuario is null || usuario.EmailTokenExpiresAt < DateTime.UtcNow)
            throw new ArgumentException("El token es inválido o ha expirado.");

        usuario.EmailVerificationToken = null;
        usuario.EmailTokenExpiresAt = null;
        usuario.Activo = true;
        await _usuarioRepo.SaveChangesAsync();
    }

    public async Task<TokenResultDto> LoginAsync(LoginUsuarioDto dto)
    {
        var usuario = await _usuarioRepo.GetByEmailAsync(dto.Email.ToLowerInvariant());
        if (usuario is null || !_hasher.VerifyPassword(dto.Password, usuario.PasswordHash))
            throw new UnauthorizedAccessException("Credenciales inválidas.");

        if (!usuario.Activo)
            throw new UnauthorizedAccessException("Cuenta inactiva o sin verificar.");

        var token = _jwt.GenerateToken(usuario.Id, usuario.Nombres, usuario.Rol.ToString());
        return new TokenResultDto
        {
            Token = token,
            Usuario = _mapper.Map<UsuarioDto>(usuario)
        };
    }

    public async Task RequestPasswordResetAsync(RequestPasswordResetDto dto)
    {
        var usuario = await _usuarioRepo.GetByEmailAsync(dto.Email.ToLowerInvariant());
        if (usuario is null)
            throw new KeyNotFoundException("No existe un usuario con ese correo.");

        usuario.ResetCodePassword = new Random().Next(100000, 999999).ToString(); 
        usuario.ResetCodePasswordExpiresAt = DateTime.UtcNow.AddHours(1);
        await _usuarioRepo.SaveChangesAsync();

        var html = $@"
                <h3>Recuperación de contraseña - SmartBook</h3>
                <p>Hola {usuario.Nombres},</p>
                <p>Has solicitado restablecer tu contraseña. Escribe el siguiente código</p>
                <p>Código <strong>{usuario.ResetCodePassword}</strong></p>
                <p>Este enlace expirará en 1 hora.</p>
            ";

        await _email.SendEmailAsync(usuario.Email, "Recuperación de contraseña", html);
    }

    public async Task ResetPasswordAsync(ResetPasswordDto dto)
    {
        var usuario = await _usuarioRepo.GetByResetTokenAsync(dto.Codigo);
        if (usuario is null || usuario.ResetCodePasswordExpiresAt < DateTime.UtcNow)
            throw new ArgumentException("El código de restablecimiento es inválido o ha expirado.");

        usuario.PasswordHash = _hasher.HashPassword(dto.NewPassword);
        usuario.ResetCodePassword = null;
        usuario.ResetCodePasswordExpiresAt = null;
        

        if (await _usuarioRepo.SaveChangesAsync() > 0) {


            var html = $@"
                <h3>Restablecimiento de contraseña</h3>
                <p>Hola {usuario.Nombres},</p>
                <p>La contraseña de tu cuenta ha sido restablecida exitosamente.</p>";

            await _email.SendEmailAsync(usuario.Email, "Restablecimiento de contraseña exitoso - SmartBook", html);


        }

    }
}
