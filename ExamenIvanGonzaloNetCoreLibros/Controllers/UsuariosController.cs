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
  
    public class UsuariosController : Controller
    {
        private RepositoryExamen repo;

        public UsuariosController(RepositoryExamen repo)
        {
            this.repo = repo;
        }

        [AuthorizeUsuarios]
        public IActionResult MiPerfil()
        {
            Usuario usuario = this.repo.UsuarioPerfil(int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value));        
            return View(usuario);
        }

        [AuthorizeUsuarios]
        public IActionResult MisPedidos()
        {
            int idusuario = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            List<VistaPedidos> pedidos = this.repo.GetPedidosUsuario(idusuario);
            return View(pedidos);
        }
    }
}
