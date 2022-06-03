using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Formulario.Model.ModelExt.Mensajeria
{
    public class CedulaMensajeria
    {
        public string factura { get; set; }
        public string monto{ get; set; }
        public string boolRecoleccion { get; set; }
        public List<Recoleccion> arregloRecoleccion { get; set; }
        public string boolEntrega { get; set; }
        public List<Entrega> arregloEntrega { get; set; }
        public string boolAcuses { get; set; }
        public List<Acuse> arregloAcuses { get; set; }
        public string boolMalEstado { get; set; }
        public List<MalEstado> arregloMalEstado { get; set; }
        public string boolExtraviadas { get; set; }
        public List<Perdido> arregloPerdido { get; set; }
        public string boolMaterial { get; set; }
        public string diasMaterial { get; set; }
        public DateTime? fechaPO { get; set; }
    }
}
