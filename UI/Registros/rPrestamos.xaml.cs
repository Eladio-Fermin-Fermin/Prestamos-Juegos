using PrestamosJuegos.BLL;
using PrestamosJuegos.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
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
            var Lista = JuegosBLL.GetList(x => true);
            this.JuegoIdComboBox.ItemsSource = Lista;
            this.JuegoIdComboBox.SelectedValuePath = "JuegoId";
            this.JuegoIdComboBox.DisplayMemberPath = "Descripcion";
            if (Lista.Count > 0)
                this.JuegoIdComboBox.SelectedIndex = 0;
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

        private void BucarButton_Click(object sender, RoutedEventArgs e)
        {
            Prestamos encontrado = PrestamosBLL.Buscar(prestamos.PrestamoId);

            if (encontrado != null)
            {
                prestamos = encontrado;
                Cargar();
            }
            else
            {
                Limpiar();
                MessageBox.Show("Tarea no existe en la base de datos", "Fallo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AgregarButton_Click(object sender, RoutedEventArgs e)
        {
            var Detalle = new PrestamosDetalle
            {
                PrestamoId = this.prestamos.PrestamoId, 
                JuegoId = Convert.ToInt32(JuegoIdComboBox.SelectedValue.ToString()),
                Cantidad = Convert.ToInt32(CantidadTextBox.Text)
            };

            //this.
            Cargar();

            JuegoIdComboBox.SelectedIndex = -1;
            CantidadTextBox.Clear();

        }

        private void RemoverButton_Click(object sender, RoutedEventArgs e)
        {
            if (DetalleDataGrid.Items.Count >= 1 && DetalleDataGrid.SelectedIndex <= DetalleDataGrid.Items.Count - 1)
            {
                prestamos.Detalles.RemoveAt(DetalleDataGrid.SelectedIndex);
                Cargar();
            }
        }

        private void NuevoButton_Click(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private void GuardarButton_Click(object sender, RoutedEventArgs e)
        {
            bool paso = false;

            if (prestamos.PrestamoId == 0)
            {
                paso = PrestamosBLL.Guardar(prestamos);
            }
            else
            {
                if (ExisteEnLaBaseDeDatos())
                {
                    paso = PrestamosBLL.Guardar(prestamos);
                }
                else
                {
                    MessageBox.Show("No existe en la base de datos", "ERROR");
                }
            }

            if (paso)
            {
                Limpiar();
                MessageBox.Show("Guardado!", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("Fallo al guardar", "Fallo", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void EliminarButton_Click(object sender, RoutedEventArgs e)
        {
            Prestamos existe = PrestamosBLL.Buscar(prestamos.PrestamoId);

            if (existe == null)
            {
                MessageBox.Show("No existe la tarea en la base de datos", "Fallo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                PrestamosBLL.Eliminar(prestamos.PrestamoId);
                MessageBox.Show("Eliminado", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
                Limpiar();
            }
        }
    }
}
