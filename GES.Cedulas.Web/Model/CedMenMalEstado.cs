using System;
using System.Collections.Generic;

namespace Formulario.Model
{
    public partial class CedMenMalEstado
    {
        public int pkMalEstado { get; set; }
        public string fkCedula { get; set; }
        public string fcNoGuia { get; set; }
        public string fcCodRastreo { get; set; }
        public string fcTipoServicio { get; set; }
        public decimal? fiPena { get; set; }
    }
}
