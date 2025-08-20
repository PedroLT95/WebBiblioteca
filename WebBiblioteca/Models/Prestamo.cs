using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBiblioteca.Models
{
    [Table("Prestamo")]
    public class Prestamo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Libro")]
        public int LibroId { get; set; }
        [ForeignKey("LibroId")]
        [InverseProperty("Prestamos")]
        public virtual Libro Libro { get; set; }
        [Required]
        [Display(Name = "Usuario")]
        public string UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public virtual ApplicationUser Usuario { get; set; }
        [Display(Name = "Fecha de Solicitud")]
        [DataType(DataType.DateTime)]
        public DateTime? FechaSolicitud { get; set; } = DateTime.Now;
        [Display(Name = "Fecha de Préstamo")]
        [DataType(DataType.DateTime)]
        public DateTime? FechaPrestamo { get; set; }
        [Display(Name = "Fecha de Devolución")]
        [DataType(DataType.DateTime)]
        public DateTime? FechaDevolucion { get; set; }
        [NotMapped]
        public string Estado
        {
            get
            {
                if (FechaDevolucion.HasValue) return "Devuelto";
                if (Incidencia) return "Incidencia";
                if (FechaPrestamo.HasValue && DateTime.Now > FechaPrestamo.Value.AddDays(DuracionDias))
                    return "Vencido";
                if (FechaPrestamo.HasValue) return "Prestado";
                return "Pendiente";
            }
        }
        [Display(Name = "Incidencia")]
        public bool Incidencia { get; set; }
        [Display(Name = "Duración (días)")]
        [Range(1, 365)]
        public int DuracionDias { get; set; } = 15;
    }
}