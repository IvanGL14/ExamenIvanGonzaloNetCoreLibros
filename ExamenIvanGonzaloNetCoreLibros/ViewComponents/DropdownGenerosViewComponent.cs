using ExamenIvanGonzaloNetCoreLibros.Models;
using ExamenIvanGonzaloNetCoreLibros.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamenIvanGonzaloNetCoreLibros.ViewComponents
{
    public class DropdownGenerosViewComponent : ViewComponent
    {
        private RepositoryExamen repo;

        public DropdownGenerosViewComponent(RepositoryExamen repo)
        {
            this.repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Genero> generos = this.repo.GetGeneros();
            return View(generos);
        }
    }
}

