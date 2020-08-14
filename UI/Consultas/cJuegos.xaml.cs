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

namespace PrestamosJuegos.UI.Consultas
{
    /// <summary>
    /// Interaction logic for cJuegos.xaml
    /// </summary>
    public partial class cJuegos : Window
    {
        public cJuegos()
        {
            InitializeComponent();
        }

        private void ConsultarButton_Click(object sender, RoutedEventArgs e)
        {
            var listado = new List<Juegos>();

            if (CriterioTextBox.Text.Trim().Length > 0)
            {
                switch (FiltroComboBox.SelectedIndex)
                {
                    case 0: //JuegoId
                        listado = JuegosBLL.GetList(j => j.JuegoId == Utilidades.ToInt(CriterioTextBox.Text));
                        break;

                    case 1: //Descripcion                       
                        listado = JuegosBLL.GetList(j => j.Descripcion.Contains(CriterioTextBox.Text, StringComparison.OrdinalIgnoreCase));
                        break;

                    case 2: //Precio                       
                        listado = JuegosBLL.GetList(j => j.Precio == Utilidades.ToInt(CriterioTextBox.Text));
                        break;

                    case 3: //Existencia                       
                        listado = JuegosBLL.GetList(j => j.Existencia == Utilidades.ToInt(CriterioTextBox.Text));
                        break;

                }
            }
            else
            {
                listado = JuegosBLL.GetList(c => true);
            }

            if (DesdeDataPicker.SelectedDate != null)
                listado = JuegosBLL.GetList(j => j.FechaCompra.Date >= DesdeDataPicker.SelectedDate);

            if (HastaDatePicker.SelectedDate != null)
                listado = JuegosBLL.GetList(j => j.FechaCompra.Date <= HastaDatePicker.SelectedDate);

            DatosDataGrid.ItemsSource = null;
            DatosDataGrid.ItemsSource = listado;
        }
    }
}
