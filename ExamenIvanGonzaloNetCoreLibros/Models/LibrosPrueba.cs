using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ExamenIvanGonzaloNetCoreLibros.Models
{
    [Table("PaginacionLibros")]

    public class LibrosPrueba
    {
        [Key]
        [Column("IDLIBRO")]
        public int IdLibro { get; set; }

        [Column("TITULO")]
        public string Titulo { get; set; }

        [Column("AUTOR")]
        public string Autor { get; set; }

        [Column("EDITORIAL")]
        public string Editorial { get; set; }

        [Column("PORTADA")]
        public string Portada { get; set; }

        [Column("PRECIO")]
        public int Precio { get; set; }

        [Column("IDGENERO")]
        public int IdGenero { get; set; }

        [Column("RESUMEN")]
        public string Resumen { get; set; }

        [Column("POSICION")]
        public int Posicion { get; set; }
    }
}
