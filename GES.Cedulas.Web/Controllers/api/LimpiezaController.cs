using Formulario.Model;
using Formulario.Model.ModelExt.Limpieza;
using Formulario.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Formulario.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LimpiezaController : ControllerBase
    {
        private readonly ILimpiezaRepository repository;
        private readonly BD_HerramientasDGSGContext context;
        private readonly IHostingEnvironment environment;

        public LimpiezaController(ILimpiezaRepository repository, BD_HerramientasDGSGContext context, IHostingEnvironment environment)
        {
            this.repository = repository;
            this.context = context;
            this.environment = environment;
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("subirArchivoPDF")]
        //public ActionResult subirArchivoPDF(IFormFile files, string noCed)
        public ActionResult subirArchivoPDF(string files)
        {
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    string id = Request.Form["id"];
                    var archivos = Request.Form.Files[0];
                    var fileName = archivos.FileName;
                    var contentType = archivos.ContentType;
                    var length = archivos.Length;

                    string folderName = "FacturasCedulas";
                    string webRootPath = environment.ContentRootPath;
                    string newPath = Path.Combine(webRootPath, folderName);

                    if (!Directory.Exists(newPath))
                    {
                        Directory.CreateDirectory(newPath);
                    }


                    string fullPath = Path.Combine(newPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        archivos.CopyTo(stream);
                    }
                    //guardar referencia en la base de datos
                    repository.InsertArchivo(id, fileName, contentType, Convert.ToInt32(length));
                    return Ok();
                }
                else
                    return BadRequest("Seleccionar archivo");
            }
            catch (Exception ex)
            {
                return (BadRequest("El archivo es demasiado largo o incompatible"));
            }
        }

        [HttpGet]
        [Route("eliminar")]
        public void eliminar(string folio)
        {
            repository.eliminar(folio);
        }

        [HttpGet]
        [Route("recalcular")]
        public IActionResult recalcular(bool entregables, DateTime? fechaLimite, DateTime? fechaEntrega, int tipoEntregable, bool capacitacion, string noCed)
        {
            try
            {
                EvaluacionLimpieza cedula = (from ced in context.EvaluacionLimpieza
                                   where ced.fcNoCed.Equals(noCed)
                                   select ced).FirstOrDefault();
                double penaEntregables = 0, penaCapacitacion = 0;
                decimal? penaCalificacion = 0;
                decimal? calificacion = cedula.fiCalificacion;
                if (entregables == false)
                {
                    double diasAtraso = (fechaEntrega - fechaLimite).GetValueOrDefault().TotalDays;
                    if (diasAtraso > 0)
                    {
                        calificacion = calificacion - (decimal)0.8;
                        penaEntregables = Math.Round((double)cedula.fiMontoFactura * 0.0001 * diasAtraso * tipoEntregable, 2);
                    }
                }

                if (capacitacion == false)
                {
                    calificacion = calificacion - (decimal)1;
                    penaCapacitacion = Math.Round((double)cedula.fiMontoFactura * 0.01, 2);
                }

                if (cedula.fiCalificacion < 8)
                {
                    cedula.fiCalificacion = calificacion;
                }
                else
                {
                    if (calificacion < 8)
                    {
                        penaCalificacion = cedula.fiMontoFactura * (decimal)0.01 / calificacion;
                        penaCalificacion = Math.Round((decimal)penaCalificacion, 2);
                    }
                    cedula.fiCalificacion = calificacion;
                }

                cedula.fiPenaEntregables = (decimal?)penaEntregables;
                cedula.fiPenaCapacitacion = (decimal?)penaCapacitacion;
                cedula.fiRecalculado = 1;
                context.Update(cedula);
                context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("rechazarCedula")]
        public IActionResult rechazarCedula(string folio)
        {
            try
            {
                repository.rechazarCedula(folio);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("aceptarCedula")]
        public IActionResult aceptarCedula(string folio)
        {
            try
            {
                repository.aceptarCedula(folio);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("enviarLimpieza")]
        public IActionResult enviarLimpieza([FromBody] CedulaLimpieza cedula, string jsonPenas, string entregables,
            string mes, int anio, string inmueble, int area, string folio,
            string usuario, string correo)
        {
            try
            {
                var a = JsonConvert.DeserializeObject<Entregables>(entregables);
                var NoCed = repository.enviarLimpieza(cedula, jsonPenas, a, mes, anio, inmueble, area, folio, usuario, correo);
                return Ok(NoCed);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("guardarLimpieza")]
        public IActionResult guardarLimpieza([FromBody] CedulaLimpieza cedula, string entregables, string mes, int anio, string inmueble, int area, string folio, string usuario)
        {
            try
            {
                var a = JsonConvert.DeserializeObject<Entregables>(entregables);
                var guardado = repository.guardarLimpieza(cedula, a, mes, anio, inmueble, area, folio, usuario);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("calcularPena")]
        public IActionResult calcularPena([FromBody] CedulaLimpieza cedula)
        {
            try
            {
                if (cedula.monto == "")
                {
                    cedula.monto = "0";
                }

                var contrato = context.ContratoCedula.Where(x => x.fkServicio.Equals(1) && x.fiActivo.Equals(true)).FirstOrDefault();
                double penaUniforme = 0, penaTotal = 0, penaCalificacion = 0, penaActividades = 0, penaEquipo = 0, penaVisita = 0, penaPO = 0;
                double calificacion = 10;

                if (cedula.boolActividades == "false")
                {
                    calificacion = calificacion - 1.4;
                    penaActividades = Math.Round(Convert.ToDouble(cedula.monto) * 0.0001 * cedula.arregloActividades.Count(), 2);
                }

                if (cedula.boolUniforme == "false")
                {
                    calificacion = calificacion - 0.4;
                    penaUniforme = Math.Round(Convert.ToDouble(cedula.monto) * 0.001, 2);
                }

                if (cedula.boolEquipo == "false")
                {
                    calificacion = calificacion - 1.4;
                    penaEquipo = Math.Round(Convert.ToDouble(cedula.monto) * 0.001, 2);
                }

                if (calificacion < 8)
                {
                    penaCalificacion = Math.Round(Convert.ToDouble(cedula.monto) * 0.01 / calificacion, 2);
                }

                if (cedula.primerMes == true)
                {
                    if (cedula.fechaPO != null)
                    {
                        var fecha = cedula.fechaPO.GetValueOrDefault().Date;
                        int diasAtraso = 0;
                        var ultimodia = new DateTime(contrato.fdFechaInicio.Year, contrato.fdFechaInicio.Month, 1).AddMonths(1).AddDays(-1);

                        for (var a = contrato.fdFechaInicio; a <= fecha; a = a.AddDays(1))
                        {
                            if (a > ultimodia)
                            {
                                break;
                            }
                            if ((a.DayOfWeek == DayOfWeek.Saturday) || (a.DayOfWeek == DayOfWeek.Sunday) && diasAtraso <= 3)//fin de semana antes de atraso no se cuenta
                            { }
                            else if ((a.DayOfWeek == DayOfWeek.Saturday) || (a.DayOfWeek == DayOfWeek.Sunday) && diasAtraso > 3)//fin de semana despues de atraso si se cuenta
                            {
                                diasAtraso += 1;
                            }
                            else//dia normal de la semana
                            {
                                diasAtraso += 1;
                            }

                        }

                        if (diasAtraso > 3)
                        {
                            penaPO = Math.Round(89.62 / 0.01 * (diasAtraso - 3), 2);
                        }
                    }
                    else if (cedula.fechaPO == null)
                    {
                        int diasAtraso = 0;
                        var ultimodía = new DateTime(contrato.fdFechaInicio.Year, contrato.fdFechaInicio.Month, 1).AddMonths(1).AddDays(-1);
                        for (var a = contrato.fdFechaInicio; a <= ultimodía; a = a.AddDays(1))
                        {
                            if ((a.DayOfWeek == DayOfWeek.Saturday) || (a.DayOfWeek == DayOfWeek.Sunday) && diasAtraso <= 3)//fin de semana antes de atraso no se cuenta
                            { }
                            else if ((a.DayOfWeek == DayOfWeek.Saturday) || (a.DayOfWeek == DayOfWeek.Sunday) && diasAtraso > 3)//fin de semana despues de atraso si se cuenta
                            {
                                diasAtraso += 1;
                            }
                            else//dia normal de la semana
                            {
                                diasAtraso += 1;
                            }
                        }
                        penaPO = Math.Round(89.62 / 0.01 * (diasAtraso - 3), 2);
                    }


                    if (cedula.fechaVisita != null)
                    {
                        var fecha = cedula.fechaPO.GetValueOrDefault().Date;
                        int diasAtraso = 0;
                        var ultimodia = new DateTime(contrato.fdFechaInicio.Year, contrato.fdFechaInicio.Month, 1).AddMonths(1).AddDays(-1);

                        for (var a = contrato.fdFechaInicio; a <= fecha; a = a.AddDays(1))
                        {
                            if (a > ultimodia)
                            {
                                break;
                            }
                            if ((a.DayOfWeek == DayOfWeek.Saturday) || (a.DayOfWeek == DayOfWeek.Sunday) && diasAtraso <= 3)//fin de semana antes de atraso no se cuenta
                            { }
                            else if ((a.DayOfWeek == DayOfWeek.Saturday) || (a.DayOfWeek == DayOfWeek.Sunday) && diasAtraso > 3)//fin de semana despues de atraso si se cuenta
                            {
                                diasAtraso += 1;
                            }
                            else//dia normal de la semana
                            {
                                diasAtraso += 1;
                            }
                        }

                        if (diasAtraso > 8)
                        {
                            penaVisita = Math.Round(Convert.ToDouble(cedula.monto) * 0.01 * (diasAtraso - 8), 2);
                        }
                    }
                    else if (cedula.fechaVisita == null)
                    {
                        int diasAtraso = 0;
                        var ultimodía = new DateTime(contrato.fdFechaInicio.Year, contrato.fdFechaInicio.Month, 1).AddMonths(1).AddDays(-1);
                        for (var a = contrato.fdFechaInicio; a <= ultimodía; a = a.AddDays(1))
                        {
                            if ((a.DayOfWeek == DayOfWeek.Saturday) || (a.DayOfWeek == DayOfWeek.Sunday) && diasAtraso <= 3)//fin de semana antes de atraso no se cuenta
                            { }
                            else if ((a.DayOfWeek == DayOfWeek.Saturday) || (a.DayOfWeek == DayOfWeek.Sunday) && diasAtraso > 3)//fin de semana despues de atraso si se cuenta
                            {
                                diasAtraso += 1;
                            }
                            else//dia normal de la semana
                            {
                                diasAtraso += 1;
                            }
                        }
                        penaVisita = Math.Round(Convert.ToDouble(cedula.monto) * 0.01 * (diasAtraso - 8), 2);
                    }

                }

                penaTotal = penaTotal + penaUniforme + penaCalificacion + penaActividades + penaPO + penaVisita;

                dynamic penas = new ExpandoObject();

                penas.penaTotal = penaTotal;
                penas.penaActividades = penaActividades;
                penas.penaUniforme = penaUniforme;
                penas.penaEquipo = penaEquipo;
                penas.penaCalificacion = penaCalificacion;
                penas.calificacion = calificacion;
                penas.penaVisita = penaVisita;
                penas.penaPO = penaPO;

                return Ok(penas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("verificarPeriodo")]
        public IActionResult VerificarPeriodo(string factura, string inmueble, string mes, string primavera)
        {
            try
            {

                var validacion = repository.validarPeriodo(factura, inmueble, mes, primavera);
                if (validacion == 1)
                {
                    return Ok();
                }
                else
                {
                    object error = null;
                    int i2 = (int)error;
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
