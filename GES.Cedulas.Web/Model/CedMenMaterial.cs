using System;
using System.Collections.Generic;

namespace Formulario.Model
{
    public partial class CedMenMaterial
    {
        public int pkMaterial { get; set; }
        public string fkCedula { get; set; }
        public int? fiMaterial { get; set; }
        public int? fiDiasMaterial { get; set; }
        public decimal? fiPenaMaterial { get; set; }
    }
}
