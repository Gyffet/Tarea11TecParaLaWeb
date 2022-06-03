using Formulario.Model;
using Formulario.Model.ModelExt.Mensajeria;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Formulario.Repositories
{
    public interface IMensajeriaRepository
    {
        public void InsertArchivo(string id, string fileName, string contentType, int lenght);
        public void aceptarMensajeria(string folio);
        public void rechazarMensajeria(string folio);
        public void eliminar(string folio);
        public string guardarMensajeria(CedulaMensajeria cedula, string jsonPenas, string mes, int anio, string inmueble, int area, string folio);
        public string enviarMensajeria(CedulaMensajeria cedula, string jsonPenas, string mes, int anio, string inmueble, int area, string folio, string usuario, string correo);
        public int validarPeriodo(string factura, string inmueble, string mes, string primavera);
    }
    public class MensajeriaRepository : IMensajeriaRepository
    {
        private readonly BD_HerramientasDGSGContext context;
        private readonly ICedulasRepository cedulasRep;

        public MensajeriaRepository(BD_HerramientasDGSGContext context, ICedulasRepository cedulasRep)
        {
            this.context = context;
            this.cedulasRep = cedulasRep;
        }

        public string folioFuera = "";

        public void InsertArchivo(string id, string fileName, string contentType, int lenght)
        {
            ArchivosCedulas archivo = new ArchivosCedulas();
            archivo.fkNoCed = id;
            archivo.fkServicio = 1;// 1 significa mensajeria
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
            var cedula = (from mensajeria in context.EvaluacionMensajeria
                                    where mensajeria.fcNoCed.Equals(folio)
                                    select mensajeria).FirstOrDefault();
            context.Remove(cedula);
            context.SaveChanges();

        }
        public void aceptarMensajeria(string folio)
        {
            var cedula = (from mensajeria in context.EvaluacionMensajeria
                          where mensajeria.fcNoCed.Equals(folio)
                          select mensajeria).FirstOrDefault();

            cedula.fiEstatus = 1;
            context.Update(cedula);
            context.SaveChanges();
        }

        public void rechazarMensajeria(string folio)
        {
            var cedula = (from mensajeria in context.EvaluacionMensajeria
                          where mensajeria.fcNoCed.Equals(folio)
                          select mensajeria).FirstOrDefault();

            cedula.fiEstatus = 0;
            context.Update(cedula);
            context.SaveChanges();
        }

        public string guardarMensajeria(CedulaMensajeria cedula, string jsonPenas, string mes, int anio, string inmueble, int area, string folio)
        {
            PenasMensajeria penas;
            if (jsonPenas == null || jsonPenas == "")
            {
                penas = new PenasMensajeria();
                penas.penaEntrega = new decimal?[cedula.arregloEntrega.Count];
                penas.penaAcuses = new decimal?[cedula.arregloAcuses.Count];
                penas.penaMalEstado = new decimal?[cedula.arregloMalEstado.Count];
                //penas.penaPerdido = new decimal?[cedula.arregloPerdido.Count];
                penas.penaPerdido = 0;
                penas.penaRecoleccion = new decimal?[cedula.arregloRecoleccion.Count];
                penas.penaPO = 0;
            }
            else
            {
                penas = JsonConvert.DeserializeObject<PenasMensajeria>(jsonPenas);
            }
            var hoy = DateTime.Now;
            if (folio != null)
            {
                var cedulaActualizable = context.EvaluacionMensajeria.Where(x => x.fcNoCed.Equals(folio)).FirstOrDefault(); ;
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
                cedulaActualizable.fdFechaGuardado = hoy;
                cedulaActualizable.fdFechaPO = cedula.fechaPO;
                context.Update(cedulaActualizable);
                context.SaveChanges();

                if (cedula.arregloRecoleccion != null)
                {
                    var arreglo = context.CedMenRecoleccion.Where(x => x.fkCedula.Equals(folio));
                    foreach (var a in arreglo)
                    {
                        context.Remove(a);
                    }
                    int i = 0;
                    foreach (var a in cedula.arregloRecoleccion)
                    {
                        CedMenRecoleccion recoleccion = new CedMenRecoleccion();
                        recoleccion.fkCedula = folio;
                        recoleccion.fdFechaProgramada = a.fechaProgramada;
                        recoleccion.fdFechaEfectiva = a.fechaEfectiva;
                        recoleccion.fcNoGuia = a.noGuiaRec;
                        recoleccion.fcCodRastreo = a.codRastreoRec;
                        recoleccion.fcTipoServicio = a.tipoServRec;
                        recoleccion.fiPena = penas.penaRecoleccion[i];
                        context.Add(recoleccion);
                        i++;
                        //context.SaveChanges();
                    }
                    context.SaveChanges();
                }

                if (cedula.arregloEntrega != null)
                {
                    var arreglo = context.CedMenEntrega.Where(x => x.fkCedula.Equals(folio));
                    foreach (var a in arreglo)
                    {
                        context.Remove(a);
                        //context.SaveChanges();
                    }
                    int i = 0;
                    foreach (var e in cedula.arregloEntrega)
                    {
                        CedMenEntrega entrega = new CedMenEntrega();
                        entrega.fkCedula = folio;
                        entrega.fdFechaProgramada = e.fechaProgramada;
                        entrega.fdFechaEfectiva = e.fechaEfectiva;
                        entrega.fcNoGuia = e.noGuiaRec;
                        entrega.fcCodRastreo = e.codRastreoRec;
                        entrega.fcTipoServicio = e.tipoServRec;
                        entrega.fiPena = penas.penaEntrega[i];
                        i++;
                        context.Add(entrega);
                        //context.SaveChanges();
                    }
                    context.SaveChanges();
                }

                if (cedula.arregloAcuses != null)
                {
                    var arreglo = context.CedMenAcuse.Where(x => x.fkCedula.Equals(folio));
                    foreach (var a in arreglo)
                    {
                        context.Remove(a);
                        //context.SaveChanges();
                    }
                    int i = 0;
                    foreach (var a in cedula.arregloAcuses)
                    {
                        CedMenAcuse acuse = new CedMenAcuse();
                        acuse.fkCedula = folio;
                        acuse.fdFechaEfectiva = a.fechaEfectiva;
                        acuse.fdFechaProgramada = a.fechaProgramada;
                        acuse.fiCantidadAcuses = a.cantidadAcuses;
                        acuse.fiPena = penas.penaAcuses[i];
                        context.Add(acuse);
                        i++;
                        //context.SaveChanges();
                    }
                    context.SaveChanges();
                }

                if (cedula.arregloMalEstado != null)
                {
                    var arreglo = context.CedMenMalEstado.Where(x => x.fkCedula.Equals(folio));
                    foreach (var a in arreglo)
                    {
                        context.Remove(a);
                    }
                    int i = 0;
                    foreach (var mE in cedula.arregloMalEstado)
                    {
                        CedMenMalEstado malEstado = new CedMenMalEstado();
                        malEstado.fkCedula = folio;
                        malEstado.fcNoGuia = mE.noGuiaRec;
                        malEstado.fcCodRastreo = mE.codRastreoRec;
                        malEstado.fcTipoServicio = mE.tipoServRec;
                        malEstado.fiPena = penas.penaMalEstado[i];
                        context.Add(malEstado);
                        i++;
                        //context.SaveChanges();
                    }
                    context.SaveChanges();
                }

                if (cedula.arregloPerdido != null)
                {
                    var arreglo = context.CedMenExtravio.Where(x => x.fkCedula.Equals(folio));
                    foreach (var a in arreglo)
                    {
                        context.Remove(a);
                    }
                    foreach (var p in cedula.arregloPerdido)
                    {
                        CedMenExtravio extravio = new CedMenExtravio();
                        extravio.fkCedula = folio;
                        extravio.fcNoGuia = p.noGuiaRec;
                        extravio.fcCodRastreo = p.codRastreoRec;
                        extravio.fcTipoServicio = p.tipoServRec;
                        context.Add(extravio);
                    }
                    context.SaveChanges();
                }

                if (cedula.boolMaterial == "false")
                {
                    var objeto = context.CedMenMaterial.Where(x => x.fkCedula.Equals(folio)).FirstOrDefault();
                    if (objeto != null)
                    {
                        context.Remove(objeto);
                    }
                    CedMenMaterial x = new CedMenMaterial();
                    x.fkCedula = folio;
                    x.fiMaterial = 1;
                    x.fiDiasMaterial = Int32.Parse(cedula.diasMaterial);
                    x.fiPenaMaterial = penas.penaMaterial;
                    
                    context.Add(x);
                    context.SaveChanges();
                }
            }

            else
            {
                var folioGenerado = cadenaAleatoria();
                folio = folioGenerado;
                var inmuebles = cedulasRep.getInmuebles(area);//Esta linea podría fallar despues de la migración estar al pendiente
                var claveInmueble = inmuebles.Select(x => x.Administracion).FirstOrDefault();

                EvaluacionMensajeria cedulaaGuardar = new EvaluacionMensajeria();
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
                cedulaaGuardar.fdFechaPO = cedula.fechaPO;
                cedulaaGuardar.fdFechaGuardado = hoy;
                cedulaaGuardar.fcServicio = "Mensajería";
                cedulaaGuardar.fiIDServicio = 1;
                context.Add(cedulaaGuardar);
                context.SaveChanges();


                if (cedula.arregloRecoleccion != null)
                {
                    int i = 0;
                    foreach (var r in cedula.arregloRecoleccion)
                    {
                        CedMenRecoleccion recoleccion = new CedMenRecoleccion();
                        recoleccion.fkCedula = folioGenerado;
                        recoleccion.fdFechaProgramada = r.fechaProgramada;
                        recoleccion.fdFechaEfectiva = r.fechaEfectiva;
                        recoleccion.fcNoGuia = r.noGuiaRec;
                        recoleccion.fcCodRastreo = r.codRastreoRec;
                        recoleccion.fcTipoServicio = r.tipoServRec;
                        recoleccion.fiPena = penas.penaRecoleccion[i];
                        i++;
                        context.Add(recoleccion);
                        context.SaveChanges();
                    }
                }

                if (cedula.arregloEntrega != null)
                {
                    int i = 0;
                    foreach (var e in cedula.arregloEntrega)
                    {
                        CedMenEntrega entrega = new CedMenEntrega();
                        entrega.fkCedula = folioGenerado;
                        entrega.fdFechaProgramada = e.fechaProgramada;
                        entrega.fdFechaEfectiva = e.fechaEfectiva;
                        entrega.fcNoGuia = e.noGuiaRec;
                        entrega.fcCodRastreo = e.codRastreoRec;
                        entrega.fcTipoServicio = e.tipoServRec;
                        entrega.fiPena = penas.penaEntrega[i];
                        i++;
                        context.Add(entrega);
                        context.SaveChanges();
                    }
                }

                if (cedula.arregloAcuses != null)
                {
                    int i = 0;
                    foreach (var a in cedula.arregloAcuses)
                    {
                        CedMenAcuse acuse = new CedMenAcuse();
                        acuse.fkCedula = folioGenerado;
                        acuse.fdFechaEfectiva = a.fechaEfectiva;
                        acuse.fdFechaProgramada = a.fechaProgramada;
                        acuse.fiCantidadAcuses = a.cantidadAcuses;
                        acuse.fiPena = penas.penaAcuses[i];
                        i++;
                        context.Add(acuse);
                        context.SaveChanges();
                    }
                }

                if (cedula.arregloMalEstado != null)
                {
                    int i = 0;
                    foreach (var a in cedula.arregloMalEstado)
                    {
                        CedMenMalEstado malEstado = new CedMenMalEstado();
                        malEstado.fkCedula = folioGenerado;
                        malEstado.fcNoGuia = a.noGuiaRec;
                        malEstado.fcCodRastreo = a.codRastreoRec;
                        malEstado.fcTipoServicio = a.tipoServRec;
                        malEstado.fiPena = penas.penaMalEstado[i];
                        i++;
                        context.Add(malEstado);
                        context.SaveChanges();
                    }
                }

                if (cedula.arregloPerdido != null)
                {
                    foreach (var a in cedula.arregloPerdido)
                    {
                        CedMenExtravio extravio = new CedMenExtravio();
                        extravio.fkCedula = folioGenerado;
                        extravio.fcNoGuia = a.noGuiaRec;
                        extravio.fcCodRastreo = a.codRastreoRec;
                        extravio.fcTipoServicio = a.tipoServRec;
                        context.Add(extravio);
                        context.SaveChanges();
                    }
                }

                if (cedula.boolMaterial == "false")
                {
                    CedMenMaterial x = new CedMenMaterial();
                    x.fkCedula = folioGenerado;
                    x.fiMaterial = 1;
                    x.fiDiasMaterial = Int32.Parse(cedula.diasMaterial);
                    x.fiPenaMaterial = penas.penaMaterial;
                    
                    context.Add(x);
                    context.SaveChanges();
                }
                
            }
            return folio;
        }

        public string enviarMensajeria(CedulaMensajeria cedula, string jsonPenas, string mes, int anio, string inmueble, int area, string folio, string usuario, string correo)
        {
            try
            {
                string folioFinal = guardarMensajeria(cedula, jsonPenas, mes, anio, inmueble, area, folio);
                PenasMensajeria penas;
                if (jsonPenas == null || jsonPenas == "")
                {
                    penas = new PenasMensajeria();
                }
                else
                {
                    penas = JsonConvert.DeserializeObject<PenasMensajeria>(jsonPenas);
                }

                var ahora = DateTime.Now;
                EvaluacionMensajeria cedulaGuardar;

                if (folio != null)
                {
                    cedulaGuardar = (from ultimaCed in context.EvaluacionMensajeria
                                     where ultimaCed.fcNoCed.Equals(folio)
                                     select ultimaCed).FirstOrDefault();
                }
                else
                {
                    cedulaGuardar = (from ultimaCed in context.EvaluacionMensajeria
                                     where ultimaCed.fcNoCed.Equals(folioFuera)
                                     select ultimaCed).FirstOrDefault();
                }

                cedulaGuardar.fdFechaEnviado = ahora;

                cedulaGuardar.fcPenaRecoleccion = 0;
                for (int i = 0; i < penas.penaRecoleccion.Length; i++)
                {
                    cedulaGuardar.fcPenaRecoleccion += penas.penaRecoleccion[i];
                }

                cedulaGuardar.fcPenaEntrega = 0;
                for (int i = 0; i < penas.penaEntrega.Length; i++)
                {
                    cedulaGuardar.fcPenaEntrega += penas.penaEntrega[i];
                }

                cedulaGuardar.fcPenaAcuses = 0;
                for (int i = 0; i < penas.penaAcuses.Length; i++)
                {
                    cedulaGuardar.fcPenaAcuses += penas.penaAcuses[i];
                }

                cedulaGuardar.fcPenaEstado = 0;
                for (int i = 0; i < penas.penaMalEstado.Length; i++)
                {
                    cedulaGuardar.fcPenaEstado += penas.penaMalEstado[i];
                }

                cedulaGuardar.fcPenaExtravio = 0;
                //for (int i = 0; i < penas.penaPerdido.Length; i++)
                //{
                //    cedulaGuardar.fcPenaExtravio += penas.penaPerdido[i];
                //}
                cedulaGuardar.fcPenaExtravio = penas.penaPerdido;
                cedulaGuardar.fcPenaMaterial = penas.penaMaterial;
                cedulaGuardar.fiCalificacion = penas.calificacion;
                cedulaGuardar.fiPenaCalificacion = penas.penaCalificacion;
                cedulaGuardar.fiPenaPO = penas.penaPO;
                cedulaGuardar.fcCorreoUsuario = correo;
                cedulaGuardar.fcUsuario = usuario;
                context.Update(cedulaGuardar); ;
                context.SaveChanges();
                return folioFinal;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            
        }

        public int validarPeriodo(string factura, string inmueble, string mes, string primavera)
        {
            if (factura == null || factura == "" || factura == " ")
            {
                factura = "Por Asignar";
            }
            int entero = Int32.Parse(primavera);
            int i = 0, j = 0, k = 0, l = 0;

            var dfewdfs = (from periodo in context.EvaluacionMensajeria select periodo).ToList();

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

            var validacion = (from periodo in context.EvaluacionMensajeria
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
