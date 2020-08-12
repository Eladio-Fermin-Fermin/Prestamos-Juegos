using PrestamosJuegos.UI.Consultas;
using PrestamosJuegos.UI.Registros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PrestamosJuegos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AmigosMenuItem_Click(object sender, RoutedEventArgs e)
        {
            rAmigos Ramigos = new rAmigos();
            Ramigos.Show();
        }

        private void JuegosMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PrestamosMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AmigoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            cAmigos amigos = new cAmigos();
            amigos.Show();
        }

        private void JuegoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            cJuegos juegos = new cJuegos();
            juegos.Show();
        }

        private void PrestamoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            cPrestamos prestamos = new cPrestamos();
            prestamos.Show();
        }
    }
}
