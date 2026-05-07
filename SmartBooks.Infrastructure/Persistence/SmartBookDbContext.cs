using Microsoft.EntityFrameworkCore;
using SmartBooks.Domain.Entities;
using SmartBooks.Domain.Enums;
using SmartBooks.Infrastructure.Security;
using System.Reflection.Emit;

namespace SmartBooks.Infrastructure.Persistence;

public class SmartBookDbContext : DbContext
{
    public SmartBookDbContext(DbContextOptions<SmartBookDbContext> opts) : base(opts) { }


    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Libro> Libros => Set<Libro>();
    public DbSet<Inventario> Inventarios => Set<Inventario>();
    public DbSet<Ingreso> Ingresos => Set<Ingreso>();

    public DbSet<Lote> Lotes => Set<Lote>();

    public DbSet<Venta> Ventas => Set<Venta>();
    public DbSet<VentaItem> VentaItems => Set<VentaItem>();


    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);


        b.Entity<Cliente>()
        .Property(c => c.FechaNacimiento)
        .HasConversion(
            v => v.ToDateTime(TimeOnly.MinValue), // Convertir DateOnly a DateTime
            v => DateOnly.FromDateTime(v)          // Convertir de DateTime a DateOnly
        );


        b.Entity<Cliente>().HasIndex(x => x.Identificacion).IsUnique();
        b.Entity<Cliente>().HasIndex(x => x.Email).IsUnique();
        b.Entity<Cliente>().HasIndex(x => x.Celular).IsUnique();


        b.Entity<Usuario>().HasIndex(x => x.Identificacion).IsUnique();
        b.Entity<Usuario>().HasIndex(x => x.Email).IsUnique();



        b.Entity<Usuario>()
            .Property(u => u.CreatedAt)
            .HasColumnType("datetime(6)");

        b.Entity<Usuario>()
            .Property(u => u.UpdatedAt)
            .HasColumnType("datetime(6)");



        b.Entity<Usuario>().HasData(new Usuario
        {
            Id = 1,
            Identificacion = "892201263",
            Nombres = "Administrador CDI",
            Email = "centrodeidiomas@cecar.edu.co",
            PasswordHash = PasswordHasherHelper.HashPassword("AdminCDI2026"),
            Rol = RolUsuario.Admin,
            Activo = true,
            CreatedAt = DateTime.UtcNow
        });


        b.Entity<Libro>()
            .HasIndex(x => new { x.Nombre, x.Nivel, x.Tipo, x.Edicion })
            .IsUnique();

        b.Entity<Inventario>().Property(x => x.StockDisponible)
            .IsConcurrencyToken();

         b.Entity<Venta>()
        .HasMany(v => v.Items)
        .WithOne(i => i.Venta)
        .HasForeignKey(i => i.VentaId)
        .OnDelete(DeleteBehavior.Cascade);


        b.Entity<Lote>().HasKey(l => l.Codigo);


    }



    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is Usuario)
            .ToList();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

}
