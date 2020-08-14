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
    public class PrestamosBLL
    {
        //Metodo Existe.
        public static bool Existe(int id)
        {
            Contexto contexto = new Contexto();
            bool encontrado = false;

            try
            {
                encontrado = contexto.Prestamos.Any(e => e.PrestamoId == id);
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
        private static bool Insertar(Prestamos prestamos)
        {
            bool paso = false;
            Contexto contexto = new Contexto();

            try
            {
                foreach(var item in prestamos.PrestamosDetalles)
                {
                    item.Juego.Existencia -= item.Cantidad;
                    contexto.Entry(item.Juego).State = EntityState.Modified;
                }

                contexto.Prestamos.Add(prestamos);
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
        public static bool Guardar(Prestamos prestamos)
        {
            if (!Existe(prestamos.PrestamoId))
                return Insertar(prestamos);
            else
                return Modificar(prestamos);
        }

        //Metodo Buscar.
        public static Prestamos Buscar(int id)
        {
            Contexto contexto = new Contexto();
            Prestamos prestamo;

            try
            {
                prestamo = contexto.Prestamos.Where(p => p.PrestamoId == id).Include(p => p.PrestamosDetalles)
                    .ThenInclude(d => d.Juego).SingleOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                contexto.Dispose();
            }

            return prestamo;
        }

        //Metodo Modificar.
        private static bool Modificar(Prestamos prestamo)
        {
            Contexto contexto = new Contexto();
            bool ok = false;
            try
            {
                contexto.Database.ExecuteSqlRaw($"Delete FROM PrestamoDetalle Where PrestamoId={prestamo.PrestamoId}");
                foreach (var item in prestamo.PrestamosDetalles)
                {
                    contexto.Entry(item).State = EntityState.Added;
                }
                contexto.Entry(prestamo).State = EntityState.Modified;
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


        //Metodo Eliminar.
        public static bool Eliminar(int id)
        {
            bool paso = false;
            Contexto contexto = new Contexto();

            try
            {
                var prestamos = PrestamosBLL.Buscar(id);

                if (prestamos != null)
                {
                    contexto.Prestamos.Remove(prestamos);
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
        public static List<Prestamos> GetList(Expression<Func<Prestamos, bool>> criterio)
        {
            List<Prestamos> Lista = new List<Prestamos>();
            Contexto contexto = new Contexto();

            try
            {
                Lista = contexto.Prestamos.Where(criterio).ToList();
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
