using Microsoft.AspNet.Identity;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebBiblioteca.Models;
using PagedList;
using PagedList.Mvc;

namespace WebBiblioteca.Controllers
{
    
    public class LibroController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Libro
        
        public ActionResult Index(string titulo, string autor, int? categoriaId, string isbn, int? anyo, int? page)
        {
            var libros = db.Libros.Include(l => l.categoria).AsQueryable();

            if (!string.IsNullOrEmpty(titulo))
                libros = libros.Where(l => l.Titulo.Contains(titulo));

            if (!string.IsNullOrEmpty(autor))
                libros = libros.Where(l => l.Autor.Contains(autor));

            if (!string.IsNullOrEmpty(isbn))
                libros = libros.Where(l => l.ISBN.Contains(isbn));

            if (anyo.HasValue)
                libros = libros.Where(l => l.Anyo == anyo.Value);

            if (categoriaId.HasValue && categoriaId > 0)
                libros = libros.Where(l => l.CategoriaId == categoriaId);

            ViewBag.Categorias = new SelectList(db.Categorias, "Id", "Nombre", categoriaId);

            int pageSize = 10;
            int pageNumber = page ?? 1;

            return View(libros.OrderBy(l => l.Titulo).ToPagedList(pageNumber, pageSize));

            //return View(libros.ToList());
        }




        // GET: Libro/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Libro libro = db.Libros.Find(id);
            if (libro == null)
            {
                return HttpNotFound();
            }
            return View(libro);
        }

        // GET: Libro/Create
        [Authorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            ViewBag.CategoriaId = new SelectList(db.Categorias, "Id", "Nombre");
            return View();
        }

        // POST: Libro/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Titulo,Autor,Anyo,ISBN,Stock,CategoriaId")] Libro libro)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Libros.Add(libro);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.CategoriaId = new SelectList(db.Categorias, "Id", "Nombre", libro.CategoriaId);
        //    return View(libro);
        //}

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult Create(Libro libro)
        {
            if (ModelState.IsValid)
            {
                db.Libros.Add(libro);
                db.SaveChanges();

                TempData["Success"] = "📚 El libro se creó correctamente.";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "❌ No se pudo crear el libro. Revisa los datos.";
            ViewBag.CategoriaId = new SelectList(db.Categorias, "Id", "Nombre", libro.CategoriaId);
            return View(libro);
        }

        // GET: Libro/Edit/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Libro libro = db.Libros.Find(id);
            if (libro == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoriaId = new SelectList(db.Categorias, "Id", "Nombre", libro.CategoriaId);
            return View(libro);
        }

        // POST: Libro/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = "Administrador")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Titulo,Autor,Anyo,ISBN,Stock,CategoriaId")] Libro libro)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(libro).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.CategoriaId = new SelectList(db.Categorias, "Id", "Nombre", libro.CategoriaId);
        //    return View(libro);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult Edit([Bind(Include = "Id,Titulo,Autor,Anyo,ISBN,Stock,CategoriaId,RowVersion")] Libro libro)
        {
            if (ModelState.IsValid)//Si cumple las restricciones del form
            {
                try
                {
                    db.Entry(libro).State = EntityState.Modified;//Comunica que cambia las propiedades a la base de datos y cambia RowVersion
                    db.SaveChanges();
                    //return RedirectToAction("Index");
                    /* ——— ÉXITO ——— */
                    TempData["Success"] = "✏️ Libro actualizado correctamente.";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single(); //Libros que han dado problemas
                    //var clientValues = (Libro)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();//Información de los libros

                    if (databaseEntry == null)
                    {
                        //ModelState.AddModelError("", "No se pudo guardar. El libro fue eliminado por otro usuario.");
                        /* ——— ERROR: otro lo ha borrado ——— */
                        TempData["Error"] = "❌ No se pudo guardar. El libro fue eliminado por otro usuario.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        var dbValues = (Libro)databaseEntry.ToObject();

                        ModelState.AddModelError("", "El registro que intentas modificar ha sido actualizado por otro usuario.");
                        ModelState.AddModelError("", "Los valores actuales de la base de datos se muestran a continuación.");

                        libro.RowVersion = dbValues.RowVersion;
                    }
                }
            }

            ViewBag.CategoriaId = new SelectList(db.Categorias, "Id", "Nombre", libro.CategoriaId);
            return View(libro);
        }



        // GET: Libro/Delete/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Libro libro = db.Libros.Find(id);
            if (libro == null)
            {
                return HttpNotFound();
            }
            return View(libro);
        }

        // POST: Libro/Delete/5
        //[Authorize(Roles = "Administrador")]
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Libro libro = db.Libros.Find(id);
        //    db.Libros.Remove(libro);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult DeleteConfirmed(int id, byte[] rowVersion)
        {
            var libro = db.Libros.Find(id);
            if (libro == null)
            {
                TempData["Error"] = "El libro ya ha sido eliminado.";
                return RedirectToAction("Index");
            }

            db.Entry(libro).OriginalValues["RowVersion"] = rowVersion;

            try
            {
                db.Libros.Remove(libro);
                db.SaveChanges();
                TempData["Success"] = "🗑️ Libro eliminado correctamente.";
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["Error"] = "No se pudo eliminar. El libro fue modificado por otro usuario.";
                return RedirectToAction("Index");
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        
        [HttpPost]
        [Authorize]
        public ActionResult SolicitarPrestamo(int id)
        {
            var usuarioId = User.Identity.GetUserId();
            var usuario = db.Users.Find(usuarioId);
            //SI el usuario sigue bloqueado no deja solicitar
            if (usuario.BloqueadoHasta.HasValue && usuario.BloqueadoHasta.Value > DateTime.Now)
            {
                TempData["Info"] = $"No puedes solicitar libros hasta " +
                    $"el {usuario.BloqueadoHasta.Value.ToShortDateString()} por retraso en devoluciones.";
                return RedirectToAction("Details", new { id });
            }
            var libro = db.Libros.Find(id);
            if (libro == null || libro.Stock <= 0)//Si no existe el libro
            {
                return HttpNotFound();
            }

            // Verificar si ya tiene uno pendiente 
            bool yaSolicitado = db.Prestamos.Any(p =>
                p.LibroId == id &&
                p.UsuarioId == usuarioId &&
                (p.FechaPrestamo == null || p.FechaDevolucion == null));
            //Verifica si ya lo tienes activo
            if (yaSolicitado)
            {
                TempData["Info"] = "Ya has solicitado o tienes prestado este libro.";
                return RedirectToAction("Details", new { id });
            }

            var config = db.ConfiguracionSistema.FirstOrDefault();
            int duracion = config?.DiasPrestamo ?? 15;

            var prestamo = new Prestamo
            {
                LibroId = id,
                UsuarioId = usuarioId,
                FechaSolicitud = DateTime.Now,
                DuracionDias = duracion
            };

            db.Prestamos.Add(prestamo);
            db.SaveChanges();

            TempData["Info"] = "Solicitud registrada correctamente. Espera a que sea aprobada.";
            return RedirectToAction("Details", new { id });
        }

    }
}
