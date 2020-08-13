using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrestamosJuegos.Entidades
{
    public class Amigos
    {
        [Key]
        public int AmigoId { get; set; }
        public string Nombres { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public DateTime FechaNacimiento {get; set; }
    }
}
