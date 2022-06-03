using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Formulario.Model.ModelExt.Limpieza
{
    public class Actividades
    {
        public DateTime fechaIncidencia { get; set; }
        public string tipoIncidencia { get; set; }
        public string areaIncidencia { get; set; }
        public string comentarios { get; set; }
        public string fechaIncidenciaFormat { get; set; }
    }
}
