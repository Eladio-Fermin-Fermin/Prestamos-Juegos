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
        public rEntradas()
        {
            InitializeComponent();
            this.DataContext = entradas;
            var Lista = JuegosBLL.GetList(x => true);
            this.JuegoIdComboBox.ItemsSource = Lista;
            this.JuegoIdComboBox.SelectedValuePath = "JuegoId";
            this.JuegoIdComboBox.DisplayMemberPath = "Descripcion";
            if (Lista.Count > 0)
                this.JuegoIdComboBox.SelectedIndex = 0;
        }

        public void Limpiar()
        {
            entradas = new Entradas();
            this.DataContext = entradas;
        }

        /*public bool Validar()
        {
            
        }*/

        private void BuscarButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Regex.IsMatch(EntradaIdTextBox.Text, "^[1-9]+$"))
            {
                MessageBox.Show("El Id no es valido.",
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
                MessageBox.Show("No Existe en la base de datos.", "No se encontro.", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("El Id no es valido.",
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
