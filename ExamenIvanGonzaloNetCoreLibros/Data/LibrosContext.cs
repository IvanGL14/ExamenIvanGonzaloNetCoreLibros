using ExamenIvanGonzaloNetCoreLibros.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamenIvanGonzaloNetCoreLibros.Data
{
        public class LibrosContext : DbContext
        {
            public LibrosContext(DbContextOptions<LibrosContext> options) : base(options) { }
            public DbSet<Libros> Libros { get; set; }
            public DbSet<Usuario> Usuarios { get; set; }
            public DbSet<Genero> Generos { get; set; }
            public DbSet<Pedidos> Pedidos { get; set; }
            public DbSet<VistaPedidos> VistaPedidos { get; set; }
            public DbSet<LibrosPrueba> LibrosPrueba { get; set; }
    }
    }

