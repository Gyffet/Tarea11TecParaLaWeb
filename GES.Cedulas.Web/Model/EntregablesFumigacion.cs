using System;
using System.Collections.Generic;

namespace Formulario.Model
{
    public partial class EntregablesFumigacion
    {
        public int ID { get; set; }
        public string fkCedula { get; set; }
        public DateTime? fdCierreMes { get; set; }
        public DateTime? fdReporteServ { get; set; }
        public DateTime? fdListadePersonal { get; set; }
        public DateTime? fdSUAIMSS { get; set; }
        public DateTime? fdActaEntrega { get; set; }
        public DateTime? fdProgramaOperacion { get; set; }
        public DateTime? fdUniforme { get; set; }
        public DateTime? fdActaInicio { get; set; }
        public DateTime? fdPersonal { get; set; }
        public DateTime? fdActaCierre { get; set; }
    }
}
