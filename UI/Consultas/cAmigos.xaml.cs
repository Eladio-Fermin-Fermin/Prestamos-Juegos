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
    /// Interaction logic for cAmigos.xaml
    /// </summary>
    public partial class cAmigos : Window
    {
        public cAmigos()
        {
            InitializeComponent();
        }

        private void ConsultarButton_Click(object sender, RoutedEventArgs e)
        {
            var listado = new List<Amigos>();

            if (CriterioTextBox.Text.Trim().Length > 0)
            {
                switch (FiltroComboBox.SelectedIndex)
                {
                    case 0: //AmigoId
                        listado = AmigosBLL.GetList(a => a.AmigoId == Utilidades.ToInt(CriterioTextBox.Text));
                        break;

                    case 1: //Nombres                       
                        listado = AmigosBLL.GetList(a => a.Nombres.Contains(CriterioTextBox.Text, StringComparison.OrdinalIgnoreCase));
                        break;

                    case 2: //Direccion                       
                        listado = AmigosBLL.GetList(a => a.Direccion.Contains(CriterioTextBox.Text, StringComparison.OrdinalIgnoreCase));
                        break;

                    case 3: //Telefono                       
                        listado = AmigosBLL.GetList(a => a.Telefono.Contains(CriterioTextBox.Text, StringComparison.OrdinalIgnoreCase));
                        break;

                    case 4: //Celular                       
                        listado = AmigosBLL.GetList(a => a.Celular.Contains(CriterioTextBox.Text, StringComparison.OrdinalIgnoreCase));
                        break;

                    case 5: //Email                       
                        listado = AmigosBLL.GetList(a => a.Email.Contains(CriterioTextBox.Text, StringComparison.OrdinalIgnoreCase));
                        break;

                    /*case 6: //FechaNacimiento                       
                        listado = AmigosBLL.GetList(a => a.FechaNacimiento.Contains(CriterioTextBox.Text, StringComparison.OrdinalIgnoreCase));
                        break;*/
                }
            }
            else
            {
                listado = AmigosBLL.GetList(c => true);
            }

            if (DesdeDataPicker.SelectedDate != null)
                listado = AmigosBLL.GetList(c => c.FechaNacimiento.Date >= DesdeDataPicker.SelectedDate);

            if (HastaDatePicker.SelectedDate != null)
                listado = AmigosBLL.GetList(c => c.FechaNacimiento.Date <= HastaDatePicker.SelectedDate);

            DatosDataGrid.ItemsSource = null;
            DatosDataGrid.ItemsSource = listado;
        }
    }
}
