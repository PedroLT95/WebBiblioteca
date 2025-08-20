using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBiblioteca.Models.ViewModels
{
    public class UsuarioViewModel
    {
        public string Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public bool EstaBloqueado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? BloqueadoHasta { get; set; }


        // Filtros
        public string FiltroEmail { get; set; }
        public bool MostrarSoloBloqueados { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}