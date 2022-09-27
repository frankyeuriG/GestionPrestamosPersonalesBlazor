using System.ComponentModel.DataAnnotations;

namespace GestionPrestamosPersonales.Models
{
    public class Personas
    {
        [Key]
        public int PersonaId { get; set; }

        [Required(ErrorMessage = "La nombre es requerido")]
        public string? Nombres { get; set; }

        public string? Telefono { get; set; }

        public string? Celular { get; set; }

        public string? Email { get; set; }

        public string? Direccion { get; set; }

        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El id de una ocupacion es requerido")]
        public int OcupacionId { get; set; }
               
        public double Balance { get; set; } 
    }
}
