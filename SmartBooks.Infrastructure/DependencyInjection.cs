using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartBooks.Application.Interfaces;
using SmartBooks.Domain.Interfaces;
using SmartBooks.Infrastructure.Persistence;
using SmartBooks.Infrastructure.Repositories;
using SmartBooks.Infrastructure.PDF;
using SmartBooks.Infrastructure.Security;
using SmartBooks.Application.Services;
using SmartBooks.Application.Security;
using SmartBooks.Infrastructure.Service;


namespace SmartBooks.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SmartBookDbContext>(opt =>
            opt.UseMySQL(
                configuration.GetConnectionString("MySql")
          
            )
        );

        services.AddScoped<ILoteRepository, LoteRepository>();
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<ILibroRepository, LibroRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IVentaRepository, VentaRepository>();
        services.AddScoped<IIngresoRepository, IngresoRepository>();
        services.AddScoped<IInventarioRepository, InventarioRepository>();


        services.AddScoped<ILoteService, LoteService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IPdfGenerator, PdfGenerator>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }
}
