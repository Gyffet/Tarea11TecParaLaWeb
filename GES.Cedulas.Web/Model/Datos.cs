using System;
using System.Collections.Generic;

namespace Formulario.Model
{
    public partial class Datos
    {
        public int pkFormulario { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
        public string documento { get; set; }
        public DateTime? fechaNacimiento { get; set; }
        public string curso { get; set; }
        public string horario { get; set; }
        public string formaPago { get; set; }
        public string NoContrato { get; set; }
        public string personaAtendio { get; set; }
        public DateTime? fechaInscripcion { get; set; }
    }
}
