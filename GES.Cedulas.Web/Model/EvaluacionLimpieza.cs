﻿using System;
using System.Collections.Generic;

namespace Formulario.Model
{
    public partial class EvaluacionLimpieza
    {
        public int pkPenalizacion { get; set; }
        public string fcNoCed { get; set; }
        public string fcServicio { get; set; }
        public int? fiIDServicio { get; set; }
        public string fcMes { get; set; }
        public int fiAnio { get; set; }
        public string fcNumFactura { get; set; }
        public decimal fiMontoFactura { get; set; }
        public string fcInmueble { get; set; }
        public int fcAdministracion { get; set; }
        public decimal? fiCalificacion { get; set; }
        public decimal? fcPenaActividades { get; set; }
        public decimal? fcPenaUniforme { get; set; }
        public decimal? fcPenaMaterial { get; set; }
        public decimal? fiPenaCalificacion { get; set; }
        public decimal? fiPenaVisita { get; set; }
        public decimal? fiPenaPO { get; set; }
        public decimal? fiPenaEntregables { get; set; }
        public decimal? fiPenaCapacitacion { get; set; }
        public int? fiInasistencias { get; set; }
        public DateTime? fdFechaGuardado { get; set; }
        public DateTime? fdFechaEnviado { get; set; }
        public int? fiEstatus { get; set; }
        public string fcUsuario { get; set; }
        public string fcCorreoUsuario { get; set; }
        public int? fiRecalculado { get; set; }
    }
}