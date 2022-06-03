using System;
using System.Collections.Generic;

namespace Formulario.Model
{
    public partial class ContratoCedula
    {
        public int pkContrato { get; set; }
        public int fkServicio { get; set; }
        public string fcDescrpcion { get; set; }
        public string fcNoContrato { get; set; }
        public DateTime fdFechaInicio { get; set; }
        public DateTime fdFechaFin { get; set; }
        public bool fiActivo { get; set; }
    }
}
