using Formulario.Model;
using Formulario.Model.ModelExt.Limpieza;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Formulario.Repositories
{
    public interface ILimpiezaRepository
    {
        public void InsertArchivo(string id, string fileName, string contentType, int lenght);
        public void aceptarCedula(string folio);
        public void rechazarCedula(string folio);
        public void eliminar(string folio);
        public int guardarLimpieza(CedulaLimpieza cedula, Entregables eL, string mes, int anio, string inmueble, int area, string folio, string usuario);
        public int validarPeriodo(string factura, string inmueble, string mes, string primavera);
        public string enviarLimpieza(CedulaLimpieza cedula, string jsonPenas, Entregables eL, string mes, int anio, string inmueble, int area, string folio, string usuario, string correo);
    }

    public class LimpiezaRepsitory: ILimpiezaRepository
    {
        private readonly BD_HerramientasDGSGContext context;
        private readonly ICedulasRepository cedulasRep;

        public LimpiezaRepsitory(BD_HerramientasDGSGContext context, ICedulasRepository cedulasRep)
        {
            this.context = context;
            this.cedulasRep = cedulasRep;
        }

        public string folioFuera = "";

        public void InsertArchivo(string id, string fileName, string contentType, int lenght)
        {
            ArchivosCedulas archivo = new ArchivosCedulas();
            archivo.fkNoCed = id;
            archivo.fkServicio = 2; // 2 significa limpieza
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
            var cedula = (from mensajeria in context.EvaluacionLimpieza
                          where mensajeria.fcNoCed.Equals(folio)
                          select mensajeria).FirstOrDefault();
            context.Remove(cedula);
            context.SaveChanges();

        }

        public void aceptarCedula(string folio)
        {
            var cedula = (from limpieza in context.EvaluacionLimpieza
                          where limpieza.fcNoCed.Equals(folio)
                          select limpieza).FirstOrDefault();

            cedula.fiEstatus = 1;
            context.Update(cedula);
            context.SaveChanges();
        }

        public void rechazarCedula(string folio)
        {
            var cedula = (from limpieza in context.EvaluacionLimpieza
                          where limpieza.fcNoCed.Equals(folio)
                          select limpieza).FirstOrDefault();

            cedula.fdFechaEnviado = null;
            
            cedula.fiEstatus = 0;

            context.Update(cedula);
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

            var dfewdfs = (from periodo in context.EvaluacionLimpieza select periodo).ToList();

            foreach (var a in dfewdfs)
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

            var validacion = (from periodo in context.EvaluacionLimpieza
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

        public int guardarLimpieza(CedulaLimpieza cedula, Entregables eL, string mes, int anio, string inmueble, int area, string folio, string usuario)
        {
            var hoy = DateTime.Now;
            if (folio != null)
            {
                var cedulaActualizable = context.EvaluacionLimpieza.Where(x => x.fcNoCed.Equals(folio)).FirstOrDefault();
                var entregableActualizable = context.EntregablesLimpieza.Where(x => x.fkCedula.Equals(folio)).FirstOrDefault();
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
                cedulaActualizable.fiInasistencias = cedula.inasistencias;
                cedulaActualizable.fcUsuario = usuario;
                context.Update(cedulaActualizable);
                context.SaveChanges();

                
                entregableActualizable.fdProgramaOperacion = eL.fechaPO;
                entregableActualizable.fdVisitaInstalaciones = eL.fechaVisita;
                entregableActualizable.fdGafeteUniforme = eL.fechaUniforme;
                entregableActualizable.fdActaInicio = eL.fechaActaInicio;
                entregableActualizable.fdCierreMes = eL.fechaCierre;
                entregableActualizable.fdActaServicios = eL.fechaActaEntrega;
                entregableActualizable.fdSUAIMSS = eL.constanciaSUA;
                context.Update(entregableActualizable);
                context.SaveChanges();

                if (cedula.arregloActividades != null)
                {
                    var arreglo = context.CedLimpActividades.Where(x => x.fkCedula.Equals(folio));
                    foreach (var a in arreglo)
                    {
                        context.Remove(a);
                    }
                    foreach (var a in cedula.arregloActividades)
                    {
                        CedLimpActividades recoleccion = new CedLimpActividades();
                        recoleccion.fkCedula = folio;
                        recoleccion.fdFechaInci = a.fechaIncidencia;
                        recoleccion.fcTipo = a.tipoIncidencia;
                        recoleccion.fcArea = a.areaIncidencia;
                        recoleccion.fcComentarios = a.comentarios;
                        context.Add(recoleccion);
                    }
                    context.SaveChanges();
                }

                if (cedula.boolUniforme == "false")
                {
                    cedulaActualizable.fcPenaUniforme = 0;
                }
                else
                {
                    cedulaActualizable.fcPenaUniforme = null;
                }

                if (cedula.arregloEquipo != null)
                {
                    var arreglo = context.CedLimpEquipo.Where(x => x.fkCedula.Equals(folio));
                    foreach (var a in arreglo)
                    {
                        context.Remove(a);
                    }
                    foreach (var e in cedula.arregloEquipo)
                    {
                        CedLimpEquipo entrega = new CedLimpEquipo();
                        entrega.fkCedula = folio;
                        entrega.fdFechaInci = e.fechaIncidencia;
                        entrega.fcMaquina = e.maquinaria;
                        entrega.fcComentarios = e.comentarios;
                        context.Add(entrega);
                    }
                    context.SaveChanges();
                }
            }

            else
            {
                var folioGenerado = cadenaAleatoria();
                var inmuebles = cedulasRep.getInmuebles(area);//Esta linea podría fallar despues de la migración estar al pendiente
                var claveInmueble = inmuebles.Select(x => x.Administracion).FirstOrDefault();

                EntregablesLimpieza entregable = new EntregablesLimpieza();
                entregable.fkCedula = folioGenerado;
                entregable.fdProgramaOperacion = eL.fechaPO;
                entregable.fdVisitaInstalaciones = eL.fechaVisita;
                entregable.fdGafeteUniforme = eL.fechaUniforme;
                entregable.fdActaInicio = eL.fechaActaInicio;
                entregable.fdCierreMes = eL.fechaCierre;
                entregable.fdActaServicios = eL.fechaActaEntrega;
                entregable.fdSUAIMSS = eL.constanciaSUA;
                context.Update(entregable);
                context.SaveChanges();



                EvaluacionLimpieza cedulaaGuardar = new EvaluacionLimpieza();
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
                cedulaaGuardar.fiInasistencias = cedula.inasistencias;
                cedulaaGuardar.fcUsuario = usuario;
                cedulaaGuardar.fcServicio = "Limpieza";
                cedulaaGuardar.fiIDServicio = 2;
                context.Add(cedulaaGuardar);
                context.SaveChanges();



                //Aqui empieza lo diferente
                if (cedula.arregloActividades != null)

                {
                    foreach (var r in cedula.arregloActividades)
                    {
                        CedLimpActividades actividades = new CedLimpActividades();
                        actividades.fkCedula = folioGenerado;
                        actividades.fdFechaInci = r.fechaIncidencia;
                        actividades.fcTipo = r.tipoIncidencia;
                        actividades.fcArea = r.areaIncidencia;
                        actividades.fcComentarios = r.comentarios;
                        //actividades.fiPena = penas.penaRecoleccion[i];
                        context.Add(actividades);
                        context.SaveChanges();
                    }
                }

                if (cedula.boolUniforme == "false")
                {
                    cedulaaGuardar.fcPenaUniforme = 0;
                }
                else
                {
                    cedulaaGuardar.fcPenaUniforme = null;
                }
                context.SaveChanges();
                if (cedula.arregloEquipo != null)
                {
                    foreach (var e in cedula.arregloEquipo)
                    {
                        CedLimpEquipo equipo = new CedLimpEquipo();
                        equipo.fkCedula = folioGenerado;
                        equipo.fdFechaInci = e.fechaIncidencia;
                        equipo.fcMaquina = e.maquinaria;
                        equipo.fcComentarios = e.comentarios;
                        context.Add(equipo);
                        context.SaveChanges();
                    }
                }
            }
            return 1;
        }

        public string enviarLimpieza(CedulaLimpieza cedula, string jsonPenas, Entregables eL, string mes, int anio, string inmueble, int area, string folio, string usuario, string correo)
        {
            if (guardarLimpieza(cedula, eL, mes, anio, inmueble, area, folio, usuario) == 1)
            {
                PenasLimpieza penas;
                if (jsonPenas == null || jsonPenas == "")
                {
                    penas = new PenasLimpieza();
                }
                else
                {
                    penas = JsonConvert.DeserializeObject<PenasLimpieza>(jsonPenas);
                }

                var ahora = DateTime.Now;
                EvaluacionLimpieza cedulaGuardar;

                if (folio != null)
                {
                    cedulaGuardar = (from ultimaCed in context.EvaluacionLimpieza
                                     where ultimaCed.fcNoCed.Equals(folio)
                                     select ultimaCed).FirstOrDefault();
                }
                else
                {
                    cedulaGuardar = (from ultimaCed in context.EvaluacionLimpieza
                                     where ultimaCed.fcNoCed.Equals(folioFuera)
                                     select ultimaCed).FirstOrDefault();
                }

                cedulaGuardar.fiEstatus = null;

                cedulaGuardar.fdFechaEnviado = ahora;
                cedulaGuardar.fcPenaActividades = penas.penaActividades;
                if (penas.penaUniforme == 0)
                {
                    cedulaGuardar.fcPenaUniforme = null;
                }
                else
                {
                    cedulaGuardar.fcPenaUniforme = penas.penaUniforme;
                }
                cedulaGuardar.fcPenaMaterial = penas.penaEquipo;
                cedulaGuardar.fiCalificacion = penas.calificacion;
                cedulaGuardar.fiPenaCalificacion = penas.penaCalificacion;
                cedulaGuardar.fiPenaVisita = penas.penaVisita;
                cedulaGuardar.fiPenaPO = penas.penaPO;
                cedulaGuardar.fcCorreoUsuario = correo;
                cedulaGuardar.fcUsuario = usuario;
                context.Update(cedulaGuardar); ;
                context.SaveChanges();
                folio = cedulaGuardar.fcNoCed;
            }
            return folio;
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
