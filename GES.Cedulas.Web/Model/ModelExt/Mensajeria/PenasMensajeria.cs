using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Formulario.Model.ModelExt.Mensajeria
{
    public class PenasMensajeria
    {
        public decimal penaTotal { get; set; }
        public decimal?[] penaEntrega { get; set; }
        public decimal?[] penaRecoleccion { get; set; }
        public decimal?[] penaAcuses { get; set; }
        public decimal?[] penaMalEstado { get; set; }
        public decimal penaMaterial { get; set; }
        public decimal? penaPerdido { get; set; }
        public decimal calificacion { get; set; }
        public decimal penaCalificacion { get; set; }
        public decimal? penaPO { get; set; }
    }
}
