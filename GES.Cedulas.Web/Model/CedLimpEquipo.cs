using System;
using System.Collections.Generic;

namespace Formulario.Model
{
    public partial class CedLimpEquipo
    {
        public int pkEquipo { get; set; }
        public string fkCedula { get; set; }
        public DateTime? fdFechaInci { get; set; }
        public string fcMaquina { get; set; }
        public string fcComentarios { get; set; }
    }
}
