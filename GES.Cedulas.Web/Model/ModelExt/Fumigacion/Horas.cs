using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Formulario.Model.ModelExt.Fumigacion
{
    public class Horas
    {
        public DateTime fechaProgramada { get; set; }
        public DateTime horaProgramada { get; set; }
        public DateTime horaEfectiva { get; set; }
        public string comentarios { get; set; }
        public string fechaProgramadaFormat { get; set; }
    }
}