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
    /// Interaction logic for rPrestamos.xaml
    /// </summary>
    public partial class rPrestamos : Window
    {
        private Prestamos prestamos = new Prestamos();

        public rPrestamos()
        {
            InitializeComponent();
            this.DataContext = prestamos;
            JuegoIdComboBox.ItemsSource = JuegosBLL.GetJuegos();
            JuegoIdComboBox.SelectedValuePath = "JuegoId";
            JuegoIdComboBox.DisplayMemberPath = "Descripcion";

            AmigoIdComboBox.ItemsSource = AmigosBLL.GetAmigos();
            AmigoIdComboBox.SelectedValuePath = "AmigoId";
            AmigoIdComboBox.DisplayMemberPath = "Nombres";
        }
        
        private void Cargar()
        {
            this.DataContext = null;
            this.DataContext = prestamos;
        }

        private void Limpiar()
        {
            this.prestamos = new Prestamos();
            this.DataContext = prestamos;
        }

        private bool ExisteEnLaBaseDeDatos()
        {
            Prestamos esValido = PrestamosBLL.Buscar(prestamos.PrestamoId);

            return (esValido != null);
        }

        private void ObservacionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            /*if (ObservacionTextBox.LineCount > previousLineCount)
            {
                previousLineCount = ObservacionTextBox.LineCount;
            }*/
        }

        public bool ValidarAgregar()
        {
            //Valida que se haya seleccionado un juego
            if (JuegoIdComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Asegúrese de haber seleccionado un juego.",
                   "Campo Juego", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            //Valida que se introduzaca una cantidad valida.
            if (!Regex.IsMatch(CantidadTextBox.Text, "^[1-9]+${1,9}"))
            {
                MessageBox.Show("Asegúrese de haber ingresado cantidad valida.",
                    "Cantidad no valido", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            //Valida que la cantidad no sea mayor que la cantidad existente
            if (JuegosBLL.Existencia(int.Parse(JuegoIdComboBox.SelectedValue.ToString())) < int.Parse(CantidadTextBox.Text))
            {
                MessageBox.Show($"En el inventario solo quedan {JuegosBLL.Existencia(int.Parse(JuegoIdComboBox.SelectedValue.ToString()))} " +
                    $"unidades disponibles.",
                    "Cantidad insuficiente.", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        public bool Validar()
        {
            //Valida el Id
            if (!Regex.IsMatch(PrestamoIdTextBox.Text, "^[1-9]+$"))
            {
                MessageBox.Show("Asegúrese de haber ingresado un Id de caracter numerico y que sea mayor a 0.",
                    "Id no valido", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            //Valida que se seleccione un amigo
            if (AmigoIdComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Asegúrese de haber seleccionado un amigo.",
                   "Campo Amigo", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void BucarButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Regex.IsMatch(PrestamoIdTextBox.Text, "^[1-9]+$"))
            {
                MessageBox.Show("Asegúrese de haber ingresado un Id de caracter numerico y que sea mayor a 0.",
                    "Id no valido", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var encontrado = PrestamosBLL.Buscar(int.Parse(PrestamoIdTextBox.Text));
            if (encontrado != null)
            {
                prestamos = encontrado;
                this.DataContext = prestamos;
            }
            else
            {
                MessageBox.Show("Ese prestamo no existe en la base de datos.", "No se encontro.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AgregarButton_Click(object sender, RoutedEventArgs e)
        {

            if (!ValidarAgregar())
                return;

            var detalle = new PrestamosDetalle
            {
                Id = 0,
                PrestamoId = int.Parse(PrestamoIdTextBox.Text),
                JuegoId = int.Parse(JuegoIdComboBox.SelectedValue.ToString()),
                Cantidad = int.Parse(CantidadTextBox.Text)
            };

            detalle.Juego = (Juegos)JuegoIdComboBox.SelectedItem;

            prestamos.PrestamosDetalles.Add(detalle);

            prestamos.CantidadJuegos += int.Parse(CantidadTextBox.Text);

            Cargar();
            CantidadTextBox.Clear();

        }

        private void RemoverButton_Click(object sender, RoutedEventArgs e)
        {
            var detalle = (PrestamosDetalle)DetalleDataGrid.SelectedItem;
            prestamos.CantidadJuegos -= detalle.Cantidad;
            prestamos.PrestamosDetalles.RemoveAt(DetalleDataGrid.SelectedIndex);
            Cargar();
        }

        private void NuevoButton_Click(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private void GuardarButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Validar())
                return;

            if (PrestamosBLL.Guardar(prestamos))
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
            if (!Regex.IsMatch(PrestamoIdTextBox.Text, "^[1-9]+$"))
            {
                MessageBox.Show("Asegúrese de haber ingresado un Id de caracter numerico y que sea mayor a 0.",
                    "Id no valido", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (PrestamosBLL.Eliminar(int.Parse(PrestamoIdTextBox.Text)))
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
