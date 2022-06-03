using Formulario.Model;
using Formulario.Model.ModelExt.Fumigacion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Formulario.Repositories
{
    public interface IFumigacionRepository
    {
        public void aceptarCedula(string folio);
        public void rechazarCedula(string folio);
        public void InsertArchivo(string id, string fileName, string contentType, int lenght);
        public void eliminar(string folio);
        public int guardar(CedulaFumigacion cedula, Entregables eF, string mes, int anio, string inmueble, int area, string folio, string usuario);
        public int validarPeriodo(string factura, string inmueble, string mes, string primavera);
        public string enviar(CedulaFumigacion cedula, string jsonPenas, Entregables eF, string mes, int anio, string inmueble, int area, string folio, string usuario, string correo);
        
        
    }

    public class FumigacionRepository : IFumigacionRepository
    {
        private readonly BD_HerramientasDGSGContext context;
        private readonly ICedulasRepository cedulasRep;

        public FumigacionRepository(BD_HerramientasDGSGContext context, ICedulasRepository cedulasRep)
        {
            this.context = context;
            this.cedulasRep = cedulasRep;
        }

        public string folioFuera = "";

        public void aceptarCedula(string folio)
        {
            var cedula = (from fumigacion in context.EvaluacionFumigacion
                          where fumigacion.fcNoCed.Equals(folio)
                          select fumigacion).FirstOrDefault();

            cedula.fiEstatus = 1;
            context.Update(cedula);
            context.SaveChanges();
        }

        public void rechazarCedula(string folio)
        {
            var cedula = (from limpieza in context.EvaluacionFumigacion
                          where limpieza.fcNoCed.Equals(folio)
                          select limpieza).FirstOrDefault();

            cedula.fdFechaEnviado = null;

            cedula.fiEstatus = 0;

            context.Update(cedula);
            context.SaveChanges();
        }


        public void InsertArchivo(string id, string fileName, string contentType, int lenght)
        {
            ArchivosCedulas archivo = new ArchivosCedulas();
            archivo.fkNoCed = id;
            archivo.fkServicio = 3; // 2 significa limpieza
            archivo.fcNombreOriginal = fileName;
            archivo.length = lenght;
            archivo.contentType = contentType;
            var fecha = DateTime.Now;
            archivo.FechaCarga = fecha;
            context.Add(archivo);
            context.SaveChanges();
        }


        public void eliminar(string folio)
        {
            var cedula = (from mensajeria in context.EvaluacionFumigacion
                          where mensajeria.fcNoCed.Equals(folio)
                          select mensajeria).FirstOrDefault();
            context.Remove(cedula);
            context.SaveChanges();

        }

        public int validarPeriodo(string factura, string inmueble, string mes, string primavera)
        {
            if (factura == null || factura == "" || factura == " ")
            {
                factura = "Por Asignar";
            }
            int entero = Int32.Parse(primavera);
            int i = 0, j = 0, k = 0, l = 0;

            var evaluacion = (from periodo in context.EvaluacionFumigacion select periodo).ToList();

            foreach (var a in evaluacion)
            {
                if (a.fcNumFactura == factura)
                {
                    i = 1;
                }
                if (a.fcInmueble == inmueble)
                {
                    i = 1;
                }
                if (a.fcMes == mes)
                {
                    i = 1;
                }
                if (a.fiAnio == entero)
                {
                    i = 1;
                }

            }

            var validacion = (from periodo in context.EvaluacionFumigacion
                              where periodo.fcInmueble.Equals(inmueble)
                              && periodo.fcMes.Equals(mes)
                              && periodo.fiAnio.Equals(entero)
                              && periodo.fcNumFactura.Equals(factura)
                              select periodo).ToList();

            if (validacion.Count > 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }

        }

        public string enviar(CedulaFumigacion cedula, string jsonPenas, Entregables eF, string mes, int anio, string inmueble, int area, string folio, string usuario, string correo)
        {
            try
            {
                if (guardar(cedula, eF, mes, anio, inmueble, area, folio, usuario) == 1)
                {
                    PenasFumigacion penas;
                    if (jsonPenas == null || jsonPenas == "")
                    {
                        penas = new PenasFumigacion();
                    }
                    else
                    {
                        penas = JsonConvert.DeserializeObject<PenasFumigacion>(jsonPenas);
                    }

                    var ahora = DateTime.Now;
                    EvaluacionFumigacion cedulaGuardar;

                    if (folio != null)
                    {
                        cedulaGuardar = (from ultimaCed in context.EvaluacionFumigacion
                                         where ultimaCed.fcNoCed.Equals(folio)
                                         select ultimaCed).FirstOrDefault();
                    }
                    else
                    {
                        cedulaGuardar = (from ultimaCed in context.EvaluacionFumigacion
                                         where ultimaCed.fcNoCed.Equals(folioFuera)
                                         select ultimaCed).FirstOrDefault();
                    }

                    cedulaGuardar.fiEstatus = null;

                    cedulaGuardar.fdFechaEnviado = ahora;
                    cedulaGuardar.fiPenaPO = penas.penaPO;
                    cedulaGuardar.fiPenaFechas = penas.penaFechas;
                    cedulaGuardar.fiPenaHorario = penas.penaHorario;
                    cedulaGuardar.fiPenaFumigacion = penas.penaFumigacion;
                    cedulaGuardar.fiPenaEtiquetas = penas.penaEtiquetas;
                    cedulaGuardar.fiPenaCalificacion = penas.penaCalificacion;
                    cedulaGuardar.fiCalificacion = penas.calificacion;
                    cedulaGuardar.fiRecalculado = 1;
                    //cedulaGuardar.fcCorreoUsuario = correo;
                    //cedulaGuardar.fcUsuario = usuario;
                    context.Update(cedulaGuardar); ;
                    context.SaveChanges();
                    folio = cedulaGuardar.fcNoCed;
                }
                return folio;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            
            
        }

        public int guardar(CedulaFumigacion cedula, Entregables eF, string mes, int anio, string inmueble, int area, string folio, string usuario)
        {
            var hoy = DateTime.Now;
            if (folio != null)
            {
                var cedulaActualizable = context.EvaluacionFumigacion.Where(x => x.fcNoCed.Equals(folio)).FirstOrDefault(); ;
                var entregableActualizable = context.EntregablesFumigacion.Where(x => x.fkCedula.Equals(folio)).FirstOrDefault();
                if (cedula.factura == "" || cedula.factura == " ")
                {
                    cedulaActualizable.fcNumFactura = "Por Asignar";
                }
                else
                {
                    cedulaActualizable.fcNumFactura = cedula.factura;
                }
                if (cedula.monto == "" || cedula.monto == " ")
                {
                    cedulaActualizable.fiMontoFactura = 0;
                }
                else
                {
                    cedulaActualizable.fiMontoFactura = Convert.ToDecimal(cedula.monto);
                }

                cedulaActualizable.fcInmueble = inmueble;
                cedulaActualizable.fdFechaGuardado = hoy;
                cedulaActualizable.fcUsuario = usuario;
                context.Update(cedulaActualizable);
                context.SaveChanges();

                entregableActualizable.fdCierreMes = eF.fechaCierre;
                entregableActualizable.fdReporteServ = eF.reporteServ;
                entregableActualizable.fdListadePersonal = eF.listaPersonal;
                entregableActualizable.fdSUAIMSS = eF.constanciaSUA;
                entregableActualizable.fdActaEntrega = eF.fechaActaEntrega;
                entregableActualizable.fdProgramaOperacion = eF.fechaPO;
                entregableActualizable.fdUniforme = eF.fechaUniforme;
                entregableActualizable.fdActaInicio = eF.fechaActaInicio;
                entregableActualizable.fdPersonal = eF.fechaPersonal;
                context.Update(cedulaActualizable);
                context.SaveChanges();

                if (cedula.arregloFechas != null)
                {
                    var arreglo = context.CedFumFechas.Where(x => x.fkCedula.Equals(folio));
                    foreach (var a in arreglo)
                    {
                        context.Remove(a);
                    }
                    foreach (var a in cedula.arregloFechas)
                    {
                        CedFumFechas fechas = new CedFumFechas();
                        fechas.fkCedula = folio;
                        fechas.fdFechaProgramada = a.fechaProgramada;
                        fechas.fdFechaRealizada = a.fechaRealizada;
                        fechas.fcComentarios = a.comentarios;
                        context.Add(fechas);
                    }
                    context.SaveChanges();
                }

                if (cedula.arregloHoras != null)
                {
                    var arreglo = context.CedFumHoras.Where(x => x.fkCedula.Equals(folio));
                    foreach (var a in arreglo)
                    {
                        context.Remove(a);
                    }
                    foreach (var e in cedula.arregloHoras)
                    {
                        CedFumHoras hora = new CedFumHoras();
                        hora.fkCedula = folio;
                        hora.fdFechaProgramada = e.fechaProgramada;
                        hora.fdHoraProgramada = e.horaProgramada;
                        hora.fdHoraEfectiva = e.horaEfectiva;
                        hora.fcComentarios = e.comentarios;
                        context.Add(hora);
                    }
                    context.SaveChanges();
                }

                if (cedula.arregloFumigacion != null)
                {
                    var arreglo = context.CedFumFumigacion.Where(x => x.fkCedula.Equals(folio));
                    foreach (var a in arreglo)
                    {
                        context.Remove(a);
                    }
                    foreach (var e in cedula.arregloFumigacion)
                    {
                        CedFumFumigacion fumigacion = new CedFumFumigacion();
                        fumigacion.fkCedula = folio;
                        fumigacion.fdFechaFumigacion = e.fechaFumigacion;
                        fumigacion.fdFechaReaparacion = e.fechaReaparicion;
                        fumigacion.fcComentarios = e.comentarios;
                        context.Add(fumigacion);
                    }
                    context.SaveChanges();
                }

                if (cedula.boolProductos == false)
                {
                    cedulaActualizable.fiEtiquetas = 0;
                }
                else if (cedula.boolProductos == true)
                {
                    cedulaActualizable.fiEtiquetas = 1;
                }
                else
                {
                    cedulaActualizable.fiEtiquetas = null;
                }
            }
            
            else
            {
                var folioGenerado = cadenaAleatoria();
                var inmuebles = cedulasRep.getInmuebles(area);//Esta linea podría fallar despues de la migración estar al pendiente
                var claveInmueble = inmuebles.Select(x => x.Administracion).FirstOrDefault();

                EntregablesFumigacion entregable = new EntregablesFumigacion();
                entregable.fkCedula = folioGenerado;
                entregable.fdCierreMes = eF.fechaCierre;
                entregable.fdReporteServ = eF.reporteServ;
                entregable.fdListadePersonal = eF.listaPersonal;
                entregable.fdSUAIMSS = eF.constanciaSUA;
                entregable.fdActaEntrega = eF.fechaActaEntrega;
                entregable.fdProgramaOperacion = eF.fechaPO;
                entregable.fdUniforme = eF.fechaUniforme;
                entregable.fdActaInicio = eF.fechaActaInicio;
                entregable.fdPersonal = eF.fechaPersonal;
                context.Update(entregable);
                context.SaveChanges();

                EvaluacionFumigacion cedulaaGuardar = new EvaluacionFumigacion();
                cedulaaGuardar.fcNoCed = folioGenerado;
                cedulaaGuardar.fcMes = mes;
                cedulaaGuardar.fiAnio = anio;
                cedulaaGuardar.fcAdministracion = (int)claveInmueble;

                if (cedula.factura == "" || cedula.factura == " ")
                {
                    cedulaaGuardar.fcNumFactura = "Por Asignar";
                }
                else
                {
                    cedulaaGuardar.fcNumFactura = cedula.factura;
                }
                if (cedula.monto == "" || cedula.monto == " ")
                {
                    cedulaaGuardar.fiMontoFactura = 0;
                }
                else
                {
                    cedulaaGuardar.fiMontoFactura = Convert.ToDecimal(cedula.monto);
                }
                
                cedulaaGuardar.fcInmueble = inmueble;
                cedulaaGuardar.fdFechaGuardado = hoy;
                cedulaaGuardar.fcUsuario = usuario;
                cedulaaGuardar.fcServicio = "Fumigación";
                cedulaaGuardar.fiIDServicio = 3;
                context.Add(cedulaaGuardar);
                context.SaveChanges();





                //Aqui empieza lo diferente
                if (cedula.arregloFechas != null)
                {
                    foreach (var r in cedula.arregloFechas)
                    {
                        CedFumFechas fechas = new CedFumFechas();
                        fechas.fkCedula = folioGenerado;
                        fechas.fdFechaProgramada = r.fechaProgramada;
                        fechas.fdFechaRealizada = r.fechaRealizada;
                        fechas.fcComentarios = r.comentarios;
                        context.Add(fechas);
                        context.SaveChanges();
                    }
                }


                if (cedula.arregloHoras != null)
                {
                    foreach (var e in cedula.arregloHoras)
                    {
                        CedFumHoras horas = new CedFumHoras();
                        horas.fkCedula = folioGenerado;
                        horas.fdFechaProgramada = e.fechaProgramada;
                        horas.fdHoraProgramada = e.horaProgramada;
                        horas.fdHoraEfectiva = e.horaEfectiva;
                        horas.fcComentarios = e.comentarios;
                        context.Add(horas);
                        context.SaveChanges();
                    }
                }

                if (cedula.arregloFumigacion != null)
                {
                    foreach (var e in cedula.arregloFumigacion)
                    {
                        CedFumFumigacion fumigacion = new CedFumFumigacion();
                        fumigacion.fkCedula = folioGenerado;
                        fumigacion.fdFechaFumigacion = e.fechaFumigacion;
                        fumigacion.fdFechaReaparacion = e.fechaReaparicion;
                        fumigacion.fcComentarios = e.comentarios;
                        context.Add(fumigacion);
                    }
                    context.SaveChanges();
                }

                if (cedula.boolProductos == false)
                {
                    cedulaaGuardar.fiEtiquetas = 0;
                }
                else if (cedula.boolProductos == true)
                {
                    cedulaaGuardar.fiEtiquetas = 1;
                }
                else
                {
                    cedulaaGuardar.fiEtiquetas = null;
                }
            }
            return 1;
        }

        public string cadenaAleatoria()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            var stringChars = new char[6];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            var cadenaAleatoria = new String(stringChars);
            folioFuera = cadenaAleatoria;
            return cadenaAleatoria;
        }
    }
}
