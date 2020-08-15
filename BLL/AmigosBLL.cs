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
    public class AmigosBLL
    {

        //Metodo Existe.
        public static bool Existe(int id)
        {
            Contexto contexto = new Contexto();
            bool ok = false;

            try
            {
                ok = contexto.Amigos.Any(a => a.AmigoId == id);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                contexto.Dispose();
            }

            return ok;
        }

        //Duplicados.
        public static bool ExisteEmail(string email)
        {
            Contexto contexto = new Contexto();
            bool ok;

            try
            {
                ok = contexto.Amigos.Any(a => a.Email.Equals(email));
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                contexto.Dispose();
            }

            return ok;
        }

        public static bool ExisteCelular(string celular)
        {
            Contexto contexto = new Contexto();
            bool ok;

            try
            {
                ok = contexto.Amigos.Any(a => a.Celular.Equals(celular));
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                contexto.Dispose();
            }

            return ok;
        }

        //Metodo Insertar.
        private static bool Insertar(Amigos amigo)
        {
            Contexto contexto = new Contexto();
            bool ok = false;

            try
            {
                contexto.Amigos.Add(amigo);
                ok = contexto.SaveChanges() > 0;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                contexto.Dispose();
            }

            return ok;
        }

        //Metodo Guardar.
        public static bool Guardar(Amigos amigos)
        {
            if (!Existe(amigos.AmigoId))
                return Insertar(amigos);
            else
                return Modificar(amigos);
        }

        //Metodo Buscar.
        public static Amigos Buscar(int id)
        {
            Amigos amigos = new Amigos();
            Contexto contexto = new Contexto();

            try
            {
                amigos = contexto.Amigos.Find(id);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }
            return amigos;
        }

        //Metodo Modificar.
        private static bool Modificar(Amigos amigos)
        {
            bool paso = false;
            Contexto contexto = new Contexto();

            try
            {
                contexto.Entry(amigos).State = EntityState.Modified;
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
                var amigos = AmigosBLL.Buscar(id);

                if (amigos != null)
                {
                    contexto.Amigos.Remove(amigos);
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

        //Metodo
        public static List<Amigos> GetAmigos()
        {
            Contexto contexto = new Contexto();
            List<Amigos> lista = new List<Amigos>();

            try
            {
                lista = contexto.Amigos.ToList();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                contexto.Dispose();
            }

            return lista;
        }


        //Metodo GetList.
        public static List<Amigos> GetList(Expression<Func<Amigos, bool>> criterio)
        {
            List<Amigos> Lista = new List<Amigos>();
            Contexto contexto = new Contexto();

            try
            {
                //obtener la lista y filtrarla según el criterio recibido por parametro.
                Lista = contexto.Amigos.Where(criterio).ToList();
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
    }
}
