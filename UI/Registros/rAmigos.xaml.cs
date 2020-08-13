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
            //Valida el Id del amigo
            if (!Regex.IsMatch(AmigoIdTextBox.Text, "^[1-9]+$"))
            {
                MessageBox.Show("Asegúrese de haber ingresado un Id de caracter numerico y que sea mayor a 0.",
                    "Id no valido", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            //válida que no hayan campos vacíos.
            if (NombresTextBox.Text.Length == 0 || EmailTextBox.Text.Length == 0 || CelularTextBox.Text.Length == 0 ||
                DireccionTextBox.Text.Length == 0)
            {
                MessageBox.Show("Asegúrese de haber llenado todos los campos.", "Campos vacíos",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            //válida que se haya introducido un nombre válido
            if (!Regex.IsMatch(NombresTextBox.Text, "^[a-zA-Z'.\\s]{1,40}$"))
            {
                MessageBox.Show("Solo se admiten carácteres alfabeticos.", "Nombre no válido.",
                   MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            //válida la edad del amigo
            DateTime fechaActual = DateTime.Now;
            DateTime fechaNacimiento = FechaNacimientoDatePicker.SelectedDate.Value;
            TimeSpan ts = fechaActual - fechaNacimiento;
            int edad = (int)ts.TotalDays;
            if (edad < 2555/*edad en dias*/ || edad == 0)
            {
                MessageBox.Show("La persona a la que intentas registrar es muy jóven.", $"Esta persona tine {edad / 365} años.",
                      MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            else if (edad > 47450)
            {
                MessageBox.Show("La persona a la que intentas registras tiene unas muy alta probabilades de haber fallecido.", $"Esta persona tine {edad / 365} años.",
                      MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            //válida la dirreccion de correo electrónico.
            if (!Regex.IsMatch(EmailTextBox.Text, "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*"))
            {
                MessageBox.Show("La direccón de correo electrónico que ha introducido no es válida.", "Campo Email.",
                   MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            //válida el telefono.

            if (TelefonoTextBox.Text.Length != 0 && !Regex.IsMatch(TelefonoTextBox.Text, @"\d{3}\-\d{3}\-\d{4}"))
            {
                MessageBox.Show("Asegúrese de haber cumplido con el siguiente formato: 111-111-1111.", "Número Teléfono no válido.",
                  MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            //válida el celular.
            if (!Regex.IsMatch(CelularTextBox.Text, @"\d{3}\-\d{3}\-\d{4}"))
            {
                MessageBox.Show("Asegúrese de haber cumplido con el siguiente formato: 111-111-1111.", "Número celular no válido.",
                  MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            //Ayudara con la válidacion del campo telefono, email, celular.
            var amigo = AmigosBLL.Buscar(int.Parse(AmigoIdTextBox.Text));

            //válidando que no se repita el mismo telefono en diferentes registros.
            if (amigo != null)
            {
                if (AmigosBLL.ExisteTelefono(TelefonoTextBox.Text) && amigo.Nombres != NombresTextBox.Text)
                {
                    MessageBox.Show("Asegúrese que haya ingresado correctamente el número de teléfono.", $"El teléfono \"{TelefonoTextBox.Text}\" ya existe.",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }
            }
            else if (AmigosBLL.ExisteTelefono(TelefonoTextBox.Text))
            {
                MessageBox.Show("Asegúrese que haya ingresado correctamente el número de teléfono.", $"El teléfono \"{TelefonoTextBox.Text}\" ya existe.",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            //válidando que no se repita el mismo celular en diferentes registros.
            if (amigo != null)
            {
                if (AmigosBLL.ExisteCelular(CelularTextBox.Text) && amigo.Nombres != NombresTextBox.Text)
                {
                    MessageBox.Show("Asegúrese que haya ingresado correctamente el número celular.", $"El celular \"{CelularTextBox.Text}\" ya existe.",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }
            }
            else if (AmigosBLL.ExisteCelular(CelularTextBox.Text))
            {
                MessageBox.Show("Asegúrese que haya ingresado correctamente el número celular.", $"El celular \"{CelularTextBox.Text}\" ya existe.",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            //válidando que no se repita el mismo Email en diferentes registros.
            if (amigo != null)
            {
                if (AmigosBLL.ExisteEmail(EmailTextBox.Text) && amigo.Nombres != NombresTextBox.Text)
                {
                    MessageBox.Show("Asegúrese que haya ingresado correctamente el correo electrónico.", $"El Email \"{EmailTextBox.Text}\" ya existe.",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }
            }
            else if (AmigosBLL.ExisteEmail(EmailTextBox.Text))
            {
                MessageBox.Show("Asegúrese que haya ingresado correctamente el correo electrónico.", $"El Email \"{EmailTextBox.Text}\" ya existe.",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }


            return true;
        }

        private void BuscarButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Regex.IsMatch(AmigoIdTextBox.Text, "^[1-9]+$"))
            {
                MessageBox.Show("Asegúrese de haber ingresado un Id de caracter numerico y que sea mayor a 0.",
                    "Id no valido", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var encontrado = AmigosBLL.Buscar(int.Parse(AmigoIdTextBox.Text));
            if (encontrado != null)
            {
                amigos = encontrado;
                this.DataContext = amigos;
            }
            else
            {
                MessageBox.Show("Ese amigo no existe en la base de datos.", "No se encontro.", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("Guardado.", "Exito.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Algo salio mal.", "Error.", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void EliminarButton_Click(object sender, RoutedEventArgs e)
        {

            if (!Regex.IsMatch(AmigoIdTextBox.Text, "^[1-9]+$"))
            {
                MessageBox.Show("Asegúrese de haber ingresado un Id de caracter numerico y que sea mayor a 0.",
                    "Id no valido", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (AmigosBLL.Eliminar(int.Parse(AmigoIdTextBox.Text)))
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
