using Formulario.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Newtonsoft.Json;
//using Formulario.Model.ModelExt.Mensajeria;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;

namespace Formulario.Repositories
{
    public interface ICedulasRepository
    {
        public dynamic getTodas();
        public List<InmueblesCJF> getInmuebles(int area);
        public dynamic getCedulasPend(int area);
        public dynamic getCedula(string servicio, string folio);
        public dynamic getPendientes();
        public dynamic getAceptadas();
        public dynamic getRechazadas();
        public dynamic pendientesRechazadas(int area);
        public dynamic aceptadasAdmin(int area);
        
    }

    public class CedulasRepository : ICedulasRepository
    {
        private readonly BD_HerramientasDGSGContext context;

        public CedulasRepository(BD_HerramientasDGSGContext context)
        {
            this.context = context;
        }

        public dynamic getTodas()
        {
            dynamic cedulas = new ExpandoObject();

            cedulas.mensajeria = (from cedula in context.EvaluacionMensajeria
                                  
                                  select cedula).ToList();

            cedulas.limpieza = (from cedula in context.EvaluacionLimpieza
                                
                                select cedula).ToList();

            cedulas.fumigacion = (from cedula in context.EvaluacionFumigacion

                                  select cedula).ToList();

            return cedulas;
        }

        public dynamic getAceptadas()
        {
            dynamic cedulas = new ExpandoObject();

            cedulas.mensajeria = (from cedula in context.EvaluacionMensajeria
                                  where cedula.fdFechaEnviado != null && cedula.fiEstatus == 1
                                  select cedula).ToList();

            cedulas.limpieza = (from cedula in context.EvaluacionLimpieza
                                where cedula.fdFechaEnviado != null && cedula.fiEstatus == 1
                                select cedula).ToList();

            cedulas.fumigacion = (from cedula in context.EvaluacionFumigacion
                                  where cedula.fdFechaEnviado != null && cedula.fiEstatus == 1
                                  select cedula).ToList();

            return cedulas;
        }

      

        public dynamic getRechazadas()
        {
            dynamic cedulas = new ExpandoObject();

            cedulas.mensajeria = (from cedula in context.EvaluacionMensajeria
                                  where cedula.fiEstatus == 0
                                  select cedula).ToList();

            cedulas.limpieza = (from cedula in context.EvaluacionLimpieza
                                where cedula.fiEstatus == 0
                                select cedula).ToList();

            cedulas.fumigacion = (from cedula in context.EvaluacionFumigacion
                                  where cedula.fiEstatus == 0
                                  select cedula).ToList();

            return cedulas;
        }

        public dynamic getPendientes()
        {
            dynamic cedulas = new ExpandoObject();

            var mensajeriaFDB = context.EvaluacionMensajeria
                .Where(x => x.fdFechaEnviado != null && x.fiEstatus == null).Select(x => new
                {
                    Periodo = x.fcMes,
                    Inmueble=x.fcInmueble,
                    Factura=x.fcNumFactura,
                    Folio=x.fcNoCed,
                    Monto= FormattedAmount(x.fiMontoFactura),
                }).ToList();

            cedulas.mensajeria = mensajeriaFDB;

            var limpiezaFDB = context.EvaluacionLimpieza
                .Where(x => x.fdFechaEnviado != null && x.fiEstatus == null).Select(x => new
                {
                    Periodo = x.fcMes,
                    Inmueble = x.fcInmueble,
                    Factura = x.fcNumFactura,
                    Folio = x.fcNoCed,
                    Monto = FormattedAmount(x.fiMontoFactura),
                }).ToList();

            cedulas.limpieza = limpiezaFDB;

            var fumigacionFDB = context.EvaluacionFumigacion
                .Where(x => x.fdFechaEnviado != null && x.fiEstatus == null).Select(x => new
                {
                    Periodo = x.fcMes,
                    Inmueble = x.fcInmueble,
                    Factura = x.fcNumFactura,
                    Folio = x.fcNoCed,
                    Monto = FormattedAmount(x.fiMontoFactura),
                }).ToList();

            cedulas.fumigacion = fumigacionFDB;
            return cedulas;
        }

        public dynamic getCedula(string servicio, string folio)
        {
            dynamic cedulass = new ExpandoObject();
            switch (servicio)
            {
                case "Mensajeria":
                    dynamic arreglos = new ExpandoObject();
                    var mensajeria = (from cedula in context.EvaluacionMensajeria
                                  where cedula.fcNoCed.Equals(folio)
                                  select cedula).FirstOrDefault();
                    arreglos.arregloReco = (from cedula in context.CedMenRecoleccion
                                    where cedula.fkCedula.Equals(folio)
                                    select cedula).ToList();
                    arreglos.arregloEntre = (from cedula in context.CedMenEntrega
                                        where cedula.fkCedula.Equals(folio)
                                        select cedula).ToList();
                    arreglos.arregloAcuse = (from cedula in context.CedMenAcuse
                                        where cedula.fkCedula.Equals(folio)
                                        select cedula).ToList();
                    arreglos.arregloExtravio = (from cedula in context.CedMenExtravio
                                           where cedula.fkCedula.Equals(folio)
                                           select cedula).ToList();
                    arreglos.arregloMalEstado = (from cedula in context.CedMenMalEstado
                                            where cedula.fkCedula.Equals(folio)
                                            select cedula).ToList();
                    arreglos.objetoMaterialUniforme = (from cedula in context.CedMenMaterial
                                          where cedula.fkCedula.Equals(folio)
                                          select cedula).ToList();

                    var documentoMen = (from doc in context.ArchivosCedulas
                                        where doc.fkNoCed.Equals(folio) && doc.fkServicio.Equals(1)
                                        select doc).FirstOrDefault();


                    cedulass.cedula = mensajeria;
                    cedulass.arreglos = arreglos;
                    cedulass.documentos = documentoMen;
                    break;

                case "Fumigacion":
                    dynamic arregloFumigacion = new ExpandoObject();
                    var fumigacion = (from cedula in context.EvaluacionFumigacion
                                      where cedula.fcNoCed.Equals(folio)
                                      select cedula).FirstOrDefault();
                    arregloFumigacion.arregloFechas = (from cedula in context.CedFumFechas
                                                       where cedula.fkCedula.Equals(folio)
                                                       select cedula).ToList();
                    arregloFumigacion.arregloFumigacion = (from cedula in context.CedFumFumigacion
                                                 where cedula.fkCedula.Equals(folio)
                                                 select cedula).ToList();

                    arregloFumigacion.arregloHoras = (from cedula in context.CedFumHoras
                                                      where cedula.fkCedula.Equals(folio)
                                                      select cedula).ToList();

                    var entregablesFumigacion = (from entregable in context.EntregablesFumigacion
                                                 where entregable.fkCedula.Equals(folio)
                                                 select entregable).FirstOrDefault();

                    var documentoFum = (from doc in context.ArchivosCedulas
                                        where doc.fkNoCed.Equals(folio) && doc.fkServicio.Equals(3)
                                        select doc).FirstOrDefault();

                    cedulass.cedula = fumigacion;
                    cedulass.arreglos = arregloFumigacion;
                    cedulass.entregable = entregablesFumigacion;
                    cedulass.documentos = documentoFum;
                    break;

                case "Limpieza":
                    dynamic arregloLimpieza = new ExpandoObject();
                    var limpieza = (from cedula in context.EvaluacionLimpieza
                                      where cedula.fcNoCed.Equals(folio)
                                      select cedula).FirstOrDefault();
                    arregloLimpieza.arregloActi = (from cedula in context.CedLimpActividades
                                            where cedula.fkCedula.Equals(folio)
                                            select cedula).ToList();
                    arregloLimpieza.arregloEquipo = (from cedula in context.CedLimpEquipo
                                             where cedula.fkCedula.Equals(folio)
                                             select cedula).ToList();
                    var entregables = (from entregable in context.EntregablesLimpieza
                                       where entregable.fkCedula.Equals(folio)
                                       select entregable).FirstOrDefault();

                    var documento = (from doc in context.ArchivosCedulas
                                     where doc.fkNoCed.Equals(folio) && doc.fkServicio.Equals(2)
                                     select doc).FirstOrDefault();

                    cedulass.cedula = limpieza;
                    cedulass.arreglos = arregloLimpieza;
                    cedulass.entregable = entregables;
                    cedulass.documentos = documento;
                    break;
            }
            return cedulass;
        }

        public List<InmueblesCJF> getInmuebles(int area)
        {
            var resultadoArea = (from areaI in context.CedulasAreaInmueble
                                 where areaI.fiClaveArea.Equals(area)
                                 select areaI).FirstOrDefault();


            var resultadoInmueble = (from inmueble in context.InmueblesCJF
                                     where inmueble.Clave.Equals(resultadoArea.fcClaveInmueble.ToString())
                                     select inmueble).FirstOrDefault();

            var inmueblesRelacionados = (from inmueble in context.InmueblesCJF
                                         where inmueble.Administracion.Equals(resultadoInmueble.Administracion)
                                         select inmueble).ToList();
                                       

            return inmueblesRelacionados;
        }

        public dynamic pendientesRechazadas(int area)
        {
            var resultadoArea = (from areaI in context.CedulasAreaInmueble
                                 where areaI.fiClaveArea.Equals(area)
                                 select areaI).FirstOrDefault();


            var resultadoInmueble = (from inmueble in context.InmueblesCJF
                                     where inmueble.Clave.Equals(resultadoArea.fcClaveInmueble.ToString())
                                     select inmueble).FirstOrDefault();

            var mensajeriaPendientes = (from cedula in context.EvaluacionMensajeria
                                        where cedula.fcAdministracion.Equals(resultadoInmueble.Administracion)
                                        && cedula.fdFechaEnviado != null && cedula.fiEstatus == 0
                                        select cedula).ToList();

            //Atencion a mensajeriaPendientes, podría tronar en la sentencia FechaEnviado != null
            //Los demas son == null. Seguramente en algun otro lado del código mando fechaEnviado a null
            //lo que me permite buscarlo en limpieza y fumigacion de esa manera

            var limpiezaPendientes = (from cedula in context.EvaluacionLimpieza
                                      where cedula.fcAdministracion.Equals(resultadoInmueble.Administracion)
                                      && cedula.fdFechaEnviado == null && cedula.fiEstatus == 0
                                      select cedula).ToList();

            var fumigacionPendientes = (from cedula in context.EvaluacionFumigacion
                                      where cedula.fcAdministracion.Equals(resultadoInmueble.Administracion)
                                      && cedula.fdFechaEnviado == null && cedula.fiEstatus == 0
                                      select cedula).ToList();

            dynamic cedulas = new ExpandoObject();

            cedulas.Mensajeria = mensajeriaPendientes;
            cedulas.Limpieza = limpiezaPendientes;
            cedulas.Fumigacion = fumigacionPendientes;

            return cedulas;
        }

        public dynamic aceptadasAdmin(int area)
        {
            var resultadoArea = (from areaI in context.CedulasAreaInmueble
                                 where areaI.fiClaveArea.Equals(area)
                                 select areaI).FirstOrDefault();


            var resultadoInmueble = (from inmueble in context.InmueblesCJF
                                     where inmueble.Clave.Equals(resultadoArea.fcClaveInmueble.ToString())
                                     select inmueble).FirstOrDefault();

            var limpieza = (from cedula in context.EvaluacionLimpieza
                            where cedula.fcAdministracion.Equals(resultadoInmueble.Administracion)
                            && cedula.fdFechaEnviado != null && cedula.fiEstatus == 1
                            select cedula).ToList();

            var mensajeria = (from cedula in context.EvaluacionMensajeria
                              where cedula.fcAdministracion.Equals(resultadoInmueble.Administracion)
                              && cedula.fdFechaEnviado != null && cedula.fiEstatus == 1
                              select cedula).ToList();

            var fumigacion = (from cedula in context.EvaluacionFumigacion
                              where cedula.fcAdministracion.Equals(resultadoInmueble.Administracion)
                              && cedula.fdFechaEnviado != null && cedula.fiEstatus == 1
                              select cedula).ToList();

            dynamic cedulas = new ExpandoObject();

            cedulas.Limpieza = limpieza;
            cedulas.Mensajeria = mensajeria;
            cedulas.Fumigacion = fumigacion;

            return cedulas;
        }

        public dynamic getCedulasPend(int area)
        {
            var resultadoArea = (from areaI in context.CedulasAreaInmueble
                                 where areaI.fiClaveArea.Equals(area)
                                 select areaI).FirstOrDefault();


            var resultadoInmueble = (from inmueble in context.InmueblesCJF
                                     where inmueble.Clave.Equals(resultadoArea.fcClaveInmueble.ToString())
                                     select inmueble).FirstOrDefault();

            var mensajeriaPendientes = (from cedula in context.EvaluacionMensajeria
                                        where cedula.fcAdministracion.Equals(resultadoInmueble.Administracion)
                                        && cedula.fdFechaEnviado == null && cedula.fiEstatus == null
                                        select cedula).OrderBy(x => x.fcInmueble).ThenBy(x => x.fcMes).ToList();


            var limpiezaPendientes = (from cedula in context.EvaluacionLimpieza
                                      where cedula.fcAdministracion.Equals(resultadoInmueble.Administracion)
                                      && cedula.fdFechaEnviado == null && cedula.fiEstatus == null
                                      select cedula).OrderBy(x => x.fcInmueble).ThenBy(x => x.fcMes).ToList();


            var fumigacionPendientes = (from cedula in context.EvaluacionFumigacion
                                        where cedula.fcAdministracion.Equals(resultadoInmueble.Administracion)
                                        && cedula.fdFechaEnviado == null && cedula.fiEstatus == null
                                        select cedula).OrderBy(x => x.fcInmueble).ThenBy(x => x.fcMes).ToList();

            dynamic cedulas = new ExpandoObject();

            cedulas.Mensajeria = mensajeriaPendientes;
            cedulas.Limpieza = limpiezaPendientes;
            cedulas.Fumigacion = fumigacionPendientes;

            return cedulas;
        }

        public static string FormattedAmount(decimal monto)
        {
            return string.Format("{0:C}", monto);
        }
    }
}
