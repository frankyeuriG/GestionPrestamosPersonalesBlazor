
using System.ComponentModel.DataAnnotations;

namespace GestionPrestamosPersonales.Models
{
    public class PagosDetalle
    {

        [Key]
        public int Id { get; set; }

        public int PagoId { get; set; }
        
        public int PrestamoId { get; set; }

        [Range(minimum: 0.01, maximum: 1000000000000, ErrorMessage = "Indique el valor a pagar")]
        public double ValorPagado { get; set; }

    }
}
