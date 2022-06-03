using Formulario.Model.ModelExt.Fumigacion;
using Formulario.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    public class FumigacionController : ControllerBase
    {
        private readonly IFumigacionRepository repository;
        private readonly IHostingEnvironment environment;

        public FumigacionController(IFumigacionRepository repository, IHostingEnvironment environment)
        {
            this.repository = repository;
            this.environment = environment;
        }

        [HttpGet]
        [Route("rechazar")]
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
        [Route("aceptar")]
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

        [HttpGet]
        [Route("eliminar")]
        public void eliminar(string folio)
        {
            repository.eliminar(folio);
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

        [HttpPost]
        [Route("guardar")]
        public IActionResult guardar([FromBody] CedulaFumigacion cedula, string entregables, string mes, int anio, string inmueble, int area, string folio, string usuario)
        {
            try
            {
                var a = JsonConvert.DeserializeObject<Entregables>(entregables);
                var guardado = repository.guardar(cedula, a, mes, anio, inmueble, area, folio, usuario);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("enviar")]
        public IActionResult enviar([FromBody] CedulaFumigacion cedula, string jsonPenas, string entregables,
            string mes, int anio, string inmueble, int area, string folio,
            string usuario, string correo)
        {
            try
            {
                var a = JsonConvert.DeserializeObject<Entregables>(entregables);
                var folioFinal = repository.enviar(cedula, jsonPenas, a, mes, anio, inmueble, area, folio, usuario, correo);
                return Ok(folioFinal);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("verificarPeriodo")]
        public IActionResult verificarPeriodo(string factura, string inmueble, string mes, string primavera)
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

        [HttpPost]
        [Route("calcularPena")]
        public IActionResult calcularPena([FromBody] CedulaFumigacion cedula)
        {
            try
            {
                if (cedula.monto == "")
                {
                    cedula.monto = "0";
                }

                DateTime inicioContrato = DateTime.ParseExact("01/01/2021", "d", CultureInfo.InvariantCulture).Add(new TimeSpan(5, 0, 0));
                double penaPO = 0, penaFechas = 0, penaHorario = 0, penaFumigacion = 0, penaEtiquetas = 0, penaTotal=0;
                double calificacion = 10;
                double penaCalificacion = 0;


                if (cedula.boolfechas == false)
                {
                    double diasTotales=0;
                    foreach(var i in cedula.arregloFechas)
                    {
                        DateTime s = (DateTime)i.fechaProgramada;
                        TimeSpan ts = new TimeSpan(5, 0, 0);
                        s = s.Date + ts;
                        DateTime? y = (DateTime?)s;

                        DateTime t = (DateTime)i.fechaRealizada;
                        TimeSpan tt = new TimeSpan(5, 0, 0);
                        t = t.Date + tt;
                        DateTime? z = (DateTime?)t;

                        double diasAtraso = ((DateTime)z - (DateTime)y).TotalDays;
                        diasTotales = diasTotales + diasAtraso;
                    }
                    penaFechas= Math.Round(Convert.ToDouble(cedula.monto) * 0.03 * diasTotales, 2);
                    calificacion = calificacion - 1;
                }

                if (cedula.boolHoras == false)
                {
                    double horasTotales = 0;
                    foreach (var i in cedula.arregloHoras)
                    {
                        double horasAtraso = (i.horaProgramada - i.horaEfectiva).TotalHours;
                        horasTotales = horasTotales + horasAtraso;
                    }
                    penaHorario = Math.Round(Convert.ToDouble(cedula.monto) * 0.03 * Math.Round(horasTotales), 2);
                    calificacion = calificacion - 1;
                }

                if (cedula.boolFumigacion == false)
                {
                    penaFumigacion = Math.Round(Convert.ToDouble(cedula.monto) * 0.05, 2);
                    calificacion = calificacion - 1.5;
                }

                if (cedula.boolProductos == false)
                {
                    penaEtiquetas = Math.Round(Convert.ToDouble(cedula.monto) * 0.02, 2);
                    calificacion = calificacion - 1;
                }

                //falta si calif < 8

                if (calificacion < 8)
                {
                    penaCalificacion = Math.Round(Convert.ToDouble(cedula.monto) * 0.02, 2);
                }

                if(cedula.primerMes == true)
                {
                    if (cedula.fechaPO != null)
                    {
                        DateTime s = (DateTime)cedula.fechaPO;
                        TimeSpan ts = new TimeSpan(5, 0, 0);
                        s = s.Date + ts;
                        DateTime? z = (DateTime?)s;
                        double diasAtraso = (z - inicioContrato).GetValueOrDefault().TotalDays;

                        if (diasAtraso > 3)
                        {
                            penaPO = Math.Round(Convert.ToDouble(cedula.monto) * 0.03 * (diasAtraso - 3), 2);
                        }
                    }
                    else if (cedula.fechaPO == null)
                    {
                        penaPO = Math.Round(Convert.ToDouble(cedula.monto) * 0.03 * (27), 2);
                    }
                    
                }
                

                penaTotal = penaPO + penaFechas + penaHorario + penaFumigacion + penaEtiquetas + penaCalificacion;

                dynamic penas = new ExpandoObject();

                penas.penaTotal = penaTotal;
                penas.penaPO = penaPO;
                penas.penaFechas = penaFechas;
                penas.penaHorario = penaHorario;
                penas.penaFumigacion = penaFumigacion;
                penas.penaEtiquetas = penaEtiquetas;
                penas.penaCalificacion = penaCalificacion;
                penas.calificacion = calificacion;

                return Ok(penas);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
            
    }
}