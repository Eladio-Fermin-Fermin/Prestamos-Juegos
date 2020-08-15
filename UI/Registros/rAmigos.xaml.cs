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
    /// Interaction logic for rAmigos.xaml
    /// </summary>
    public partial class rAmigos : Window
    {
        Amigos amigos = new Amigos();
        public rAmigos()
        {
            InitializeComponent();
            this.DataContext = amigos;
        }

        private void Limpiar()
        {
            this.amigos = new Amigos();
            this.DataContext = amigos;
        }

        public bool Validar()
        {

            if (AmigoIdTextBox.Text.Length == 0 || NombresTextBox.Text.Length == 0 || EmailTextBox.Text.Length == 0 || CelularTextBox.Text.Length == 0 ||
                DireccionTextBox.Text.Length == 0)
            {
                MessageBox.Show("Existen Campos vacíos..", "Campos vacíos",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            /*if (!Regex.IsMatch(CelularTextBox.Text, (@"^([0-3]{-}[0-3]{-}[0-4]$")))
            {
                MessageBox.Show("No tiene el Formato: 999-999-9999.", "Formato no válido.",
                  MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }*/

            if (!Regex.IsMatch(EmailTextBox.Text, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
            {
                MessageBox.Show("Este correo electrónico no es válida.", "Campo Email.",
                   MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            //Metodo Para los Duplicado.
            var amigo = AmigosBLL.Buscar(int.Parse(AmigoIdTextBox.Text));
            if (amigo != null)
            {
                if (AmigosBLL.ExisteCelular(CelularTextBox.Text) && amigo.Nombres != NombresTextBox.Text)
                {
                    MessageBox.Show("Este Celular no esta disponible.", "Ya Existe.",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }
            }

            if (amigo != null)
            {
                if (AmigosBLL.ExisteEmail(EmailTextBox.Text) && amigo.Nombres != NombresTextBox.Text)
                {
                    MessageBox.Show("Este Correo electrónico no esta disponible.","Ya Existe.",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }
            }
 
            return true;
        }

        public bool ValidarBuscar()
        {
            bool ValidarB = true;
            if (AmigoIdTextBox.Text.Length == 0)
            {
                ValidarB = false;
                GuardarButton.IsEnabled = false;
                MessageBox.Show("AmigoId está vacio", "Fallo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                AmigoIdTextBox.Focus();
                GuardarButton.IsEnabled = true;
            }
            return ValidarB;
        }

        private void BuscarButton_Click(object sender, RoutedEventArgs e)
        {
            ValidarBuscar();
            Amigos amigo = AmigosBLL.Buscar(int.Parse(AmigoIdTextBox.Text));
            if (amigo != null)
            {
                amigos = amigo;
                this.DataContext = amigos;
            }
            else
            {
                MessageBox.Show("El Amigo No existe.", "No se encontro.", MessageBoxButton.OK, MessageBoxImage.Error);
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

            if (AmigosBLL.Guardar(amigos))
            {
                Limpiar();
                MessageBox.Show("Transacción exitosa!", "Exito.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Transacción Fallida", "Error.", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void EliminarButton_Click(object sender, RoutedEventArgs e)
        {

            if (AmigosBLL.Eliminar(int.Parse(AmigoIdTextBox.Text)))
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
