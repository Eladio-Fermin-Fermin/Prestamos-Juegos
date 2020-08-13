﻿using PrestamosJuegos.BLL;
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
    /// Interaction logic for rJuegos.xaml
    /// </summary>
    public partial class rJuegos : Window
    {
        private Juegos juegos = new Juegos();
        public rJuegos()
        {
            InitializeComponent();
            this.DataContext = juegos;
        }

        public void Limpiar()
        {
            juegos = new Juegos();
            this.DataContext = juegos;
        }

        public bool Validar()
        {
            //Valida el Id
            if (!Regex.IsMatch(JuegoIdTextBox.Text, "^[1-9]+$"))
            {
                MessageBox.Show("Asegúrese de haber ingresado un Id de caracter numerico y que sea mayor a 0.",
                    "Id no valido", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            //válida que no hayan campos vacíos.
            if (DescripcionTextBox.Text.Length == 0 || PrecioTextBox.Text.Length == 0)
            {
                MessageBox.Show("Asegúrese de haber llenado todos los campos.", "Campos vacíos",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            //válida que haya un dato válido en el precio.
            if (!Regex.IsMatch(PrecioTextBox.Text, @"^[0-9]{1,8}$|^[0-9]{1,8}\.[0-9]{1,8}$"))
            {
                MessageBox.Show("Solo puede introducir carácteres numéricos.", "Campo Precio.",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            //Valida que no se creen varios registro del mismo juego
            var juego = JuegosBLL.DoplicadoJuego(DescripcionTextBox.Text);
            if (juego != null)
            {
                if ((DescripcionTextBox.Text == juego.Descripcion) && (int.Parse(JuegoIdTextBox.Text) != juego.JuegoId))
                {
                    MessageBox.Show($"Este juego ya existe con el Id: {juego.JuegoId}.", "Existente en el inventario.",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }

            return true;
        }


        private void BuscarButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Regex.IsMatch(JuegoIdTextBox.Text, "^[1-9]+$"))
            {
                MessageBox.Show("Asegúrese de haber ingresado un Id de caracter numerico y que sea mayor a 0.",
                    "Id no valido", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var encontrado = JuegosBLL.Buscar(int.Parse(JuegoIdTextBox.Text));
            if (encontrado != null)
            {
                juegos = encontrado;
                this.DataContext = juegos;
            }
            else
            {
                MessageBox.Show("Ese juego no existe en la base de datos.", "No se encontro.", MessageBoxButton.OK, MessageBoxImage.Error);
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

            if (JuegosBLL.Guardar(juegos))
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
            if (!Regex.IsMatch(JuegoIdTextBox.Text, "^[1-9]+$"))
            {
                MessageBox.Show("Asegúrese de haber ingresado un Id de caracter numerico y que sea mayor a 0.",
                    "Id no valido", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (JuegosBLL.Eliminar(int.Parse(JuegoIdTextBox.Text)))
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
