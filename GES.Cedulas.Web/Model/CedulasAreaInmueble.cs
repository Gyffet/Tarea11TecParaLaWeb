using System;
using System.Collections.Generic;

namespace Formulario.Model
{
    public partial class CedulasAreaInmueble
    {
        public int pkClave { get; set; }
        public int fcClaveInmueble { get; set; }
        public string fcClaveAdscripcion { get; set; }
        public int fiClaveArea { get; set; }
        public string fcNombreArea { get; set; }
        public string fcTipoArea { get; set; }
    }
}
