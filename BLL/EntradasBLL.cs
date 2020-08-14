using Microsoft.EntityFrameworkCore;
using PrestamosJuegos.DAL;
using PrestamosJuegos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows;

namespace PrestamosJuegos.BLL
{
    public class EntradasBLL
    {
        //Metodo Existe.
        public static bool Existe(int id)
        {
            Contexto contexto = new Contexto();
            bool encontrado = false;

            try
            {
                encontrado = contexto.Entradas.Any(e => e.EntradaId == id);
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
        private static bool Insertar(Entradas entradas)
        {
            bool paso = false;
            Contexto contexto = new Contexto();

            try
            {
                var Juego = JuegosBLL.Buscar(entradas.JuegoId);

                Juego.Existencia += entradas.Cantidad;
                if (JuegosBLL.Guardar(Juego))
                {
                    contexto.Entradas.Add(entradas);
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

        //Metodo Guardar.
        public static bool Guardar(Entradas entradas)
        {

            if (!Existe(entradas.EntradaId))
                return Insertar(entradas);
            else
                return Modificar(entradas);
        }

        //Metodo Buscar.
        public static Entradas Buscar(int id)
        {
            Entradas entradas = new Entradas();
            Contexto contexto = new Contexto();

            try
            {
                entradas = contexto.Entradas.Find(id);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }
            return entradas;
        }

        //Metodo Modificar.
        private static bool Modificar(Entradas entradas)
        {
            bool paso = false;
            Contexto contexto = new Contexto();

            try
            {
                contexto.Entry(entradas).State = EntityState.Modified;
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
                var entradas = EntradasBLL.Buscar(id);

                if (entradas != null)
                {
                    contexto.Entradas.Remove(entradas); //remover la entidad
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
        public static List<Entradas> GetList(Expression<Func<Entradas, bool>> criterio)
        {
            List<Entradas> Lista = new List<Entradas>();
            Contexto contexto = new Contexto();

            try
            {
                //obtener la lista y filtrarla según el criterio recibido por parametro.
                Lista = contexto.Entradas.Where(criterio).ToList();
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

        public static void IncrementaInventario(Entradas entrada)
        {
            Juegos juego = JuegosBLL.Buscar(entrada.JuegoId);
            juego.Existencia += entrada.Cantidad;
            JuegosBLL.Guardar(juego);
        }

        public static bool ModificaInventario(Entradas NuevaEntrada)
        {
            Entradas entrada = Buscar(NuevaEntrada.EntradaId);//Se buscala entrada anterior
            Juegos juego = JuegosBLL.Buscar(NuevaEntrada.JuegoId);//Se busca el juego a modificar

            juego.Existencia -= entrada.Cantidad;//Se le resta la cantidad de la entrada anterior.
            juego.Existencia += NuevaEntrada.Cantidad;//Se le suma la nueva cantidad.

            //Se puede dar el caso de que se preste una una cantidad X de juegos y se quiera modificar 
            //la entrada por una cantidad menor a la que se presto y el inventario quede en - 
            if (juego.Existencia < 0)
            {
                MessageBox.Show("No puedes realizar este cambio porque al parecer prestaste una cantidad mayor de la que ahora quieres " +
                    "ingresar.",
                    "Ha ocurrido un conflicto.", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            JuegosBLL.Guardar(juego);
            return true;
        }
    }
}
