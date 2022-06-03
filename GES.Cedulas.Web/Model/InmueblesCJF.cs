using System;
using System.Collections.Generic;

namespace Formulario.Model
{
    public partial class InmueblesCJF
    {
        public int Id { get; set; }
        public string Clave { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public DateTime FechaInicioVigencia { get; set; }
        public DateTime FechaFinVigencia { get; set; }
        public int? Administracion { get; set; }
    }
}
