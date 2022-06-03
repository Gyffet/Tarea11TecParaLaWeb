using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Formulario.Model.ModelExt.Limpieza
{
    public class Equipo
    {
        public DateTime fechaIncidencia { get; set; }
        public string maquinaria { get; set; }
        public string comentarios { get; set; }
        public string fechaIncidenciaFormat { get; set; }
    }
}
