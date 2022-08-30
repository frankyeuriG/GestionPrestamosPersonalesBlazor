using System.ComponentModel.DataAnnotations;

namespace GestionPrestamosPersonales.Models
{
    public class Ocupaciones
    {
        [Key]
        public int OcupacionId { get; set; }
        public string? Descripcion { get; set; }

    }
}
