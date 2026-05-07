using SmartBooks.Domain.Entities;

namespace SmartBooks.Application.Interfaces
{
    public interface IPdfGenerator
    {
        byte[] GenerarFacturaPdf(Venta venta);
    }
}
