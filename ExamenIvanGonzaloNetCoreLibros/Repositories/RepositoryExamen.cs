using ExamenIvanGonzaloNetCoreLibros.Data;
using ExamenIvanGonzaloNetCoreLibros.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamenIvanGonzaloNetCoreLibros.Repositories
{
    #region PROCEDURES
    //create procedure SP_MostrarPedidos(@idusuario int)
    //as

    // select* from VISTAPEDIDOS where IdUsuario = @idusuario order by FECHA
    //go


    // create procedure SP_PAGINARGRUPO_LIBROS
    // (@POSICION INT)
    // AS

    // select* from PaginacionLibros
    //where posicion >= @POSICION and posicion<(@POSICION + 6)
    // GO

    #endregion

    #region VISTAS

 //   CREATE VIEW PaginacionLibros 
 //   as
 //   select CAST(
 //   row_number() over (order by idlibro) as int)
	//as posicion, isnull(IdLibro, 0) as IdLibro
	//, Titulo, Autor, Editorial, Portada, Resumen, Precio, IdGenero from LIBROS

 //   go

    #endregion
    public class RepositoryExamen
    {
        private LibrosContext context;

        public RepositoryExamen(LibrosContext context)
        {
            this.context = context;
        }

        public List<Libros> GetLibros()
        {
            return this.context.Libros.ToList();
        }

        public List<Genero> GetGeneros()
        {
            var consulta = from datos in this.context.Generos
                           select datos;

            return consulta.ToList();
        }

        public Libros GetDetallesLibro(int idlibro)
        {
            return this.context.Libros.FirstOrDefault(x => x.IdLibro == idlibro);
        }

        public List<Libros> GetLibrosGenero(int idgenero)
        {
            var consulta = from datos in this.context.Libros
                           where datos.IdGenero == idgenero
                           select datos;

            return consulta.ToList();
        }

        public Usuario FindUsuario(string email, string password)
        {
            var consulta = from datos in this.context.Usuarios
                           where datos.Email == email && datos.Pass == password
                           select datos;

            return consulta.FirstOrDefault();
        }

        public Usuario UsuarioPerfil(int idusuario)
        {
            return this.context.Usuarios.FirstOrDefault(z => z.IdUsuario == idusuario);
        }

        public List<Libros> GetLibrosCarrito(List<int> ids)
        {
            var consulta = from datos in this.context.Libros
                           where ids.Contains(datos.IdLibro)
                           select datos;
            return consulta.ToList();
        }

        public int GetMaxIdFactura()
        {
            return this.context.Pedidos.Max(z => z.IdPedido) + 1;
        }

        public int GetMaxIdPedido()
        {
            if (this.context.Pedidos.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this.context.Pedidos.Max(z => z.IdPedido) + 1;
            }
        }


        public void InsertarPedido(int idfactura, int idlibro, int idusuario)
        {
            Pedidos pedido = new Pedidos();
            pedido.IdPedido = this.GetMaxIdPedido();
            pedido.IdFactura = idfactura;
            pedido.Fecha = DateTime.Now;
            pedido.IdLibro = idlibro;
            pedido.IdUsuario = idusuario;
            pedido.Cantidad = 1;

            this.context.Pedidos.Add(pedido);
            this.context.SaveChanges();
        }

        //public List<Libros> GetLibrosGenero()
        //{
        //    return this.context.Libros.ToList();
        //}

        public List<VistaPedidos> GetPedidosUsuario(int idusuario)
        {
            string sql = "SP_MostrarPedidos @idusuario ";
            SqlParameter paramId = new SqlParameter("@idusuario", idusuario);
           
            var consulta = this.context.VistaPedidos.FromSqlRaw(sql, paramId);
            List<VistaPedidos> pedidos = consulta.ToList();
           
            return pedidos;
        }

        public int GetNumeroRegistrosVistaLibros()
        {
            return this.context.LibrosPrueba.Count();
        }

        public List<LibrosPrueba> GetVistaLibrosPrueba(int posicion)
        {
            string sql = "SP_PAGINARGRUPO_LIBROS @POSICION";
            SqlParameter paramposicion =
            new SqlParameter("@POSICION", posicion);
            var consulta =
            this.context.LibrosPrueba.FromSqlRaw
            (sql, paramposicion);
            return consulta.ToList();

        }


    }
}
