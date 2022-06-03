using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Formulario.Model.ModelExt.Fumigacion
{
    public class CedulaFumigacion
    {
        public string factura { get; set; }
        public string monto { get; set; }
        public bool? boolfechas { get; set; }
        public List<Fechas> arregloFechas { get; set; }
        public bool? boolHoras { get; set; }
        public List<Horas> arregloHoras { get; set; }
        public bool? boolFumigacion { get; set; }
        public List<Fumigacion> arregloFumigacion { get; set; }
        public bool? boolProductos { get; set; }
        public DateTime? fechaPO { get; set; }
        public bool primerMes { get; set; }
    }
}
