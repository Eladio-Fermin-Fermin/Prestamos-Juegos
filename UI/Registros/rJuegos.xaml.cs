using PrestamosJuegos.BLL;
using PrestamosJuegos.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PrestamosJuegos.UI.Registros
{
    /// <summary>
    /// Interaction logic for rJuegos.xaml
    /// </summary>
    public partial class rJuegos : Window
    {
        private Juegos juegos = new Juegos();
        public rJuegos()
        {
            InitializeComponent();
            this.DataContext = juegos;
        }

        public void Limpiar()
        {
            juegos = new Juegos();
            this.DataContext = juegos;
        }

        //Metodo validar
        public bool esValido()
        {
            if (JuegoIdTextBox.Text.Length == 0 || DescripcionTextBox.Text.Length == 0 || PrecioTextBox.Text.Length == 0)
            {
                MessageBox.Show("Existen Campos vacíos.", "Campos vacíos",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            var juego = JuegosBLL.ValidaExistenciaJuego(DescripcionTextBox.Text);
            if (juego != null)
            {
                if ((DescripcionTextBox.Text == juego.Descripcion) && (int.Parse(JuegoIdTextBox.Text) != juego.JuegoId))
                {
                    MessageBox.Show($"Hay conflictos con registros existentes en la base de datos.","Ya Existente.", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }

            if (!Regex.IsMatch(PrecioTextBox.Text, @"^[0-9]{1,8}$|^[0-9]{1,8}\.[0-9]{1,8}$"))
            {
                MessageBox.Show("Solo se permiten numeros.", "Advertencia."
                  , MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        //private
        //

        private void BuscarButton_Click(object sender, RoutedEventArgs e)
        {

            var encontrado = JuegosBLL.Buscar(int.Parse(JuegoIdTextBox.Text));
            if (encontrado != null)
            {
                juegos = encontrado;
                this.DataContext = juegos;
            }
            else
            {
                MessageBox.Show("No se encontro el juego.", "No se encontro.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NuevoButton_Click(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private void GuardarButton_Click(object sender, RoutedEventArgs e)
        {

            if (!esValido())
                return;

            if (JuegosBLL.Guardar(juegos))
            {
                Limpiar();
                MessageBox.Show("Transacción exitosa!.", "Exito.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Transacción Fallida.", "Error.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EliminarButton_Click(object sender, RoutedEventArgs e)
        {

            if (JuegosBLL.Eliminar(int.Parse(JuegoIdTextBox.Text)))
            {
                Limpiar();
                MessageBox.Show("Se Elimino el Registro.", "Exito.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("No fue posible eliminar el registro.", "Error.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
