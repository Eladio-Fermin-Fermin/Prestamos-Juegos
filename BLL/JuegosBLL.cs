using Microsoft.EntityFrameworkCore;
using PrestamosJuegos.DAL;
using PrestamosJuegos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace PrestamosJuegos.BLL
{
    public class JuegosBLL
    {
        //Metodo Existe.
        public static bool Existe(int id)
        {
            Contexto contexto = new Contexto();
            bool encontrado = false;

            try
            {
                encontrado = contexto.Juegos.Any(e => e.JuegoId == id);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }

            return encontrado;
        }

        //Metodo Insertar.
        private static bool Insertar(Juegos juegos)
        {
            bool paso = false;
            Contexto contexto = new Contexto();

            try
            {
                //Agregar la entidad que se desea insertar al contexto
                contexto.Juegos.Add(juegos);
                paso = contexto.SaveChanges() > 0;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }
            return paso;
        }

        //Metodo Guardar.
        public static bool Guardar(Juegos juegos)
        {
            if (!Existe(juegos.JuegoId))
                return Insertar(juegos);
            else
                return Modificar(juegos);
        }

        //Metodo Buscar.
        public static Juegos Buscar(int id)
        {
            Juegos juegos = new Juegos();
            Contexto contexto = new Contexto();

            try
            {
                juegos = contexto.Juegos.Find(id);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }
            return juegos;
        }

        //Metodo Modificar.
        private static bool Modificar(Juegos juegos)
        {
            bool paso = false;
            Contexto contexto = new Contexto();

            try
            {
                //marcar la entidad como modificada para que el contexto sepa como proceder
                contexto.Entry(juegos).State = EntityState.Modified;
                paso = contexto.SaveChanges() > 0;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }
            return paso;
        }

        //Metodo Eliminar.
        public static bool Eliminar(int id)
        {
            bool paso = false;
            Contexto contexto = new Contexto();

            try
            {
                //buscar la entidad que se desea eliminar
                var juegos = JuegosBLL.Buscar(id);

                if (juegos != null)
                {
                    contexto.Juegos.Remove(juegos); //remover la entidad
                    paso = contexto.SaveChanges() > 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }
            return paso;
        }

        //Metodo GetList.
        public static List<Juegos> GetList(Expression<Func<Juegos, bool>> criterio)
        {
            List<Juegos> Lista = new List<Juegos>();
            Contexto contexto = new Contexto();

            try
            {
                //obtener la lista y filtrarla según el criterio recibido por parametro.
                Lista = contexto.Juegos.Where(criterio).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }
            return Lista;
        }

        public static Juegos DoplicadoJuego(string descripcion)
        {
            Contexto contexto = new Contexto();
            List<Juegos> lista = new List<Juegos>();
            Juegos juego;

            try
            {
                lista = contexto.Juegos.ToList();
                juego = lista.Find(j => j.Descripcion == descripcion);

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                contexto.Dispose();
            }

            return juego;
        }
    }
}
