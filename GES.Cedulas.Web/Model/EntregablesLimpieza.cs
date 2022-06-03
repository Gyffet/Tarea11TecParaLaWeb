using System;
using System.Collections.Generic;

namespace Formulario.Model
{
    public partial class EntregablesLimpieza
    {
        public int ID { get; set; }
        public string fkCedula { get; set; }
        public DateTime? fdProgramaOperacion { get; set; }
        public DateTime? fdVisitaInstalaciones { get; set; }
        public DateTime? fdGafeteUniforme { get; set; }
        public DateTime? fdActaInicio { get; set; }
        public DateTime? fdActaCierre { get; set; }
        public DateTime? fdCierreMes { get; set; }
        public DateTime? fdActaServicios { get; set; }
        public DateTime? fdSUAIMSS { get; set; }
    }
}
