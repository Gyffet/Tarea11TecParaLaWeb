using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Formulario.Model.ModelExt.Fumigacion
{
    public class Fumigacion
    {
        public DateTime fechaFumigacion { get; set; }
        public DateTime fechaReaparicion { get; set; }
        public string comentarios { get; set; }
        public string fechaFumigacionFormat { get; set; }
        public string fechaReaparicionFormat { get; set; }
    }
}
