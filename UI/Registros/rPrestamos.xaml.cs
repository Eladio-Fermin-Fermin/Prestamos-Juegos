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

        public bool ValidandoAgregar()
        {
            if (JuegoIdComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("No a seleccionado un juego por favor seleccione un juego.",
                   "Campo Juego", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        public bool Validar()
        {
            bool esValido = true;
            if (PrestamoIdTextBox.Text.Length == 0)
            {
                esValido = false;
                GuardarButton.IsEnabled = false;
                MessageBox.Show("PrestamoId está vacio", "Fallo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                PrestamoIdTextBox.Focus();
                GuardarButton.IsEnabled = true;
            }

            return esValido;
            //return true;
        }

        private void BucarButton_Click(object sender, RoutedEventArgs e)
        {
        
            var encontrado = PrestamosBLL.Buscar(int.Parse(PrestamoIdTextBox.Text));
            if (encontrado != null)
            {
                prestamos = encontrado;
                this.DataContext = prestamos;
            }
            else
            {
                MessageBox.Show("El prestamo no existe.", "No se encontro.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AgregarButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidandoAgregar())
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
            Cargar();
            CantidadTextBox.Clear();

        }

        private void RemoverButton_Click(object sender, RoutedEventArgs e)
        {
            var detalle = (PrestamosDetalle)DetalleDataGrid.SelectedItem;
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
                MessageBox.Show("Transacción exitosa!.", "Exito.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Transacción Fallida..", "Error.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EliminarButton_Click(object sender, RoutedEventArgs e)
        {
           
            if (PrestamosBLL.Eliminar(int.Parse(PrestamoIdTextBox.Text)))
            {
                Limpiar();
                MessageBox.Show("Se a Eliminado el Registro.", "Exito.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("No se pudo Emilinar el Registro Algo salio mal.", "Error.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
