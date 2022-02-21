using ExamenIvanGonzaloNetCoreLibros.Models;
using ExamenIvanGonzaloNetCoreLibros.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExamenIvanGonzaloNetCoreLibros.Controllers
{
    public class ManageController : Controller
    {
        private RepositoryExamen repo;

        public ManageController(RepositoryExamen repo)
        {
            this.repo = repo;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            Usuario usuario = this.repo.FindUsuario(email, password);

            if (usuario != null)
            {
                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

                Claim claimName = new Claim(ClaimTypes.Name, usuario.Nombre);
                Claim claimId = new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString());     
                Claim claimImagen = new Claim("imagen", usuario.Foto); 
         
                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);

                identity.AddClaim(claimName);
                identity.AddClaim(claimId);
                identity.AddClaim(claimImagen);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.Now.AddMinutes(15)
                });

                if (TempData["controller"] != null && TempData["action"] != null)
                {
                    string controller = TempData["controller"].ToString();
                    string action = TempData["action"].ToString();

                    return RedirectToAction(action, controller);
                }
                else
                {
                    return RedirectToAction("Index", "Libros");
                }

            }
            else
            {
                ViewBag.Mensaje = "Credenciales incorrectas";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Libros");
        }

        public IActionResult ErrorAcceso()
        {
            return View();
        }

    }
}
