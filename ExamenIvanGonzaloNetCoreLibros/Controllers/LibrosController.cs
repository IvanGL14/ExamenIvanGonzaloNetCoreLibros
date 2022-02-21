using ExamenIvanGonzaloNetCoreLibros.Extension;
using ExamenIvanGonzaloNetCoreLibros.Filters;
using ExamenIvanGonzaloNetCoreLibros.Models;
using ExamenIvanGonzaloNetCoreLibros.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExamenIvanGonzaloNetCoreLibros.Controllers
{
    public class LibrosController : Controller
    {
        private RepositoryExamen repo;

        public LibrosController(RepositoryExamen repo)
        {
            this.repo = repo;
        }

        public IActionResult Prueba(int? posicion)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            int numeroregistros = this.repo.GetNumeroRegistrosVistaLibros();
            ViewData["NUMEROREGISTROS"] = numeroregistros;
            List<LibrosPrueba> libros =
            this.repo.GetVistaLibrosPrueba(posicion.Value);
            return View(libros);

        }

        public IActionResult Index()
        {
            List<Libros> libros = this.repo.GetLibros();
            return View(libros);
        }

        public IActionResult LibrosGenero(int idgenero)
        {
            List<Libros> libros = this.repo.GetLibrosGenero(idgenero);
            return View(libros);
        }

        public IActionResult DetallesLibro(int idlibro)
        {
            Libros libro = this.repo.GetDetallesLibro(idlibro);
            return View(libro);
        }

        public IActionResult AddCarrito(int? idlibro)
        {
            if (idlibro != null)
            {
                List<int> listalibros;

                if (HttpContext.Session.GetObject<List<int>>("IDLIBROS") == null)
                {
                    //NO EXISTE NADA EN SESSION, PUES CREAMOS LA COLECCION
                    listalibros = new List<int>();
                }
                else
                {
                    //EXISTE Y RECUPERAMOS LA COLECCION DE SESSION
                    listalibros = HttpContext.Session.GetObject<List<int>>("IDLIBROS");
                }
                //ALMACENAMOS EL ID EN LA COLECICON
                listalibros.Add(idlibro.Value);
                //ALMACENAMOS LA COLECCION EN SESSION
                HttpContext.Session.SetObject("IDLIBROS", listalibros);
            }

            return RedirectToAction("Index");

        }

        public IActionResult CarritoCompra(int? idlibro)
        {
            List<int> carrito = HttpContext.Session.GetObject<List<int>>("IDLIBROS");
            if (carrito == null)
            {
                ViewBag.mensaje = "Actualmente no tienes ningún producto en el carrito";
                return View();
            }
            else
            {
                if (idlibro != null)
                {
                    carrito.Remove(idlibro.Value);

                    if (carrito.Count == 0)
                    {
                        HttpContext.Session.Remove("IDLIBROS");
                        ViewBag.mensaje = "Actualmente no tienes ningún producto en el carrito";
                    }
                    else
                    {
                        HttpContext.Session.SetObject("IDLIBROS", carrito);
                    }

                }

                List<Libros> libros = this.repo.GetLibrosCarrito(carrito);
                return View(libros);
            }
        }

        [AuthorizeUsuarios]
        public IActionResult Checkout()
        {
            int idfactura = this.repo.GetMaxIdFactura();
            int idusuario = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            List<int> carrito = HttpContext.Session.GetObject<List<int>>("IDLIBROS");
            List<Libros> libros = this.repo.GetLibrosCarrito(carrito);

            foreach (Libros libro in libros)
            {
                this.repo.InsertarPedido(idfactura, libro.IdLibro, idusuario) ;
            }
            HttpContext.Session.Remove("IDLIBROS");
            return RedirectToAction("MisPedidos", "Usuarios");
        }

    }
}
