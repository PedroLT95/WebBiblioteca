using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using WebBiblioteca.Models;
using WebBiblioteca.Models.ViewModels;
using PagedList;
using PagedList.Mvc;

namespace WebBiblioteca.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuarioController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //public ActionResult Index(string filtroEmail, bool? mostrarSoloBloqueados, DateTime? fechaInicio, DateTime? fechaFin, int? page)
        //{
        //    var usuarios = db.Users.AsQueryable();

        //    if (!string.IsNullOrEmpty(filtroEmail))
        //    {
        //        usuarios = usuarios.Where(u => u.Email.Contains(filtroEmail));
        //    }

        //    if (mostrarSoloBloqueados == true)
        //    {
        //        usuarios = usuarios.Where(u => u.BloqueadoHasta.HasValue && u.BloqueadoHasta > DateTime.Now);
        //    }

        //    if (fechaInicio.HasValue)
        //    {
        //        usuarios = usuarios.Where(u => u.FechaRegistro >= fechaInicio.Value);
        //    }

        //    if (fechaFin.HasValue)
        //    {
        //        usuarios = usuarios.Where(u => u.FechaRegistro <= fechaFin.Value);
        //    }

        //    var resultado = usuarios
        //        .OrderBy(u => u.Email)
        //        .ToList()
        //        .Select(u => new UsuarioViewModel
        //        {
        //            Id = u.Id,
        //            NombreCompleto = u.NombreCompleto,
        //            Email = u.Email,
        //            EstaBloqueado = u.BloqueadoHasta.HasValue && u.BloqueadoHasta > DateTime.Now,
        //            FechaRegistro = u.FechaRegistro
        //        });

        //    int pageSize = 10;
        //    int pageNumber = page ?? 1;

        //    return View(usuarios.OrderBy(u => u.Email).ToPagedList(pageNumber, pageSize));


        //    //return View(resultado);
        //}

        public ActionResult Index(string nombre, string correo, DateTime? fechaInicio, DateTime? fechaFin, bool? bloqueado, int? page)
        {
            var query = db.Users.AsQueryable();

            if (!string.IsNullOrEmpty(nombre))
                query = query.Where(u => u.NombreCompleto.Contains(nombre));

            if (!string.IsNullOrEmpty(correo))
                query = query.Where(u => u.Email.Contains(correo));

            if (fechaInicio.HasValue)
                query = query.Where(u => u.FechaRegistro >= fechaInicio.Value);

            if (fechaFin.HasValue)
                query = query.Where(u => u.FechaRegistro <= fechaFin.Value);

            if (bloqueado.HasValue && bloqueado.Value)
                query = query.Where(u => u.BloqueadoHasta.HasValue && u.BloqueadoHasta > DateTime.Now);

            // Proyección a ViewModel antes de paginar
            var usuariosFiltrados = query
                .Select(u => new UsuarioViewModel
                {
                    Id = u.Id,
                    NombreCompleto = u.NombreCompleto,
                    Email = u.Email,
                    FechaRegistro = u.FechaRegistro,
                    BloqueadoHasta = u.BloqueadoHasta
                });

            // Paginación
            int pageSize = 10;
            int pageNumber = page ?? 1;

            return View(usuariosFiltrados.OrderBy(u => u.Email).ToPagedList(pageNumber, pageSize));
        }

    }

}
