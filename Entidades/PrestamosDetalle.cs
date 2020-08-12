using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrestamosJuegos.Entidades
{
    public class PrestamosDetalle
    {
        [Key]
        public int Id { get; set; }
        public int PrestamoId { get; set; }
        public int JuegoId { get; set; }
        public int Cantidad { get; set; }

        public PrestamosDetalle(int cantidad)
        {
            Id = 0;
            //PrestamoId = prestamoid;
            //JuegoId = juegoid;
            Cantidad = cantidad;
        }
    }
}
