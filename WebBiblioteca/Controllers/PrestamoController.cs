using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebBiblioteca.Models;

namespace WebBiblioteca.Controllers
{
    [Authorize]
    public class PrestamoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Prestamo
        //public ActionResult Index()
        //{
        //    var prestamos = db.Prestamos.Include(p => p.Libro).Include(p => p.Usuario);
        //    return View(prestamos.ToList());
        //}
        //public ActionResult Index(int? page)
        //{
        //    int pageSize = 10;
        //    int pageNumber = page ?? 1;

        //    try
        //    {
        //        var prestamos = db.Prestamos
        //            .Include(p => p.Libro)
        //            .Include(p => p.Usuario)
        //            .OrderByDescending(p => p.FechaSolicitud)
        //            .ToPagedList(pageNumber, pageSize);

        //        if (!prestamos.Any())
        //        {
        //            TempData["Info"] = "No hay préstamos registrados en el sistema.";
        //        }

        //        return View(prestamos);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = "Error al cargar los préstamos: " + ex.Message;
        //        return View(new PagedList<WebBiblioteca.Models.Prestamo>(new List<WebBiblioteca.Models.Prestamo>(), 1, 1));
        //    }
        //}

        public ActionResult Index(
           string correo,
           DateTime? fechaInicio,
           DateTime? fechaFin,
           bool? soloIncidencias,
           int? diasRestantesMax,
           int? page)
        {
            var query = db.Prestamos
                          .Include(p => p.Libro)
                          .Include(p => p.Usuario)
                          .AsQueryable();

            /* — FILTROS — */
            if (!string.IsNullOrWhiteSpace(correo))
                query = query.Where(p => p.Usuario.Email.Contains(correo));

            if (fechaInicio.HasValue)
                query = query.Where(p => p.FechaPrestamo >= fechaInicio.Value);

            if (fechaFin.HasValue)
                query = query.Where(p => p.FechaPrestamo <= fechaFin.Value);

            if (soloIncidencias == true)
                query = query.Where(p => p.Incidencia);

            /* Días restantes (solo préstamos activos) */
            if (diasRestantesMax.HasValue)
            {
                var hoy = DateTime.Now;
                query = query.Where(p =>                      // solo activos
                             p.FechaPrestamo != null &&
                             p.FechaDevolucion == null)
                             .AsEnumerable()                  // pasamos a memoria
                             .Where(p =>
                             {//Para calcular los días restantes
                                 var restantes = (p.FechaPrestamo.Value
                                                 .AddDays(p.DuracionDias) - hoy).Days;
                                 return restantes <= diasRestantesMax.Value;
                             })
                             .AsQueryable();
            }

            /* Paginación */
            int pageSize = 10;
            int pageNumber = page ?? 1;

            /* Orden: más recientes primero */
            var paged = query.OrderByDescending(p => p.FechaPrestamo ?? p.FechaSolicitud)
                             .ToPagedList(pageNumber, pageSize);

            /* Para mantener los valores en la vista */
            ViewBag.Correo = correo;
            ViewBag.FechaInicio = fechaInicio?.ToString("yyyy-MM-dd");
            ViewBag.FechaFin = fechaFin?.ToString("yyyy-MM-dd");
            ViewBag.SoloIncidencias = soloIncidencias;
            ViewBag.DiasMax = diasRestantesMax;

            return View(paged);
        }



        // GET: Prestamo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prestamo prestamo = db.Prestamos.Find(id);
            if (prestamo == null)
            {
                return HttpNotFound();
            }
            return View(prestamo);
        }

        // GET: Prestamo/Create
        public ActionResult Create()
        {
            ViewBag.LibroId = new SelectList(db.Libros, "Id", "Titulo");
            ViewBag.UsuarioId = new SelectList(db.Users, "Id", "NombreCompleto");
            return View();
        }

        // POST: Prestamo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,LibroId,UsuarioId,FechaSolicitud,FechaPrestamo,FechaDevolucion,Incidencia,DuracionDias")] Prestamo prestamo)
        {
            if (ModelState.IsValid)
            {
                db.Prestamos.Add(prestamo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LibroId = new SelectList(db.Libros, "Id", "Titulo", prestamo.LibroId);
            ViewBag.UsuarioId = new SelectList(db.Users, "Id", "NombreCompleto", prestamo.UsuarioId);
            return View(prestamo);
        }

        // GET: Prestamo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prestamo prestamo = db.Prestamos.Find(id);
            if (prestamo == null)
            {
                return HttpNotFound();
            }
            ViewBag.LibroId = new SelectList(db.Libros, "Id", "Titulo", prestamo.LibroId);
            ViewBag.UsuarioId = new SelectList(db.Users, "Id", "NombreCompleto", prestamo.UsuarioId);
            return View(prestamo);
        }

        // POST: Prestamo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,LibroId,UsuarioId,FechaSolicitud,FechaPrestamo,FechaDevolucion,Incidencia,DuracionDias")] Prestamo prestamo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prestamo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LibroId = new SelectList(db.Libros, "Id", "Titulo", prestamo.LibroId);
            ViewBag.UsuarioId = new SelectList(db.Users, "Id", "NombreCompleto", prestamo.UsuarioId);
            return View(prestamo);
        }

        // GET: Prestamo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prestamo prestamo = db.Prestamos.Find(id);
            if (prestamo == null)
            {
                return HttpNotFound();
            }
            return View(prestamo);
        }

        // POST: Prestamo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Prestamo prestamo = db.Prestamos.Find(id);
            db.Prestamos.Remove(prestamo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize(Roles = "Administrador")]
        //public ActionResult Pendiente()
        //{
        //    var prestamos = db.Prestamos
        //        .Include(p => p.Libro)
        //        .Include(p => p.Usuario)
        //        .Where(p => p.FechaPrestamo == null && !p.Incidencia && !p.FechaDevolucion.HasValue)
        //        .ToList();

        //    return View(prestamos);
        //}

        public ActionResult Pendiente(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            var prestamosPendientes = db.Prestamos
                .Where(p => p.FechaPrestamo == null && p.Incidencia == false)
                .Include(p => p.Libro)
                .Include(p => p.Usuario)
                .OrderByDescending(p => p.FechaSolicitud)
                .ToPagedList(pageNumber, pageSize);

            return View(prestamosPendientes);
        }


        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public ActionResult Aprobar(int id)
        {
            var prestamo = db.Prestamos.Include(p => p.Libro).FirstOrDefault(p => p.Id == id);
            if (prestamo == null || prestamo.FechaPrestamo != null)
                return HttpNotFound();

            if (prestamo.Libro.Stock <= 0)
            {
                TempData["Error"] = "No hay ejemplares disponibles para este libro.";
                return RedirectToAction("Pendiente");
            }

            prestamo.FechaPrestamo = DateTime.Now;
            prestamo.Libro.Stock -= 1;
            db.SaveChanges();

            return RedirectToAction("Pendiente");
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public ActionResult Rechazar(int id)
        {
            var prestamo = db.Prestamos.Find(id);
            if (prestamo == null || prestamo.FechaPrestamo != null)
                return HttpNotFound();

            db.Prestamos.Remove(prestamo);
            db.SaveChanges();

            return RedirectToAction("Pendiente");
        }

        [Authorize]
        public ActionResult MiPrestamo(int? page)
        {
            string userId = User.Identity.GetUserId();
            int pageSize = 10;
            int pageNumber = page ?? 1;

            //var prestamos = db.Prestamos
            //    .Include(p => p.Libro)
            //    .Where(p => p.UsuarioId == userId && !p.FechaDevolucion.HasValue && !p.Incidencia)
            //    .ToList();

            var prestamos = db.Prestamos
                .Include(p => p.Libro)
                .Where(p => p.UsuarioId == userId
                    && !p.FechaDevolucion.HasValue
                    && !p.Incidencia)
                .OrderByDescending(p => p.FechaPrestamo ?? p.FechaSolicitud)
                .ToPagedList(pageNumber, pageSize);

            return View(prestamos);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Devolver(int id)
        {
            var userId = User.Identity.GetUserId();
            var prestamo = db.Prestamos
                .Include(p => p.Libro)
                .Include(p => p.Usuario)
                .FirstOrDefault(p => p.Id == id && p.UsuarioId == userId);

            if (prestamo == null || prestamo.FechaDevolucion.HasValue)
                return HttpNotFound();

            prestamo.FechaDevolucion = DateTime.Now;

            // Si no hay incidencia, sumamos stock
            if (!prestamo.Incidencia)
                prestamo.Libro.Stock += 1;

            // Si el préstamo venció, se bloquea al usuario
            var config = db.ConfiguracionSistema.FirstOrDefault();
            int diasBloqueo = config?.DiasBloqueoPorVencimiento ?? 7;

            var fechaLimite = prestamo.FechaPrestamo?.AddDays(prestamo.DuracionDias);
            if (fechaLimite.HasValue && DateTime.Now > fechaLimite.Value)
            {
                var usuario = prestamo.Usuario;
                usuario.BloqueadoHasta = DateTime.Now.AddDays(diasBloqueo);
            }

            db.SaveChanges();
            return RedirectToAction("MiPrestamo");
        }

    }
} 