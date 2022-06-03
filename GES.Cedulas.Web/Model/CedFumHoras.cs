using System;
using System.Collections.Generic;

namespace Formulario.Model
{
    public partial class CedFumHoras
    {
        public int pkHoras { get; set; }
        public string fkCedula { get; set; }
        public DateTime? fdFechaProgramada { get; set; }
        public DateTime? fdHoraProgramada { get; set; }
        public DateTime? fdHoraEfectiva { get; set; }
        public string fcComentarios { get; set; }
    }
}
