using System;
using System.Collections.Generic;

namespace Formulario.Model
{
    public partial class CedFumFumigacion
    {
        public int pkFumigacion { get; set; }
        public string fkCedula { get; set; }
        public DateTime? fdFechaFumigacion { get; set; }
        public DateTime? fdFechaReaparacion { get; set; }
        public string fcComentarios { get; set; }
    }
}
