using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBiblioteca.Models.ViewModels
{
    public class EstadisticasViewModel
    {
        public int TotalLibros { get; set; }
        public int PrestamosActivos { get; set; }
        public int PrestamosPendientes { get; set; }

        public int PrestamosVencidos { get; set; }
        public int PrestamosConIncidencia { get; set; }
        public int TotalUsuarios { get; set; }
        public List<LibroMasDemandado> LibrosMasDemandados { get; set; }
    }

    public class LibroMasDemandado
    {
        public string Titulo { get; set; }
        public int VecesPrestado { get; set; }
    }
}