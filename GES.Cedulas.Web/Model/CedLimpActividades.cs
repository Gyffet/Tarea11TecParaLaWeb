using System;
using System.Collections.Generic;

namespace Formulario.Model
{
    public partial class CedLimpActividades
    {
        public int pkActividad { get; set; }
        public string fkCedula { get; set; }
        public DateTime? fdFechaInci { get; set; }
        public string fcTipo { get; set; }
        public string fcArea { get; set; }
        public string fcComentarios { get; set; }
    }
}
