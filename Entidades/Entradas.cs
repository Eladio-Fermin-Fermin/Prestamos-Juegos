﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PrestamosJuegos.Entidades
{
    public class Entradas
    {
       [Key]
       public int EntradaId { get; set; }
       public DateTime FechaEntrada { get; set; } = DateTime.Now;
       public int Cantidad { get; set; }
       public int JuegoId { get; set; }
       //[ForeignKey("JuegoId")]
    }
}
