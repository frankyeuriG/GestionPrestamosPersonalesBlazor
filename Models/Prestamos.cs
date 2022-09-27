
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionPrestamosPersonales.Models
{
    public class Prestamos
    {
        [Key]
        public int PrestamoId { get; set; }

        [Required(ErrorMessage = "Favor de Ingresar La Fecha de inicio.")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Favor de Ingresar la fecha de vencimiento")]
        public DateTime Vence { get; set; }

        [Range(1, 200000000)]
        [Required(ErrorMessage = "Favor de ingresar el monto")]
        public double Monto { get; set; }

        [Required(ErrorMessage = "Favor Selecccionar una pesona")]
        public int PersonaId { get; set; }

        [Required(ErrorMessage = "Favor de Ingresar EL concepto")]
        public string? Concepto { get; set; }
         
        public double Balance { get; set; }
    }
}
