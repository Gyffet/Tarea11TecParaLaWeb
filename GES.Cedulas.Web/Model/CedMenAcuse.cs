using System;
using System.Collections.Generic;

namespace Formulario.Model
{
    public partial class CedMenAcuse
    {
        public int pkAcuse { get; set; }
        public string fkCedula { get; set; }
        public DateTime? fdFechaProgramada { get; set; }
        public DateTime? fdFechaEfectiva { get; set; }
        public int? fiCantidadAcuses { get; set; }
        public decimal? fiPena { get; set; }
    }
}
