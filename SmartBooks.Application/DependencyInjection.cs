using Microsoft.Extensions.DependencyInjection;
using SmartBooks.Application.Interfaces;
using SmartBooks.Application.Mapping;
using SmartBooks.Application.Services;

namespace SmartBooks.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IClientesService, ClientesService>();
            services.AddScoped<IUsuariosService, UsuariosService>();
            services.AddScoped<ILibroService, LibroService>();
            services.AddScoped<IIngresoService, IngresoService>();
            services.AddScoped<IVentaService, VentaService>();
            services.AddScoped<IInventarioService, InventarioService>();

            services.AddAutoMapper(cfg => { }, typeof(ClienteProfile).Assembly);

            return services;
        }
    }
}
