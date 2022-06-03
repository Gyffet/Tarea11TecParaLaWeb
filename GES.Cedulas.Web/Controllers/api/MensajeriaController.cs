using Formulario.Model.ModelExt.Mensajeria;
using Formulario.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Formulario.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MensajeriaController : ControllerBase
    {
        private readonly IMensajeriaRepository repository;
        private readonly IHostingEnvironment environment;

        public MensajeriaController(IMensajeriaRepository repository, IHostingEnvironment environment)
        {
            this.repository = repository;
            this.environment = environment;
        }

        [HttpGet]
        [Route("aceptarMensajeria")]
        public IActionResult aceptarMensajeria(string folio)
        {
            try
            {
                repository.aceptarMensajeria(folio);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("rechazarMensajeria")]
        public IActionResult rechazarMensajeria(string folio)
        {
            try
            {
                repository.rechazarMensajeria(folio);
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

        [HttpPost]
        [Route("guardarMensajeria")]
        public IActionResult guardarMensajeria([FromBody] CedulaMensajeria cedula, string mes, int anio, string inmueble, int area, string folio)
        {
            try
            {
                var guardado = repository.guardarMensajeria(cedula, null, mes, anio, inmueble, area, folio);
                return Ok(guardado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
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
        [Route("enviarMensajeria")]
        public IActionResult enviarMensajeria([FromBody] CedulaMensajeria cedula, string jsonPenas,
            string mes, int anio, string inmueble, int area, string folio,
            string usuario, string correo)
        {
            try
            {
                string resultado = repository.enviarMensajeria(cedula, jsonPenas, mes, anio, inmueble, area, folio, usuario, correo);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("calcularPena")]
        public IActionResult calcularPena([FromBody] CedulaMensajeria cedula)
        {
            try
            {
                if (cedula.monto == "")
                {
                    cedula.monto = "0";
                }
                DateTime inicioContrato = DateTime.ParseExact("01/25/2021", "d", CultureInfo.InvariantCulture).Add(new TimeSpan(5, 0, 0));
                double penaMaterial = 0, penaTotal = 0, penaCalificacion = 0, penaPerdido = 0;
                double calificacion = 10;
                double penaPO = 0;

                double[] penaRecoleccion = new double[cedula.arregloRecoleccion.Count()];
                double[] penaEntrega = new double[cedula.arregloEntrega.Count()];
                double[] penaAcuses = new double[cedula.arregloAcuses.Count()];
                double[] penaMalEstado = new double[cedula.arregloMalEstado.Count()];
                //double[] penaPerdido = new double[cedula.arregloPerdido.Count()];

                if (cedula.fechaPO != null)
                {
                    DateTime s = (DateTime)cedula.fechaPO;
                    TimeSpan ts = new TimeSpan(5, 0, 0);
                    s = s.Date + ts;
                    DateTime? z = (DateTime?)s;
                    double diasAtraso = (z - inicioContrato).GetValueOrDefault().TotalDays;

                    if (diasAtraso > 14)
                    {
                        penaPO = Math.Round(89.62 / 0.01 * (diasAtraso-14), 2);
                    }
                }

                if (cedula.boolRecoleccion == "false")
                {
                    calificacion = calificacion - 2.5;
                    for (int i = 0; i < cedula.arregloRecoleccion.Count(); i++)
                    {
                        switch (cedula.arregloRecoleccion[i].tipoServRec)
                        {//agregar numero de factura
                            case "Nacional":
                                penaRecoleccion[i] = Math.Round((cedula.arregloRecoleccion[i].fechaEfectiva - cedula.arregloRecoleccion[i].fechaProgramada).TotalDays * 0.01 * 61.63, 2);
                                break;

                            case "Nacional Sobrepeso":
                                penaRecoleccion[i] = Math.Round((cedula.arregloRecoleccion[i].fechaEfectiva - cedula.arregloRecoleccion[i].fechaProgramada).TotalDays * 0.01 * 16.55, 2);
                                break;

                            case "Internacional":
                                penaRecoleccion[i] = Math.Round((cedula.arregloRecoleccion[i].fechaEfectiva - cedula.arregloRecoleccion[i].fechaProgramada).TotalDays * 0.01 * 492.86, 2);
                                break;

                            case "Internacional Sobrepeso":
                                penaRecoleccion[i] = Math.Round((cedula.arregloRecoleccion[i].fechaEfectiva - cedula.arregloRecoleccion[i].fechaProgramada).TotalDays * 0.01 * 51.74, 2);
                                break;

                        }
                    }
                }

                if (cedula.boolEntrega == "false")
                {
                    calificacion = calificacion - 2.5;
                    for (int i = 0; i < cedula.arregloEntrega.Count(); i++)
                    {
                        switch (cedula.arregloEntrega[i].tipoServRec)
                        {
                            case "Nacional":
                                penaEntrega[i] = Math.Round((cedula.arregloEntrega[i].fechaEfectiva - cedula.arregloEntrega[i].fechaProgramada).TotalDays * 0.1 * 61.63, 2);
                                break;

                            case "Nacional Sobrepeso":
                                penaEntrega[i] = Math.Round((cedula.arregloEntrega[i].fechaEfectiva - cedula.arregloEntrega[i].fechaProgramada).TotalDays * 0.1 * 16.55, 2);
                                break;

                            case "Internacional":
                                penaEntrega[i] = Math.Round((cedula.arregloEntrega[i].fechaEfectiva - cedula.arregloEntrega[i].fechaProgramada).TotalDays * 0.1 * 492.86, 2);
                                break;

                            case "Internacional Sobrepeso":
                                penaEntrega[i] = Math.Round((cedula.arregloEntrega[i].fechaEfectiva - cedula.arregloEntrega[i].fechaProgramada).TotalDays * 0.1 * 51.74, 2);
                                break;
                        }
                    }
                }

                if (cedula.arregloAcuses.Count() > 0)
                {
                    calificacion = calificacion - 0.5;
                    for (int i = 0; i < cedula.arregloAcuses.Count(); i++)
                    {
                        penaAcuses[i] = Math.Round(cedula.arregloAcuses[i].cantidadAcuses * 12.45 * 0.05, 2);
                        //penaAcuses[i] = Math.Round((cedula.arregloAcuses[i].fechaEfectiva - cedula.arregloAcuses[i].fechaProgramada).TotalDays * 0.05, 2);
                    }
                }

                if (cedula.arregloMalEstado.Count() > 0)
                {
                    calificacion = calificacion - 1;
                    for (int i = 0; i < cedula.arregloMalEstado.Count(); i++)
                    {
                        switch (cedula.arregloMalEstado[i].tipoServRec)
                        {
                            case "Nacional":
                                penaMalEstado[i] = Math.Round((89.62 * 1), 2);//Debido a cambio en el AT es solo valor de la UMA
                                break;

                            case "Nacional Sobrepeso":
                                penaMalEstado[i] = Math.Round((89.62 * 1), 2);//Reemplazar "1" por valor de la guia
                                break;

                            case "Internacional":
                                penaMalEstado[i] = Math.Round((89.62 * 1), 2);//Reemplazar "1" por valor de la guia
                                break;

                            case "Internacional Sobrepeso":
                                penaMalEstado[i] = Math.Round((89.62 * 1), 2);//Reemplazar "1" por valor de la guia
                                break;
                        }
                    }
                }

                if (cedula.arregloPerdido.Count() > 0)
                {
                    calificacion = calificacion - 2.5;
                    //for (int i = 0; i < cedula.arregloPerdido.Count(); i++)
                    //{
                    //switch (cedula.arregloPerdido[i].tipoServRec)
                    //{
                    //    case "Nacional":
                    //        penaPerdido[i] = Math.Round((0.0 * 1), 2);//Reemplazar "1" por valor de la guia
                    //        break;

                    //    case "Nacional Sobrepeso":
                    //        penaPerdido[i] = Math.Round((0.0 * 1), 2);//Reemplazar "1" por valor de la guia
                    //        break;

                    //    case "Internacional":
                    //        penaPerdido[i] = Math.Round((0.0 * 1), 2);//Reemplazar "1" por valor de la guia
                    //        break;

                    //    case "Internacional Sobrepeso":
                    //        penaPerdido[i] = Math.Round((0.0 * 1), 2);//Reemplazar "1" por valor de la guia
                    //        break;
                    //}
                    //}
                    penaPerdido = Math.Round(Convert.ToDouble(cedula.monto) * 0.003 * cedula.arregloPerdido.Count(), 2);
                }

                if (cedula.boolMaterial == "false")
                {
                    calificacion = calificacion - 0.5;
                    //Convert.ToDouble(cedula.monto);
                    //Int32.Parse(cedula.diasMaterial);
                    penaMaterial = Math.Round(Convert.ToDouble(cedula.monto) * 0.01 * Int32.Parse(cedula.diasMaterial), 2);
                }

                

                //Calculo de la pena total

                for (int i = 0; i < penaRecoleccion.Length; i++)
                {
                    penaTotal = penaTotal + penaRecoleccion[i];
                }

                for (int i = 0; i < penaEntrega.Length; i++)
                {
                    penaTotal = penaTotal + penaEntrega[i];
                }

                for (int i = 0; i < penaAcuses.Length; i++)
                {
                    penaTotal = penaTotal + penaAcuses[i];
                }

                for (int i = 0; i < penaMalEstado.Length; i++)
                {
                    penaTotal = penaTotal + penaMalEstado[i];
                }

                //for (int i =0; i < penaPerdido.Length; i++)
                //{
                //    penaTotal = penaTotal + penaPerdido[i];
                //}

                if (calificacion < 8)
                {
                    penaCalificacion = Math.Round(Convert.ToDouble(cedula.monto) * 0.01 / calificacion, 2);
                }

                penaTotal = penaTotal + penaMaterial + penaCalificacion + penaPerdido + penaPO;

                dynamic penas = new ExpandoObject();

                penas.penaTotal = penaTotal;
                penas.penaEntrega = penaEntrega;
                penas.penaRecoleccion = penaRecoleccion;
                penas.penaAcuses = penaAcuses;
                penas.penaMalEstado = penaMalEstado;
                penas.penaMaterial = penaMaterial;
                penas.penaPerdido = penaPerdido;
                penas.penaCalificacion = penaCalificacion;
                penas.calificacion = calificacion;
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
