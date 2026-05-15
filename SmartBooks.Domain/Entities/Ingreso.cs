using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartBooks.Domain.Entities
{
    public class Ingreso
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        [Required]
        public int LibroId { get; set; }

        [ForeignKey(nameof(LibroId))]
        public Libro Libro { get; set; } = default!;

        public int Lote { get; set; } = default!;

        [Required]
        public int Unidades { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorCompra { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorVentaPublico { get; set; }
    }
}

