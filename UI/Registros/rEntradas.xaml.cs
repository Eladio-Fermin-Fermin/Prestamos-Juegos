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
    /// Interaction logic for rEntradas.xaml
    /// </summary>
    public partial class rEntradas : Window
    {
        private Entradas entradas = new Entradas();
        private rJuegos juegos = new rJuegos();
        public rEntradas()
        {
            InitializeComponent();
            this.DataContext = entradas;
            this.DataContext = juegos;
        }

        public void Limpiar()
        {
            entradas = new Entradas();
            this.DataContext = entradas;
        }

        public bool Validar()
        {
            if (!Regex.IsMatch(EntradaIdTextBox.Text, "^[1-9]+$"))
            {
                MessageBox.Show("Asegúrese de haber ingresado un Id de caracter numerico y que sea mayor a 0.",
                    "Id no valido", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            //válida que no hayan campos vacíos.
            if (JuegoIdTextBox.Text.Length == 0 || CantidadTextBox.Text.Length == 0)
            {
                MessageBox.Show("Asegúrese de haber llenado todos los campos.", "Campos vacíos",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            if (!Regex.IsMatch(JuegoIdTextBox.Text, "^[1-9]+$"))
            {
                MessageBox.Show("Asegúrese de haber ingresado un Id de caracter numerico y que sea mayor a 0.",
                    "Id no valido", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!JuegosBLL.Existe(int.Parse(JuegoIdTextBox.Text)))
            {
                MessageBox.Show("Este juego no se encontro en la base de datos. Recuerde crear el juego antes de darle una entrada.",
                    "No existe.", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void BuscarButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Regex.IsMatch(EntradaIdTextBox.Text, "^[1-9]+$"))
            {
                MessageBox.Show("Asegúrese de haber ingresado un Id de caracter numerico y que sea mayor a 0.",
                    "Id no valido", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var encontrado = EntradasBLL.Buscar(int.Parse(EntradaIdTextBox.Text));
            if (encontrado != null)
            {
                entradas = encontrado;
                this.DataContext = entradas;
            }
            else
            {
                MessageBox.Show("Esa entrada no existe en la base de datos.", "No se encontro.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NuevoButton_Click(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private void GuardarButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Validar())
                return;

            if (EntradasBLL.Guardar(entradas))
            {
                CantidadTextBox.Text = juegos.ExistenciaTextBox.Text;
                Limpiar();
                MessageBox.Show("Guardado.", "Exito.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Algo salio mal.", "Error.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EliminarButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Regex.IsMatch(EntradaIdTextBox.Text, "^[1-9]+$"))
            {
                MessageBox.Show("Asegúrese de haber ingresado un Id de caracter numerico y que sea mayor a 0.",
                    "Id no valido", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (EntradasBLL.Eliminar(int.Parse(EntradaIdTextBox.Text)))
            {
                Limpiar();
                MessageBox.Show("Eliminado.", "Exito.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Algo salio mal.", "Error.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
