using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Formulario.Model.ModelExt.Mensajeria
{
    public class Recoleccion
    {
        public DateTime fechaProgramada { get; set; }
        public DateTime fechaEfectiva { get; set; }
        public string noGuiaRec { get; set; }
        public string codRastreoRec { get; set; }
        public string tipoServRec { get; set; }
        public string fechaPrograFormat { get; set; }
        public string fehcaEfecFormat { get; set; }
    }
}
