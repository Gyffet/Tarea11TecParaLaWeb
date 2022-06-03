using System;
using System.Collections.Generic;

namespace Formulario.Model
{
    public partial class CedFumFechas
    {
        public int pkFecha { get; set; }
        public string fkCedula { get; set; }
        public DateTime? fdFechaProgramada { get; set; }
        public DateTime? fdFechaRealizada { get; set; }
        public string fcComentarios { get; set; }
    }
}
