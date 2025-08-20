using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBiblioteca.Models
{
    [Table("Libro")]
    public class Libro
    {
        public int Id { get; set;}

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; }

        [Required]
        [StringLength(200)]
        public string Autor {  get; set; }
        public int Anyo { get; set; }
        public string ISBN { get; set; }
        public int? Stock { get; set;}
        
        //Categoria
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public virtual Categoria categoria {  get; set; }
        
        //Prestamos
        public virtual ICollection<Prestamo> Prestamos { get; set;}

        [Timestamp]
        public byte[] RowVersion { get; set; }

    }
}