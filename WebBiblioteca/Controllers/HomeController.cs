using System;
using System.Linq;
using System.Web.Mvc;
using WebBiblioteca.Models;
using WebBiblioteca.Models.ViewModels;

namespace WebBiblioteca.Controllers
{
    public class HomeController : Controller
    {
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public ActionResult Index()
        {
            if (User.IsInRole("Administrador"))
            {
                var db = new ApplicationDbContext();

                var viewModel = new EstadisticasViewModel
                {
                    TotalLibros = db.Libros.Count(),
                    PrestamosActivos = db.Prestamos.Count(p => p.FechaPrestamo != null && p.FechaDevolucion == null && p.Incidencia == false),
                    PrestamosPendientes = db.Prestamos.Count(p => p.FechaPrestamo == null && p.Incidencia == false),
                    PrestamosVencidos = db.Prestamos
                        .Where(p => p.FechaPrestamo != null && p.FechaDevolucion == null)
                        .AsEnumerable()
                        .Count(p => p.FechaPrestamo.Value.AddDays(p.DuracionDias) < DateTime.Now),

                    PrestamosConIncidencia = db.Prestamos.Count(p => p.Incidencia == true),
                    TotalUsuarios = db.Users.Count(),
                    LibrosMasDemandados = db.Prestamos
                        .Where(p => p.FechaPrestamo != null)
                        .GroupBy(p => p.Libro.Titulo)
                        .Select(g => new LibroMasDemandado
                        {
                            Titulo = g.Key,
                            VecesPrestado = g.Count()
                        })
                        .OrderByDescending(g => g.VecesPrestado)
                        .Take(5)
                        .ToList()
                };

                ViewBag.Estadisticas = viewModel;
            }

            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}