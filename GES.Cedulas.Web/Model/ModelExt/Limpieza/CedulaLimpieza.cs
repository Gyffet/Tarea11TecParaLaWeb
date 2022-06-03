using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Formulario.Model.ModelExt.Limpieza
{
    public class CedulaLimpieza
    {
        public string factura { get; set; }
        public string monto { get; set; }
        public string boolActividades { get; set; }
        public List<Actividades> arregloActividades { get; set; }
        public string boolUniforme { get; set; }
        public string boolEquipo { get; set; }
        public List<Equipo> arregloEquipo { get; set; }
        public string boolAsistencias { get; set; }
        public int? inasistencias { get; set; }
        public DateTime? fechaPO { get; set; }
        public DateTime? fechaVisita { get; set; }
        public bool primerMes { get; set; }
    }
}
