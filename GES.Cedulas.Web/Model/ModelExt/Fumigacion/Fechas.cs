using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Formulario.Model.ModelExt.Fumigacion
{
    public class Fechas
    {
        public DateTime fechaProgramada { get; set; }
        public DateTime fechaRealizada { get; set; }
        public string comentarios { get; set; }
        public string fechaProgramadaFormat { get; set; }
        public string fechaRealizadaFormat { get; set; }
    }
}
