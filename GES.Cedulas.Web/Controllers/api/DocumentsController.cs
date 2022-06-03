using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Spire.Doc;
using System.Drawing;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Formulario.Model;
using Formulario.Repositories;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Formulario.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IHostingEnvironment environment;
        private readonly ICedulasRepository repository;
        private readonly BD_HerramientasDGSGContext context;

        public DocumentsController(IHostingEnvironment environment, ICedulasRepository repository, BD_HerramientasDGSGContext context)
        {
            this.environment = environment;
            this.repository = repository;
            this.context = context;
        }

        
        [Route("DescargarArchivo/{nombre}")]
        public IActionResult Archivo(string nombre)
        {
            string folderName = "Docs";
            string webRootPath = environment.ContentRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            string pathArchivo = Path.Combine(newPath, nombre);
            
            if (System.IO.File.Exists(pathArchivo))
            {
                Stream stream = System.IO.File.Open(pathArchivo, FileMode.Open);

                return File(stream, "application/pdf", nombre);
            }
            return NotFound();
        }

        [Route("ArchivoCedula/{nombre}")]
        public IActionResult ArchivoCed(string nombre)
        {
            string folderName = "FacturasCedulas";
            string webRootPath = environment.ContentRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            string pathArchivo = Path.Combine(newPath, nombre);

            if (System.IO.File.Exists(pathArchivo))
            {
                Stream stream = System.IO.File.Open(pathArchivo, FileMode.Open);

                return File(stream, "application/pdf", nombre);
            }
            return NotFound();
        }

        [Route("GenerarReporteLimpieza/{folio}")]
        public IActionResult ReporteLimpieza(string folio)
        {
            var cedFromRep = repository.getCedula("Limpieza", folio);

            EvaluacionLimpieza cedula = cedFromRep.cedula;
            EntregablesLimpieza entregable = cedFromRep.entregable;
            CedLimpActividades[] actividades = new CedLimpActividades[cedFromRep.arreglos.arregloActi.Count];
            CedLimpEquipo[] equipo = new CedLimpEquipo[cedFromRep.arreglos.arregloEquipo.Count];
            
            for (int i = 0; i < actividades.Length; i++)
            {
                actividades[i] = cedFromRep.arreglos.arregloActi[i];
            }

            for (int i = 0; i < equipo.Length; i++)
            {
                equipo[i] = cedFromRep.arreglos.arregloEquipo[i];
            }

            //Jalar documento con marcas
            Document document = new Document();
            //var path = @"d:\Usuarios\oiguerrero\Source\Repos\GESCore-master\Formulario\Docs\RepLimp.docx";
            var path = @"e:\sitios\Cedulas\Docs\RepLimp.docx";
            document.LoadFromFile(path);

            
            //Crear Tabla
            //Section tablas = document.AddSection();

            if (actividades.Length > 0)
            {
                Section seccionActividades = document.AddSection();
                Table tablaActividades = seccionActividades.AddTable(true);

                String[] cabeceraActividades = { "Tipo", "Área", "Fecha Incidencia" };
                var datosActividades = actividades.Select(w => new String[] { w.fcTipo, w.fcArea, w.fdFechaInci.GetValueOrDefault().ToString("dd/MM/yyy") }).ToArray();

                tablaActividades.ResetCells(datosActividades.Length + 1, cabeceraActividades.Length);

                TableRow recRow = tablaActividades.Rows[0];
                recRow.IsHeader = true;
                recRow.Height = 8;

                recRow.RowFormat.BackColor = Color.Black;

                for (int i = 0; i < cabeceraActividades.Length; i++)
                {
                    //Alineacion de celdas
                    Paragraph p = recRow.Cells[i].AddParagraph();
                    recRow.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;

                    //Formato de datos
                    TextRange TR = p.AppendText(cabeceraActividades[i]);
                    TR.CharacterFormat.FontName = "Montserrat";
                    TR.CharacterFormat.FontSize = 8;
                    TR.CharacterFormat.TextColor = Color.White;
                }

                for (int r = 0; r < datosActividades.Length; r++)
                {
                    TableRow DataRow = tablaActividades.Rows[r + 1];
                    //Fila Height
                    DataRow.Height = 5;

                    //Columnas
                    for (int c = 0; c < datosActividades[r].Length; c++)
                    {
                        //Alineacion de Celdas
                        DataRow.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                        //Llenar datos en filas
                        Paragraph p2 = DataRow.Cells[c].AddParagraph();
                        TextRange TR2 = p2.AppendText(datosActividades[r][c]);
                        //Formato de celdas
                        p2.Format.HorizontalAlignment = HorizontalAlignment.Center;
                        TR2.CharacterFormat.FontName = "Montserrat";
                        TR2.CharacterFormat.FontSize = 8;
                    }
                }

                BookmarksNavigator marcaActividades = new BookmarksNavigator(document);
                marcaActividades.MoveToBookmark("ProgramaOP", true, true);
                marcaActividades.InsertTable(tablaActividades);
                document.Sections.Remove(seccionActividades);
                document.Replace("||ProgramaOP||", "", false, true);
            }
            else
                document.Replace("||ProgramaOP||", "Sin Incidencias en el mes.", false, true);

            if (equipo.Length > 0)
            {
                Section seccionEquipo = document.AddSection();
                Table tablaEquipo = seccionEquipo.AddTable(true);

                String[] caberaEquipo = { "Maquina/Equipo", "Fecha Incidencia" };
                var datosEquipo = equipo.Select(w => new String[] { w.fcMaquina, w.fdFechaInci.GetValueOrDefault().ToString("dd/MM/yyy") }).ToArray();

                tablaEquipo.ResetCells(datosEquipo.Length + 1, caberaEquipo.Length);

                TableRow entRow = tablaEquipo.Rows[0];
                entRow.IsHeader = true;
                entRow.Height = 8;
                entRow.RowFormat.BackColor = Color.Black;

                for (int i = 0; i < caberaEquipo.Length; i++)
                {
                    //Alineacion de celdas
                    Paragraph p = entRow.Cells[i].AddParagraph();
                    entRow.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;

                    //Formato de datos
                    TextRange TR = p.AppendText(caberaEquipo[i]);
                    TR.CharacterFormat.FontName = "Montserrat";
                    TR.CharacterFormat.FontSize = 8;
                    TR.CharacterFormat.TextColor = Color.White;
                }

                for (int r = 0; r < datosEquipo.Length; r++)
                {
                    TableRow DataRow = tablaEquipo.Rows[r + 1];
                    //Fila Height
                    DataRow.Height = 5;

                    //Columnas
                    for (int c = 0; c < datosEquipo[r].Length; c++)
                    {
                        //Alineacion de Celdas
                        DataRow.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                        //Llenar datos en filas
                        Paragraph p2 = DataRow.Cells[c].AddParagraph();
                        TextRange TR2 = p2.AppendText(datosEquipo[r][c]);
                        //Formato de celdas
                        p2.Format.HorizontalAlignment = HorizontalAlignment.Center;
                        TR2.CharacterFormat.FontName = "Montserrat";
                        TR2.CharacterFormat.FontSize = 8;
                    }
                }

                BookmarksNavigator marcaEquipo = new BookmarksNavigator(document);
                marcaEquipo.MoveToBookmark("Maquinaria", true, true);
                marcaEquipo.InsertTable(tablaEquipo);
                document.Sections.Remove(seccionEquipo);
                document.Replace("||Maquinaria||", "", false, true);
            }
            else
                document.Replace("||Maquinaria||", "Sin Incidencias en el mes.", false, true);

            if (cedula.fcPenaUniforme == null)
            {
                document.Replace("||Uniforme||", "Sin incidencias en el mes.", false, true);
                
            }
            else
            {
                document.Replace("||Uniforme||", "El personal del proveedor no cumplió con los requisitos de identidad (uniforme y credencial)", false, true);
            }

            if (cedula.fiInasistencias > 0)
            {
                document.Replace("||Inasistencias||", "Hubo " + cedula.fiInasistencias + " en el periodo", false, true);
            }
            else
            {
                document.Replace("||Inasistencias||", "No hubo inasistencias en el periodo", false, true);
            }


            if (entregable.fdCierreMes == null)
            {
                document.Replace("||FechaC||", "Fecha de cierre por definir", false, true);
            }
            else
            {
                document.Replace("||FechaC||", "Fecha de cierre establecida el " + entregable.fdCierreMes.GetValueOrDefault().ToString("dd/MM/yyy"), false, true);
            }

            if (entregable.fdSUAIMSS == null)
            {
                document.Replace("||FechaIMSS||", "No lo ha entregado", false, true);
            }
            else
            {
                document.Replace("||FechaIMSS||", "Fecha de entrega: " + entregable.fdSUAIMSS.GetValueOrDefault().ToString("dd/MM/yyy"), false, true);
            }

            if (entregable.fdActaServicios == null)
            {
                document.Replace("||FechaActa||", "No lo ha entregado", false, true);
            }
            else
            {
                document.Replace("||FechaActa||", "Fecha de entrega: " + entregable.fdActaServicios.GetValueOrDefault().ToString("dd/MM/yyy"), false, true);
            }

            document.Replace("||C||", cedula.fiCalificacion.ToString(), false, true);
            
            DateTime fecha;
            if (cedula.fdFechaEnviado == null)
            {
                fecha = (DateTime)cedula.fdFechaGuardado;
            }
            else
            {
                fecha = (DateTime)cedula.fdFechaEnviado;
            }
            document.Replace("||Mes||", cedula.fcMes + " " + cedula.fiAnio.ToString(), false, true);
            document.Replace("||Fecha||", fecha.Day.ToString() + "-" + intaMes(fecha.Month) + "-" + fecha.Year.ToString(), false, true);
            document.Replace("||Factura||", cedula.fcNumFactura, false, true);
            document.Replace("||Monto||", cedula.fiMontoFactura.ToString("C"), false, true);
            document.Replace("||F||", cedula.fcNoCed, false, true);
            if (cedula.fcUsuario == null)
            {
                document.Replace("||E||", "Por designar", false, true);
            }
            else
            {
                document.Replace("||E||", cedula.fcUsuario, false, true);
            }

            var administracion = (from admin in context.InmueblesCJF
                                  where admin.Administracion.Equals(cedula.fcAdministracion)
                                  select admin).FirstOrDefault();

            document.Replace("||Administracion||", "Administración: " + administracion.Nombre
                + "; Inmueble: " + cedula.fcInmueble, false, true);
            document.Replace("||Usuario||", cedula.fcUsuario, false, true);

            if (cedula.fcMes == "Enero")
            {
                string A, B, C, D;
                if (entregable.fdVisitaInstalaciones == null)
                {
                    A = "No se presentó";
                }
                else
                {
                    A = "Se presentó el " + entregable.fdVisitaInstalaciones.GetValueOrDefault().ToString("dd/MM/yyy");
                }

                if (entregable.fdProgramaOperacion == null)
                {
                    B = "No se entregó";
                }
                else
                {
                    B = "Se presentó el " + entregable.fdProgramaOperacion.GetValueOrDefault().ToString("dd/MM/yyy");
                }

                if (entregable.fdGafeteUniforme == null)
                {
                    C = "No se entregaron";
                }
                else
                {
                    C = "Se presentó el " + entregable.fdGafeteUniforme.GetValueOrDefault().ToString("dd/MM/yyy");
                }

                if (entregable.fdActaInicio == null)
                {
                    D = "No se entregó";
                }
                else
                {
                    D = "Se presentó el " + entregable.fdActaInicio.GetValueOrDefault().ToString("dd/MM/yyy");
                }

                Paragraph paraInserted = new Paragraph(document);
                TextRange textRange = paraInserted.AppendText("\n" + "A. ¿Cúando se presentó el prestador del servicio a realizar la visita a las instalaciones?\n" +
                A + "\n" +
                "B. ¿Cúando entregó el prestador del servicio el Programa de Operación?\n" +
                B + "\n" +
                "C. ¿Cúando entregó el prestador del servicio los uniformes y gafetes a su personal?\n" +
                C + "\n" +
                "D. ¿Cúando entregó el prestador del servicio el Acta de Inicio del servicio?\n" +
                D + "\n");
                textRange.CharacterFormat.FontSize = 8;
                textRange.CharacterFormat.FontName = "Montserrat";
                textRange.CharacterFormat.Bold = true;

                Section section = document.Sections[0];
                TextSelection selection = document.FindString("||PrimerMes||", true, true);
                TextRange range = selection.GetAsOneRange();
                Paragraph paragraph = range.OwnerParagraph;
                Body body = paragraph.OwnerTextBody;
                int index = body.ChildObjects.IndexOf(paragraph);
                body.ChildObjects.Remove(paragraph);
                body.ChildObjects.Insert(index, paraInserted);
            }
            else
            {
                document.Replace("||PrimerMes||", "", false, true);
            }

            //Salvar y Lanzar

            byte[] toArray = null;
            using (MemoryStream ms1 = new MemoryStream())
            {
                document.SaveToStream(ms1, Spire.Doc.FileFormat.PDF);
                toArray = ms1.ToArray();
            }
            return File(toArray, "application/pdf", "ReporteLimpieza_" + cedula.fcInmueble + cedula.fcMes + ".pdf");
        }

        [Route("GenerarReporteMensajeria/{folio}")]
        public IActionResult ReporteMensajeria(string folio)
        {
            //var licenseFile = new FileInfo(@"d:\Usuarios\oiguerrero\Downloads\BIN\netcoreapp3.0\Spire.License.dll");
            //Spire.License.LicenseProvider.SetLicenseFile(licenseFile);

            ////Tell the license module that you changed the license file name.
            //Spire.License.LicenseProvider.SetLicenseFileName("Spire.NewLicense.dll");






            //            N9Q9EUUVTICI
            //8XJFSX0F9OQD
            //K0BQKPJJC3T9
            //RSB3HQA9DV71
            //Y35MTYN2CYLR


            //Spire.License.LicenseProvider.LoadLicense();






            var cedFromRep = repository.getCedula("Mensajeria", folio);

            EvaluacionMensajeria cedula = cedFromRep.cedula;
            CedMenRecoleccion[] recoleccion = new CedMenRecoleccion[cedFromRep.arreglos.arregloReco.Count];
            CedMenEntrega[] entrega = new CedMenEntrega[cedFromRep.arreglos.arregloEntre.Count];
            CedMenAcuse[] acuse = new CedMenAcuse[cedFromRep.arreglos.arregloAcuse.Count];
            CedMenExtravio[] extravio = new CedMenExtravio[cedFromRep.arreglos.arregloExtravio.Count];
            CedMenMalEstado[] malEstado = new CedMenMalEstado[cedFromRep.arreglos.arregloMalEstado.Count];
            CedMenMaterial[] material = new CedMenMaterial[cedFromRep.arreglos.objetoMaterialUniforme.Count];

            for (int i = 0; i < recoleccion.Length; i++)
            {
                recoleccion[i] = cedFromRep.arreglos.arregloReco[i];
            }

            for (int i = 0; i < entrega.Length; i++)
            {
                entrega[i] = cedFromRep.arreglos.arregloEntre[i];
            }

            for (int i = 0; i < acuse.Length; i++)
            {
                acuse[i] = cedFromRep.arreglos.arregloAcuse[i];
            }

            for (int i = 0; i < extravio.Length; i++)
            {
                extravio[i] = cedFromRep.arreglos.arregloExtravio[i];
            }

            for (int i = 0; i < malEstado.Length; i++)
            {
                malEstado[i] = cedFromRep.arreglos.arregloMalEstado[i];
            }

            for (int i = 0; i < material.Length; i++)
            {
                material[i] = cedFromRep.arreglos.objetoMaterialUniforme[i];
            }

            

            //Jalar documento con marcas
            Document document = new Document();
            //var path = @"d:\Usuarios\oiguerrero\Source\Repos\GESCore\Formulario\Docs\ReporteMensualMensajeria.docx";
            var path = @"e:\sitios\Cedulas\Docs\RepMensa.docx";
            document.LoadFromFile(path);

            //Crear Tabla
            Section tablas = document.AddSection();

            if (recoleccion.Length > 0)
            {
                Table tablaRecoleccion = tablas.AddTable(true);

                String[] cabeceraRecoleccion = { "Número de Guía", "Código de Rastreo", "Fecha Programada", "Fecha Efectiva", "Tipo de Servicio" };
                var datosRecoleccion = recoleccion.Select(w => new String[] { w.fcNoGuia, w.fcCodRastreo, w.fdFechaProgramada.GetValueOrDefault().ToString("dd/MM/yyy"), w.fdFechaEfectiva.GetValueOrDefault().ToString("dd/MM/yyy"), w.fcTipoServicio }).ToArray();

                tablaRecoleccion.ResetCells(datosRecoleccion.Length + 1, cabeceraRecoleccion.Length);

                TableRow recRow = tablaRecoleccion.Rows[0];
                recRow.IsHeader = true;
                recRow.Height = 8;

                recRow.RowFormat.BackColor = Color.Black;

                for (int i = 0; i < cabeceraRecoleccion.Length; i++)
                {
                    //Alineacion de celdas
                    Paragraph p = recRow.Cells[i].AddParagraph();
                    recRow.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;

                    //Formato de datos
                    TextRange TR = p.AppendText(cabeceraRecoleccion[i]);
                    TR.CharacterFormat.FontName = "Montserrat";
                    TR.CharacterFormat.FontSize = 8;
                    TR.CharacterFormat.TextColor = Color.White;
                }

                for (int r = 0; r < datosRecoleccion.Length; r++)
                {
                    TableRow DataRow = tablaRecoleccion.Rows[r + 1];
                    //Fila Height
                    DataRow.Height = 5;


                    //Columnas
                    for (int c = 0; c < datosRecoleccion[r].Length; c++)
                    {
                        //Alineacion de Celdas
                        DataRow.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                        //Llenar datos en filas
                        Paragraph p2 = DataRow.Cells[c].AddParagraph();
                        TextRange TR2 = p2.AppendText(datosRecoleccion[r][c]);
                        //Formato de celdas
                        p2.Format.HorizontalAlignment = HorizontalAlignment.Center;
                        TR2.CharacterFormat.FontName = "Montserrat";
                        TR2.CharacterFormat.FontSize = 8;
                    }
                }

                BookmarksNavigator marcaRecoleccion = new BookmarksNavigator(document);
                marcaRecoleccion.MoveToBookmark("Recoleccion", true, true);
                marcaRecoleccion.InsertTable(tablaRecoleccion);
                document.Replace("||Recoleccion||", "", false, true);
            }
            else
                document.Replace("||Recoleccion||", "Sin Incidencias en el mes.", false, true);

            if (entrega.Length > 0)
            {
                Table tablaEntrega = tablas.AddTable(true);

                String[] cabeceraEntrega = { "Número de Guía", "Código de Rastreo", "Fecha Programada", "Fecha Efectiva", "Tipo de Servicio" };
                var datosEntrega = entrega.Select(w => new String[] { w.fcNoGuia, w.fcCodRastreo, w.fdFechaProgramada.GetValueOrDefault().ToString("dd/MM/yyy"), w.fdFechaEfectiva.GetValueOrDefault().ToString("dd/MM/yyy"), w.fcTipoServicio }).ToArray();

                tablaEntrega.ResetCells(datosEntrega.Length + 1, cabeceraEntrega.Length);

                TableRow entRow = tablaEntrega.Rows[0];
                entRow.IsHeader = true;
                entRow.Height = 8;
                entRow.RowFormat.BackColor = Color.Black;

                for (int i = 0; i < cabeceraEntrega.Length; i++)
                {
                    //Alineacion de celdas
                    Paragraph p = entRow.Cells[i].AddParagraph();
                    entRow.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;

                    //Formato de datos
                    TextRange TR = p.AppendText(cabeceraEntrega[i]);
                    TR.CharacterFormat.FontName = "Montserrat";
                    TR.CharacterFormat.FontSize = 8;
                    TR.CharacterFormat.TextColor = Color.White;
                }

                for (int r = 0; r < datosEntrega.Length; r++)
                {
                    TableRow DataRow = tablaEntrega.Rows[r + 1];
                    //Fila Height
                    DataRow.Height = 5;

                    //Columnas
                    for (int c = 0; c < datosEntrega[r].Length; c++)
                    {
                        //Alineacion de Celdas
                        DataRow.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                        //Llenar datos en filas
                        Paragraph p2 = DataRow.Cells[c].AddParagraph();
                        TextRange TR2 = p2.AppendText(datosEntrega[r][c]);
                        //Formato de celdas
                        p2.Format.HorizontalAlignment = HorizontalAlignment.Center;
                        TR2.CharacterFormat.FontName = "Montserrat";
                        TR2.CharacterFormat.FontSize = 8;
                    }
                }

                BookmarksNavigator marcaEntrega = new BookmarksNavigator(document);
                marcaEntrega.MoveToBookmark("Entrega", true, true);



                marcaEntrega.InsertTable(tablaEntrega);
                document.Replace("||Entrega||", "", false, true);
            }
            else
                document.Replace("||Entrega||", "Sin Incidencias en el mes.", false, true);

            if (acuse.Length > 0)
            {
                Table tablaAcuse = tablas.AddTable(true);

                String[] cabeceraAcuse = { "Acuses Faltantes", "Fecha Programada", "Fecha Efectiva" };
                var datosAcuse = acuse.Select(w => new String[] { w.fiCantidadAcuses.ToString(), w.fdFechaProgramada.GetValueOrDefault().ToString("dd/MM/yyy"), w.fdFechaEfectiva.GetValueOrDefault().ToString("dd/MM/yyy") }).ToArray();

                tablaAcuse.ResetCells(datosAcuse.Length + 1, cabeceraAcuse.Length);

                TableRow acuRow = tablaAcuse.Rows[0];
                acuRow.IsHeader = true;
                acuRow.Height = 8;
                acuRow.RowFormat.BackColor = Color.Black;

                for (int i = 0; i < cabeceraAcuse.Length; i++)
                {
                    //Alineacion de celdas
                    Paragraph p = acuRow.Cells[i].AddParagraph();
                    acuRow.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;

                    //Formato de datos
                    TextRange TR = p.AppendText(cabeceraAcuse[i]);
                    TR.CharacterFormat.FontName = "Montserrat";
                    TR.CharacterFormat.FontSize = 8;
                    TR.CharacterFormat.TextColor = Color.White;
                }

                for (int r = 0; r < datosAcuse.Length; r++)
                {
                    TableRow DataRow = tablaAcuse.Rows[r + 1];
                    //Fila Height
                    DataRow.Height = 5;

                    //Columnas
                    for (int c = 0; c < datosAcuse[r].Length; c++)
                    {
                        //Alineacion de Celdas
                        DataRow.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                        //Llenar datos en filas
                        Paragraph p2 = DataRow.Cells[c].AddParagraph();
                        TextRange TR2 = p2.AppendText(datosAcuse[r][c]);
                        //Formato de celdas
                        p2.Format.HorizontalAlignment = HorizontalAlignment.Center;
                        TR2.CharacterFormat.FontName = "Montserrat";
                        TR2.CharacterFormat.FontSize = 8;
                    }
                }

                BookmarksNavigator marcaAcuse = new BookmarksNavigator(document);
                marcaAcuse.MoveToBookmark("Acuses", true, true);

                marcaAcuse.InsertTable(tablaAcuse);
                document.Replace("||Acuses||", "", false, true);
            }
            else
                document.Replace("||Acuses||", "Sin Incidencias en el mes.", false, true);

            if (extravio.Length > 0)
            {
                Table tablaExtravio = tablas.AddTable(true);

                String[] cabeceraExtravio = { "Número de Guía", "Código de Rastreo", "Tipo de Servicio" };
                var datosExtravio = extravio.Select(w => new String[] { w.fcNoGuia, w.fcCodRastreo, w.fcTipoServicio }).ToArray();

                tablaExtravio.ResetCells(datosExtravio.Length + 1, cabeceraExtravio.Length);

                TableRow extRow = tablaExtravio.Rows[0];
                extRow.IsHeader = true;
                extRow.Height = 8;
                extRow.RowFormat.BackColor = Color.Black;

                for (int i = 0; i < cabeceraExtravio.Length; i++)
                {
                    //Alineacion de celdas
                    Paragraph p = extRow.Cells[i].AddParagraph();
                    extRow.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;

                    //Formato de datos
                    TextRange TR = p.AppendText(cabeceraExtravio[i]);
                    TR.CharacterFormat.FontName = "Montserrat";
                    TR.CharacterFormat.FontSize = 8;
                    TR.CharacterFormat.TextColor = Color.White;
                }
                for (int r = 0; r < datosExtravio.Length; r++)
                {
                    TableRow DataRow = tablaExtravio.Rows[r + 1];
                    //Fila Height
                    DataRow.Height = 5;

                    //Columnas
                    for (int c = 0; c < datosExtravio[r].Length; c++)
                    {
                        //Alineacion de Celdas
                        DataRow.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                        //Llenar datos en filas
                        Paragraph p2 = DataRow.Cells[c].AddParagraph();
                        TextRange TR2 = p2.AppendText(datosExtravio[r][c]);
                        //Formato de celdas
                        p2.Format.HorizontalAlignment = HorizontalAlignment.Center;
                        TR2.CharacterFormat.FontName = "Montserrat";
                        TR2.CharacterFormat.FontSize = 8;
                    }
                }

                BookmarksNavigator marcaExtravio = new BookmarksNavigator(document);
                marcaExtravio.MoveToBookmark("Extravio", true, true);

                marcaExtravio.InsertTable(tablaExtravio);
                document.Replace("||Extravio||", "", false, true);
            }
            else
                document.Replace("||Extravio||", "Sin Incidencias en el mes.", false, true);

            if (malEstado.Length > 0)
            {
                //Crear Tabla
                Table tablaMalEstado = tablas.AddTable(true);

                //Crear Cabeceras y datos
                String[] cabeceraMalEstado = { "Número de Guía", "Código de Rastreo", "Tipo de Servicio" };
                var datosMalEstado = malEstado.Select(w => new String[] { w.fcNoGuia, w.fcCodRastreo, w.fcTipoServicio }).ToArray();

                //Agregar Celdas
                tablaMalEstado.ResetCells(datosMalEstado.Length + 1, cabeceraMalEstado.Length);

                //Cabecera
                TableRow malERow = tablaMalEstado.Rows[0];
                malERow.IsHeader = true;

                //Fila Height
                malERow.Height = 8;

                //Formato Cabecera
                malERow.RowFormat.BackColor = Color.Black;
                for (int i = 0; i < cabeceraMalEstado.Length; i++)
                {
                    //Alineacion de celdas
                    Paragraph p = malERow.Cells[i].AddParagraph();
                    malERow.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;

                    //Formato de datos
                    TextRange TR = p.AppendText(cabeceraMalEstado[i]);
                    TR.CharacterFormat.FontName = "Montserrat";
                    TR.CharacterFormat.FontSize = 8;
                    TR.CharacterFormat.TextColor = Color.White;
                }

                //Fila de datos
                for (int r = 0; r < datosMalEstado.Length; r++)
                {
                    TableRow DataRow = tablaMalEstado.Rows[r + 1];
                    //Fila Height
                    DataRow.Height = 5;

                    //Columnas
                    for (int c = 0; c < datosMalEstado[r].Length; c++)
                    {
                        //Alineacion de Celdas
                        DataRow.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                        //Llenar datos en filas
                        Paragraph p2 = DataRow.Cells[c].AddParagraph();
                        TextRange TR2 = p2.AppendText(datosMalEstado[r][c]);
                        //Formato de celdas
                        p2.Format.HorizontalAlignment = HorizontalAlignment.Center;
                        TR2.CharacterFormat.FontName = "Montserrat";
                        TR2.CharacterFormat.FontSize = 8;
                    }
                }

                //Buscar la Marca e Instertar
                BookmarksNavigator marcaMalEstado = new BookmarksNavigator(document);
                marcaMalEstado.MoveToBookmark("MalEstado", true, true);

                marcaMalEstado.InsertTable(tablaMalEstado);
                document.Replace("||MalEstado||", "", false, true);
            }
            else
                document.Replace("||MalEstado||", "Sin Incidencias en el mes.", false, true);

            document.Replace("||C||", cedula.fiCalificacion.ToString(),false,true);
            DateTime fecha = (DateTime)cedula.fdFechaEnviado;
            document.Replace("||Mes||", cedula.fcMes + " " + cedula.fiAnio.ToString(), false, true);
            document.Replace("||Fecha||", fecha.Day.ToString() + "-" + intaMes(fecha.Month) + "-" + fecha.Year.ToString(), false, true);
            document.Replace("||Factura||", cedula.fcNumFactura, false, true);
            document.Replace("||Monto||", cedula.fiMontoFactura.ToString("C"), false, true);
            document.Replace("||F||", cedula.fcNoCed, false, true);

            var administracion = (from admin in context.InmueblesCJF
                                  where admin.Administracion.Equals(cedula.fcAdministracion)
                                  select admin).FirstOrDefault();

            document.Replace("||Administracion||", "Administración: " + administracion.Nombre
                + "; Inmueble: " + cedula.fcInmueble, false, true);
            document.Replace("||Usuario||", cedula.fcUsuario, false, true);

            if (material.Length > 0)
            {
                if (material[0].fiDiasMaterial > 0)
                {
                    document.Replace("||Material||", "El proveedor tuvo en total " + material[0].fiDiasMaterial.ToString() + " día(s) de atraso al entregar material de embalaje", false, true);
                }
                else
                {
                    document.Replace("||Material||", "Sin incidencias en el mes.", false, true);
                }
            }
            else
            {
                document.Replace("||Material||", "Sin incidencias en el mes.", false, true);
            }


            //Salvar y Lanzar

            byte[] toArray = null;
            using (MemoryStream ms1 = new MemoryStream())
            {
                document.SaveToStream(ms1, Spire.Doc.FileFormat.PDF);
                toArray = ms1.ToArray();
            }
            return File(toArray, "application/pdf", "ReporteMensajeria"+cedula.fcInmueble+cedula.fcMes+".pdf");
        }

        [Route("ReporteLimpiezaPorValidar/{folio}")]
        public IActionResult LimpiezaPorValidar(string folio)
        {
            var cedFromRep = repository.getCedula("Limpieza", folio);

            EvaluacionLimpieza cedula = cedFromRep.cedula;
            EntregablesLimpieza entregable = cedFromRep.entregable;
            CedLimpActividades[] actividades = new CedLimpActividades[cedFromRep.arreglos.arregloActi.Count];
            CedLimpEquipo[] equipo = new CedLimpEquipo[cedFromRep.arreglos.arregloEquipo.Count];


            for (int i = 0; i < actividades.Length; i++)
            {
                actividades[i] = cedFromRep.arreglos.arregloActi[i];
            }

            for (int i = 0; i < equipo.Length; i++)
            {
                equipo[i] = cedFromRep.arreglos.arregloEquipo[i];
            }

            //Jalar documento con marcas
            Document document = new Document();
            //var path = @"d:\Usuarios\oiguerrero\Source\Repos\GESCore-master\Formulario\Docs\RepLimpValid.docx";
            var path = @"e:\sitios\Cedulas\Docs\RepLimpValid.docx";
            document.LoadFromFile(path);

            

            if (actividades.Length > 0)
            {
                //Crear Tabla
                Section seccionActi = document.AddSection();
                Table tablaActividades = seccionActi.AddTable(true);

                String[] cabeceraActividades = { "Tipo", "Área", "Fecha Incidencia" };
                var datosActividades = actividades.Select(w => new String[] { w.fcTipo, w.fcArea, w.fdFechaInci.GetValueOrDefault().ToString("dd/MM/yyy") }).ToArray();

                tablaActividades.ResetCells(datosActividades.Length + 1, cabeceraActividades.Length);

                TableRow recRow = tablaActividades.Rows[0];
                recRow.IsHeader = true;
                recRow.Height = 8;

                recRow.RowFormat.BackColor = Color.Black;

                for (int i = 0; i < cabeceraActividades.Length; i++)
                {
                    //Alineacion de celdas
                    Paragraph p = recRow.Cells[i].AddParagraph();
                    recRow.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;

                    //Formato de datos
                    TextRange TR = p.AppendText(cabeceraActividades[i]);
                    TR.CharacterFormat.FontName = "Montserrat";
                    TR.CharacterFormat.FontSize = 8;
                    TR.CharacterFormat.TextColor = Color.White;
                }

                for (int r = 0; r < datosActividades.Length; r++)
                {
                    TableRow DataRow = tablaActividades.Rows[r + 1];
                    //Fila Height
                    DataRow.Height = 5;

                    //Columnas
                    for (int c = 0; c < datosActividades[r].Length; c++)
                    {
                        //Alineacion de Celdas
                        DataRow.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                        //Llenar datos en filas
                        Paragraph p2 = DataRow.Cells[c].AddParagraph();
                        TextRange TR2 = p2.AppendText(datosActividades[r][c]);
                        //Formato de celdas
                        p2.Format.HorizontalAlignment = HorizontalAlignment.Center;
                        TR2.CharacterFormat.FontName = "Montserrat";
                        TR2.CharacterFormat.FontSize = 8;
                    }
                }

                BookmarksNavigator marcaActividades = new BookmarksNavigator(document);
                marcaActividades.MoveToBookmark("ProgramaOP", true, true);
                marcaActividades.InsertTable(tablaActividades);
                document.Sections.Remove(seccionActi);
                document.Replace("||ProgramaOP||", "", false, true);
            }
            else
                document.Replace("||ProgramaOP||", "Sin Incidencias en el mes.", false, true);


            if (equipo.Length > 0)
            {
                //Crear Tabla
                Section seccionEquipo = document.AddSection();
                Table tablaEquipo = seccionEquipo.AddTable(true);

                String[] caberaEquipo = { "Maquina/Equipo", "Fecha Incidencia" };
                var datosEquipo = equipo.Select(w => new String[] { w.fcMaquina, w.fdFechaInci.GetValueOrDefault().ToString("dd/MM/yyy") }).ToArray();

                tablaEquipo.ResetCells(datosEquipo.Length + 1, caberaEquipo.Length);

                TableRow entRow = tablaEquipo.Rows[0];
                entRow.IsHeader = true;
                entRow.Height = 8;
                entRow.RowFormat.BackColor = Color.Black;

                for (int i = 0; i < caberaEquipo.Length; i++)
                {
                    //Alineacion de celdas
                    Paragraph p = entRow.Cells[i].AddParagraph();
                    entRow.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;

                    //Formato de datos
                    TextRange TR = p.AppendText(caberaEquipo[i]);
                    TR.CharacterFormat.FontName = "Montserrat";
                    TR.CharacterFormat.FontSize = 8;
                    TR.CharacterFormat.TextColor = Color.White;
                }

                for (int r = 0; r < datosEquipo.Length; r++)
                {
                    TableRow DataRow = tablaEquipo.Rows[r + 1];
                    //Fila Height
                    DataRow.Height = 5;

                    //Columnas
                    for (int c = 0; c < datosEquipo[r].Length; c++)
                    {
                        //Alineacion de Celdas
                        DataRow.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                        //Llenar datos en filas
                        Paragraph p2 = DataRow.Cells[c].AddParagraph();
                        TextRange TR2 = p2.AppendText(datosEquipo[r][c]);
                        //Formato de celdas
                        p2.Format.HorizontalAlignment = HorizontalAlignment.Center;
                        TR2.CharacterFormat.FontName = "Montserrat";
                        TR2.CharacterFormat.FontSize = 8;
                    }
                }

                

                BookmarksNavigator marcaEquipo = new BookmarksNavigator(document);
                marcaEquipo.MoveToBookmark("Maquinaria", true, true);
                marcaEquipo.InsertTable(tablaEquipo);
                document.Sections.Remove(seccionEquipo);
                document.Replace("||Maquinaria||", "", false, true);
            }
            else
                document.Replace("||Maquinaria||", "Sin Incidencias en el mes.", false, true);


            if (cedula.fcPenaUniforme == null)
            {
                document.Replace("||Uniforme||", "Sin incidencias en el mes.", false, true);

            }
            else
            {
                document.Replace("||Uniforme||", "El personal del proveedor no cumplió con los requisitos de identidad (uniforme y credencial)", false, true);
            }


            if (cedula.fiInasistencias > 0)
            {
                document.Replace("||Inasistencias||", "Hubo " + cedula.fiInasistencias + " en el periodo", false, true);
            }
            else
            {
                document.Replace("||Inasistencias||", "No hubo inasistencias en el periodo", false, true);
            }


            if (entregable.fdCierreMes == null)
            {
                document.Replace("||FechaC||", "Fecha de cierre por definir", false, true);
            }
            else
            {
                document.Replace("||FechaC||", "Fecha de cierre establecida el " + entregable.fdCierreMes.GetValueOrDefault().ToString("dd/MM/yyy"), false, true);
            }

            if (entregable.fdSUAIMSS == null)
            {
                document.Replace("||FechaIMSS||", "No lo ha entregado", false, true);
            }
            else
            {
                document.Replace("||FechaIMSS||", "Fecha de entrega: " + entregable.fdSUAIMSS.GetValueOrDefault().ToString("dd/MM/yyy"), false, true);
            }

            if (entregable.fdActaServicios == null)
            {
                document.Replace("||FechaActa||", "No lo ha entregado", false, true);
            }
            else
            {
                document.Replace("||FechaActa||", "Fecha de entrega: " + entregable.fdActaServicios.GetValueOrDefault().ToString("dd/MM/yyy"), false, true);
            }

            document.Replace("||C||", cedula.fiCalificacion.ToString(), false, true);
            DateTime fecha;
            if (cedula.fdFechaEnviado == null)
            {
                fecha = (DateTime)cedula.fdFechaGuardado;
            }
            else
            {
                fecha = (DateTime)cedula.fdFechaEnviado;
            }
            document.Replace("||Mes||", cedula.fcMes + " " + cedula.fiAnio.ToString(), false, true);
            document.Replace("||Fecha||", fecha.Day.ToString() + "-" + intaMes(fecha.Month) + "-" + fecha.Year.ToString(), false, true);
            document.Replace("||Factura||", cedula.fcNumFactura, false, true);
            document.Replace("||Monto||", cedula.fiMontoFactura.ToString("C"), false, true);
            document.Replace("||F||", cedula.fcNoCed, false, true);

            if (cedula.fcUsuario == null)
            {
                document.Replace("||E||", "Por designar", false, true);
            }
            else
            {
                document.Replace("||E||", cedula.fcUsuario, false, true);
            }
            
            var administracion = (from admin in context.InmueblesCJF
                                  where admin.Administracion.Equals(cedula.fcAdministracion)
                                  select admin).FirstOrDefault();

            document.Replace("||Administracion||", "Administración: " + administracion.Nombre
                + "; Inmueble: " + cedula.fcInmueble, false, true);
            document.Replace("||Usuario||", cedula.fcUsuario, false, true);


            if (cedula.fcMes == "Enero")
            {
                string A, B, C, D;
                if (entregable.fdVisitaInstalaciones == null)
                {
                    A = "No se presentó / No se ha registrado";
                }
                else
                {
                    A = "Se presentó el " + entregable.fdVisitaInstalaciones.GetValueOrDefault().ToString("dd/MM/yyy");
                }

                if (entregable.fdProgramaOperacion == null)
                {
                    B = "No se entregó / No se ha registrado";
                }
                else
                {
                    B = "Se presentó el " + entregable.fdProgramaOperacion.GetValueOrDefault().ToString("dd/MM/yyy");
                }

                if (entregable.fdGafeteUniforme == null)
                {
                    C = "No se entregaron / No se ha registrado";
                }
                else
                {
                    C = "Se presentó el " + entregable.fdGafeteUniforme.GetValueOrDefault().ToString("dd/MM/yyy");
                }

                if (entregable.fdActaInicio == null)
                {
                    D = "No se entregó / No se ha registrado";
                }
                else
                {
                    D = "Se presentó el " + entregable.fdActaInicio.GetValueOrDefault().ToString("dd/MM/yyy");
                }

                Paragraph paraInserted = new Paragraph(document);
                TextRange textRange = paraInserted.AppendText("\n" + "A. ¿Cúando se presentó el prestador del servicio a realizar la visita a las instalaciones?\n" +
                A + "\n" +
                "B. ¿Cúando entregó el prestador del servicio el Programa de Operación?\n" +
                B + "\n" +
                "C. ¿Cúando entregó el prestador del servicio los uniformes y gafetes a su personal?\n" +
                C + "\n" +
                "D. ¿Cúando entregó el prestador del servicio el Acta de Inicio del servicio?\n" +
                D + "\n");
                textRange.CharacterFormat.FontSize = 8;
                textRange.CharacterFormat.FontName = "Montserrat";
                textRange.CharacterFormat.Bold = true;

                Section section = document.Sections[0];
                TextSelection selection = document.FindString("||PrimerMes||", true, true);
                TextRange range = selection.GetAsOneRange();
                Paragraph paragraph = range.OwnerParagraph;
                Body body = paragraph.OwnerTextBody;
                int index = body.ChildObjects.IndexOf(paragraph);
                body.ChildObjects.Remove(paragraph);
                body.ChildObjects.Insert(index, paraInserted);
            }
            else
            {
                document.Replace("||PrimerMes||", "", false, true);
            }


            //Salvar y Lanzar

            byte[] toArray = null;
            using (MemoryStream ms1 = new MemoryStream())
            {
                document.SaveToStream(ms1, Spire.Doc.FileFormat.PDF);
                toArray = ms1.ToArray();
            }
            return File(toArray, "application/pdf", "ReporteLimpieza_" + cedula.fcInmueble + cedula.fcMes + ".pdf");
        }

        [Route("GenerarReporteFumigacion/{folio}")]
        public IActionResult ReporteFumigacion(string folio)
        {
            var cedFromRep = repository.getCedula("Fumigacion", folio);

            EvaluacionFumigacion cedula = cedFromRep.cedula;
            EntregablesFumigacion entregable = cedFromRep.entregable;
            CedFumFechas[] fechas = new CedFumFechas[cedFromRep.arreglos.arregloFechas.Count];
            CedFumFumigacion[] fumigacion = new CedFumFumigacion[cedFromRep.arreglos.arregloFumigacion.Count];
            CedFumHoras[] horas = new CedFumHoras[cedFromRep.arreglos.arregloHoras.Count];

            for (int i = 0; i < fechas.Length; i++)
            {
                fechas[i] = cedFromRep.arreglos.arregloFechas[i];
            }

            for (int i = 0; i < fumigacion.Length; i++)
            {
                fumigacion[i] = cedFromRep.arreglos.arregloFumigacion[i];
            }

            for (int i = 0; i < horas.Length; i++)
            {
                horas[i] = cedFromRep.arreglos.arregloHoras[i];
            }

            //Jalar documento con marcas
            Document document = new Document();
            //var path = @"d:\Usuarios\oiguerrero\Source\Repos\GESCore-master\Formulario\Docs\RepFum.docx";
            var path = @"e:\sitios\Cedulas\Docs\RepFum.docx";
            document.LoadFromFile(path);


            //Crear Tabla

            if (fechas.Length > 0)
            {
                Section seccionFechas = document.AddSection();
                Table tablaFechas = seccionFechas.AddTable(true);

                String[] cabeceraFechas = { "Fecha Programada", "Fecha Realizada", "Comentarios" };
                var datosFechas = fechas.Select(w => new String[] { w.fdFechaProgramada.GetValueOrDefault().ToString("dd/MM/yyy"), w.fdFechaRealizada.GetValueOrDefault().ToString("dd/MM/yyy"), w.fcComentarios }).ToArray();

                tablaFechas.ResetCells(datosFechas.Length + 1, cabeceraFechas.Length);

                TableRow recRow = tablaFechas.Rows[0];
                recRow.IsHeader = true;
                recRow.Height = 8;

                recRow.RowFormat.BackColor = Color.Black;

                for (int i = 0; i < cabeceraFechas.Length; i++)
                {
                    //Alineacion de celdas
                    Paragraph p = recRow.Cells[i].AddParagraph();
                    recRow.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;

                    //Formato de datos
                    TextRange TR = p.AppendText(cabeceraFechas[i]);
                    TR.CharacterFormat.FontName = "Montserrat";
                    TR.CharacterFormat.FontSize = 8;
                    TR.CharacterFormat.TextColor = Color.White;
                }

                for (int r = 0; r < datosFechas.Length; r++)
                {
                    TableRow DataRow = tablaFechas.Rows[r + 1];
                    //Fila Height
                    DataRow.Height = 5;

                    //Columnas
                    for (int c = 0; c < datosFechas[r].Length; c++)
                    {
                        //Alineacion de Celdas
                        DataRow.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                        //Llenar datos en filas
                        Paragraph p2 = DataRow.Cells[c].AddParagraph();
                        TextRange TR2 = p2.AppendText(datosFechas[r][c]);
                        //Formato de celdas
                        p2.Format.HorizontalAlignment = HorizontalAlignment.Center;
                        TR2.CharacterFormat.FontName = "Montserrat";
                        TR2.CharacterFormat.FontSize = 8;
                    }
                }

                BookmarksNavigator marcaFechas = new BookmarksNavigator(document);
                marcaFechas.MoveToBookmark("Fechas", true, true);
                marcaFechas.InsertTable(tablaFechas);
                document.Sections.Remove(seccionFechas);
                document.Replace("||Fechas||", "", false, true);
            }
            else
                document.Replace("||Fechas||", "Sin Incidencias en el mes.", false, true);

            if (fumigacion.Length > 0)
            {
                Section seccionFum = document.AddSection();
                Table tablaFum = seccionFum.AddTable(true);

                String[] cabeceraFum = { "Fecha Fumigación", "Fecha Reaparición", "Comentarios" };
                var datosFum = fumigacion.Select(w => new String[] { w.fdFechaFumigacion.GetValueOrDefault().ToString("dd/MM/yyy"), w.fdFechaReaparacion.GetValueOrDefault().ToString("dd/MM/yyy"), w.fcComentarios }).ToArray();

                tablaFum.ResetCells(datosFum.Length + 1, cabeceraFum.Length);

                TableRow entRow = tablaFum.Rows[0];
                entRow.IsHeader = true;
                entRow.Height = 8;
                entRow.RowFormat.BackColor = Color.Black;

                for (int i = 0; i < cabeceraFum.Length; i++)
                {
                    //Alineacion de celdas
                    Paragraph p = entRow.Cells[i].AddParagraph();
                    entRow.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;

                    //Formato de datos
                    TextRange TR = p.AppendText(cabeceraFum[i]);
                    TR.CharacterFormat.FontName = "Montserrat";
                    TR.CharacterFormat.FontSize = 8;
                    TR.CharacterFormat.TextColor = Color.White;
                }

                for (int r = 0; r < datosFum.Length; r++)
                {
                    TableRow DataRow = tablaFum.Rows[r + 1];
                    //Fila Height
                    DataRow.Height = 5;

                    //Columnas
                    for (int c = 0; c < datosFum[r].Length; c++)
                    {
                        //Alineacion de Celdas
                        DataRow.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                        //Llenar datos en filas
                        Paragraph p2 = DataRow.Cells[c].AddParagraph();
                        TextRange TR2 = p2.AppendText(datosFum[r][c]);
                        //Formato de celdas
                        p2.Format.HorizontalAlignment = HorizontalAlignment.Center;
                        TR2.CharacterFormat.FontName = "Montserrat";
                        TR2.CharacterFormat.FontSize = 8;
                    }
                }

                BookmarksNavigator marcaFum = new BookmarksNavigator(document);
                marcaFum.MoveToBookmark("Efectivo", true, true);
                marcaFum.InsertTable(tablaFum);
                document.Sections.Remove(seccionFum);
                document.Replace("||Efectivo||", "", false, true);
            }
            else
                document.Replace("||Efectivo||", "Sin Incidencias en el mes.", false, true);

            if (horas.Length > 0)
            {
                Section seccionHoras = document.AddSection();
                Table tablaHoras = seccionHoras.AddTable(true);

                String[] cabeceraHoras = { "Fecha", "Hora Programada", "Hora Realizada", "Comentarios" };
                var datosHoras = horas.Select(w => new String[] { w.fdFechaProgramada.GetValueOrDefault().ToString("dd/MM/yyy"), w.fdHoraProgramada.GetValueOrDefault().ToString("t"), w.fdHoraEfectiva.GetValueOrDefault().ToString("t"), w.fcComentarios }).ToArray();

                tablaHoras.ResetCells(datosHoras.Length + 1, cabeceraHoras.Length);

                TableRow entRow = tablaHoras.Rows[0];
                entRow.IsHeader = true;
                entRow.Height = 8;
                entRow.RowFormat.BackColor = Color.Black;

                for (int i = 0; i < cabeceraHoras.Length; i++)
                {
                    //Alineacion de celdas
                    Paragraph p = entRow.Cells[i].AddParagraph();
                    entRow.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;

                    //Formato de datos
                    TextRange TR = p.AppendText(cabeceraHoras[i]);
                    TR.CharacterFormat.FontName = "Montserrat";
                    TR.CharacterFormat.FontSize = 8;
                    TR.CharacterFormat.TextColor = Color.White;
                }

                for (int r = 0; r < datosHoras.Length; r++)
                {
                    TableRow DataRow = tablaHoras.Rows[r + 1];
                    //Fila Height
                    DataRow.Height = 5;

                    //Columnas
                    for (int c = 0; c < datosHoras[r].Length; c++)
                    {
                        //Alineacion de Celdas
                        DataRow.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                        //Llenar datos en filas
                        Paragraph p2 = DataRow.Cells[c].AddParagraph();
                        TextRange TR2 = p2.AppendText(datosHoras[r][c]);
                        //Formato de celdas
                        p2.Format.HorizontalAlignment = HorizontalAlignment.Center;
                        TR2.CharacterFormat.FontName = "Montserrat";
                        TR2.CharacterFormat.FontSize = 8;
                    }
                }

                BookmarksNavigator marcaFum = new BookmarksNavigator(document);
                marcaFum.MoveToBookmark("Horas", true, true);
                marcaFum.InsertTable(tablaHoras);
                document.Sections.Remove(seccionHoras);
                document.Replace("||Horas||", "", false, true);
            }
            else
                document.Replace("||Horas||", "Sin Incidencias en el mes.", false, true);

            if (cedula.fiEtiquetas == 0)
            {
                document.Replace("||Vigencia||", "Los productos o el etiquetado de estos estaban caducos o no cumplian con la regulación vigente", false, true);
            }
            else
            {
                document.Replace("||Vigencia||", "Sin incidencias en el mes.", false, true);
            }


            if (entregable.fdCierreMes == null)
            {
                document.Replace("||FechaC||", "Fecha de cierre por definir", false, true);
            }
            else
            {
                document.Replace("||FechaC||", "Fecha de cierre establecida el " + entregable.fdCierreMes.GetValueOrDefault().ToString("dd/MM/yyy"), false, true);
            }

            if (entregable.fdReporteServ == null)
            {
                document.Replace("||FechaRS||", "No lo ha entregado", false, true);
            }
            else
            {
                document.Replace("||FechaRS||", "Fecha de entrega: " + entregable.fdReporteServ.GetValueOrDefault().ToString("dd/MM/yyy"), false, true);
            }

            if (entregable.fdListadePersonal == null)
            {
                document.Replace("||FechaListado||", "No lo ha entregado", false, true);
            }
            else
            {
                document.Replace("||FechaListado||", "Fecha de entrega: " + entregable.fdListadePersonal.GetValueOrDefault().ToString("dd/MM/yyy"), false, true);
            }

            if (entregable.fdSUAIMSS == null)
            {
                document.Replace("||FechaIMSS||", "No lo ha entregado", false, true);
            }
            else
            {
                document.Replace("||FechaIMSS||", "Fecha de entrega: " + entregable.fdSUAIMSS.GetValueOrDefault().ToString("dd/MM/yyy"), false, true);
            }

            if (entregable.fdActaEntrega == null)
            {
                document.Replace("||FechaActa||", "No lo ha entregado", false, true);
            }
            else
            {
                document.Replace("||FechaActa||", "Fecha de entrega: " + entregable.fdActaEntrega.GetValueOrDefault().ToString("dd/MM/yyy"), false, true);
            }

            document.Replace("||C||", cedula.fiCalificacion.ToString(), false, true);

            DateTime fecha;
            if (cedula.fdFechaEnviado == null)
            {
                fecha = (DateTime)cedula.fdFechaGuardado;
            }
            else
            {
                fecha = (DateTime)cedula.fdFechaEnviado;
            }
            document.Replace("||Mes||", cedula.fcMes + " " + cedula.fiAnio.ToString(), false, true);
            document.Replace("||Fecha||", fecha.Day.ToString() + "-" + intaMes(fecha.Month) + "-" + fecha.Year.ToString(), false, true);
            document.Replace("||Factura||", cedula.fcNumFactura, false, true);
            document.Replace("||Monto||", cedula.fiMontoFactura.ToString("C"), false, true);
            document.Replace("||F||", cedula.fcNoCed, false, true);

            if (cedula.fcUsuario == null)
            {
                document.Replace("||E||", "Por designar", false, true);
            }
            else
            {
                document.Replace("||E||", cedula.fcUsuario, false, true);
            }

            var administracion = (from admin in context.InmueblesCJF
                                  where admin.Administracion.Equals(cedula.fcAdministracion)
                                  select admin).FirstOrDefault();

            document.Replace("||Administracion||", "Administración: " + administracion.Nombre
                + "; Inmueble: " + cedula.fcInmueble, false, true);
            document.Replace("||Usuario||", cedula.fcUsuario, false, true);

            if (cedula.fcMes == "Enero")
            {
                string A, B, C, D;
                if (entregable.fdProgramaOperacion == null)
                {
                    A = "No se entregó";
                }
                else
                {
                    A = "Se entregó el " + entregable.fdProgramaOperacion.GetValueOrDefault().ToString("dd/MM/yyy");
                }

                if (entregable.fdUniforme == null)
                {
                    B = "No se entregó";
                }
                else
                {
                    B = "Se entregó el " + entregable.fdUniforme.GetValueOrDefault().ToString("dd/MM/yyy");
                }

                if (entregable.fdActaInicio == null)
                {
                    C = "No se entregó";
                }
                else
                {
                    C = "Se entregó el " + entregable.fdActaInicio.GetValueOrDefault().ToString("dd/MM/yyy");
                }

                if (entregable.fdListadePersonal == null)
                {
                    D = "No se entregó";
                }
                else
                {
                    D = "Se presentó el " + entregable.fdListadePersonal.GetValueOrDefault().ToString("dd/MM/yyy");
                }

                Paragraph paraInserted = new Paragraph(document);
                TextRange textRange = paraInserted.AppendText("\n" + "A. ¿Cúando se presentó el prestador del servicio a realizar la visita a las instalaciones?\n" +
                A + "\n" +
                "B. ¿Cúando entregó el prestador del servicio el Programa de Operación?\n" +
                B + "\n" +
                "C. ¿Cúando entregó el prestador del servicio los uniformes y gafetes a su personal?\n" +
                C + "\n" +
                "D. ¿Cúando entregó el prestador del servicio el Acta de Inicio del servicio?\n" +
                D + "\n");
                textRange.CharacterFormat.FontSize = 8;
                textRange.CharacterFormat.FontName = "Montserrat";
                textRange.CharacterFormat.Bold = true;

                Section section = document.Sections[0];
                TextSelection selection = document.FindString("||PrimerMes||", true, true);
                TextRange range = selection.GetAsOneRange();
                Paragraph paragraph = range.OwnerParagraph;
                Body body = paragraph.OwnerTextBody;
                int index = body.ChildObjects.IndexOf(paragraph);
                body.ChildObjects.Remove(paragraph);
                body.ChildObjects.Insert(index, paraInserted);
            }
            else
            {
                document.Replace("||PrimerMes||", "", false, true);
            }

            //Salvar y Lanzar

            byte[] toArray = null;
            using (MemoryStream ms1 = new MemoryStream())
            {
                document.SaveToStream(ms1, Spire.Doc.FileFormat.PDF);
                toArray = ms1.ToArray();
            }
            return File(toArray, "application/pdf", "ReporteFumigación_" + cedula.fcInmueble + cedula.fcMes + ".pdf");
        }

        [Route("ReporteFumigacionPorValidar/{folio}")]
        public IActionResult FumigacionPorValidar(string folio)
        {
            var cedFromRep = repository.getCedula("Fumigacion", folio);

            EvaluacionFumigacion cedula = cedFromRep.cedula;
            EntregablesFumigacion entregable = cedFromRep.entregable;
            CedFumFechas[] fechas = new CedFumFechas[cedFromRep.arreglos.arregloFechas.Count];
            CedFumFumigacion[] fumigacion = new CedFumFumigacion[cedFromRep.arreglos.arregloFumigacion.Count];
            CedFumHoras[] horas = new CedFumHoras[cedFromRep.arreglos.arregloHoras.Count];

            for (int i = 0; i < fechas.Length; i++)
            {
                fechas[i] = cedFromRep.arreglos.arregloFechas[i];
            }

            for (int i = 0; i < fumigacion.Length; i++)
            {
                fumigacion[i] = cedFromRep.arreglos.arregloFumigacion[i];
            }

            for (int i = 0; i < horas.Length; i++)
            {
                horas[i] = cedFromRep.arreglos.arregloHoras[i];
            }

            //Jalar documento con marcas
            Document document = new Document();
            //var path = @"d:\Usuarios\oiguerrero\Source\Repos\GESCore\Formulario\Docs\RepFumValid.docx";
            var path = @"e:\sitios\Cedulas\Docs\RepFumValid.docx";
            document.LoadFromFile(path);


            //Crear Tabla

            if (fechas.Length > 0)
            {
                Section seccionFechas = document.AddSection();
                Table tablaFechas = seccionFechas.AddTable(true);

                String[] cabeceraFechas = { "Fecha Programada", "Fecha Realizada", "Comentarios" };
                var datosFechas = fechas.Select(w => new String[] { w.fdFechaProgramada.GetValueOrDefault().ToString("dd/MM/yyy"), w.fdFechaRealizada.GetValueOrDefault().ToString("dd/MM/yyy"), w.fcComentarios }).ToArray();

                tablaFechas.ResetCells(datosFechas.Length + 1, cabeceraFechas.Length);

                TableRow recRow = tablaFechas.Rows[0];
                recRow.IsHeader = true;
                recRow.Height = 8;

                recRow.RowFormat.BackColor = Color.Black;

                for (int i = 0; i < cabeceraFechas.Length; i++)
                {
                    //Alineacion de celdas
                    Paragraph p = recRow.Cells[i].AddParagraph();
                    recRow.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;

                    //Formato de datos
                    TextRange TR = p.AppendText(cabeceraFechas[i]);
                    TR.CharacterFormat.FontName = "Montserrat";
                    TR.CharacterFormat.FontSize = 8;
                    TR.CharacterFormat.TextColor = Color.White;
                }

                for (int r = 0; r < datosFechas.Length; r++)
                {
                    TableRow DataRow = tablaFechas.Rows[r + 1];
                    //Fila Height
                    DataRow.Height = 5;

                    //Columnas
                    for (int c = 0; c < datosFechas[r].Length; c++)
                    {
                        //Alineacion de Celdas
                        DataRow.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                        //Llenar datos en filas
                        Paragraph p2 = DataRow.Cells[c].AddParagraph();
                        TextRange TR2 = p2.AppendText(datosFechas[r][c]);
                        //Formato de celdas
                        p2.Format.HorizontalAlignment = HorizontalAlignment.Center;
                        TR2.CharacterFormat.FontName = "Montserrat";
                        TR2.CharacterFormat.FontSize = 8;
                    }
                }

                BookmarksNavigator marcaFechas = new BookmarksNavigator(document);
                marcaFechas.MoveToBookmark("Fechas", true, true);
                marcaFechas.InsertTable(tablaFechas);
                document.Sections.Remove(seccionFechas);
                document.Replace("||Fechas||", "", false, true);
            }
            else
                document.Replace("||Fechas||", "Sin Incidencias en el mes.", false, true);

            if (fumigacion.Length > 0)
            {
                Section seccionFum = document.AddSection();
                Table tablaFum = seccionFum.AddTable(true);

                String[] cabeceraFum = { "Fecha Fumigación", "Fecha Reaparición", "Comentarios" };
                var datosFum = fumigacion.Select(w => new String[] { w.fdFechaFumigacion.GetValueOrDefault().ToString("dd/MM/yyy"), w.fdFechaReaparacion.GetValueOrDefault().ToString("dd/MM/yyy"), w.fcComentarios }).ToArray();

                tablaFum.ResetCells(datosFum.Length + 1, cabeceraFum.Length);

                TableRow entRow = tablaFum.Rows[0];
                entRow.IsHeader = true;
                entRow.Height = 8;
                entRow.RowFormat.BackColor = Color.Black;

                for (int i = 0; i < cabeceraFum.Length; i++)
                {
                    //Alineacion de celdas
                    Paragraph p = entRow.Cells[i].AddParagraph();
                    entRow.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;

                    //Formato de datos
                    TextRange TR = p.AppendText(cabeceraFum[i]);
                    TR.CharacterFormat.FontName = "Montserrat";
                    TR.CharacterFormat.FontSize = 8;
                    TR.CharacterFormat.TextColor = Color.White;
                }

                for (int r = 0; r < datosFum.Length; r++)
                {
                    TableRow DataRow = tablaFum.Rows[r + 1];
                    //Fila Height
                    DataRow.Height = 5;

                    //Columnas
                    for (int c = 0; c < datosFum[r].Length; c++)
                    {
                        //Alineacion de Celdas
                        DataRow.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                        //Llenar datos en filas
                        Paragraph p2 = DataRow.Cells[c].AddParagraph();
                        TextRange TR2 = p2.AppendText(datosFum[r][c]);
                        //Formato de celdas
                        p2.Format.HorizontalAlignment = HorizontalAlignment.Center;
                        TR2.CharacterFormat.FontName = "Montserrat";
                        TR2.CharacterFormat.FontSize = 8;
                    }
                }

                BookmarksNavigator marcaFum = new BookmarksNavigator(document);
                marcaFum.MoveToBookmark("Efectivo", true, true);
                marcaFum.InsertTable(tablaFum);
                document.Sections.Remove(seccionFum);
                document.Replace("||Efectivo||", "", false, true);
            }
            else
                document.Replace("||Efectivo||", "Sin Incidencias en el mes.", false, true);

            if (horas.Length > 0)
            {
                Section seccionHoras = document.AddSection();
                Table tablaHoras = seccionHoras.AddTable(true);

                String[] cabeceraHoras = { "Fecha", "Hora Programada", "Hora Realizada", "Comentarios" };
                var datosHoras = horas.Select(w => new String[] { w.fdFechaProgramada.GetValueOrDefault().ToString("dd/MM/yyy"), w.fdHoraProgramada.GetValueOrDefault().ToString("t"), w.fdHoraEfectiva.GetValueOrDefault().ToString("t"), w.fcComentarios }).ToArray();

                tablaHoras.ResetCells(datosHoras.Length + 1, cabeceraHoras.Length);

                TableRow entRow = tablaHoras.Rows[0];
                entRow.IsHeader = true;
                entRow.Height = 8;
                entRow.RowFormat.BackColor = Color.Black;

                for (int i = 0; i < cabeceraHoras.Length; i++)
                {
                    //Alineacion de celdas
                    Paragraph p = entRow.Cells[i].AddParagraph();
                    entRow.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;

                    //Formato de datos
                    TextRange TR = p.AppendText(cabeceraHoras[i]);
                    TR.CharacterFormat.FontName = "Montserrat";
                    TR.CharacterFormat.FontSize = 8;
                    TR.CharacterFormat.TextColor = Color.White;
                }

                for (int r = 0; r < datosHoras.Length; r++)
                {
                    TableRow DataRow = tablaHoras.Rows[r + 1];
                    //Fila Height
                    DataRow.Height = 5;

                    //Columnas
                    for (int c = 0; c < datosHoras[r].Length; c++)
                    {
                        //Alineacion de Celdas
                        DataRow.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                        //Llenar datos en filas
                        Paragraph p2 = DataRow.Cells[c].AddParagraph();
                        TextRange TR2 = p2.AppendText(datosHoras[r][c]);
                        //Formato de celdas
                        p2.Format.HorizontalAlignment = HorizontalAlignment.Center;
                        TR2.CharacterFormat.FontName = "Montserrat";
                        TR2.CharacterFormat.FontSize = 8;
                    }
                }

                BookmarksNavigator marcaFum = new BookmarksNavigator(document);
                marcaFum.MoveToBookmark("Horas", true, true);
                marcaFum.InsertTable(tablaHoras);
                document.Sections.Remove(seccionHoras);
                document.Replace("||Horas||", "", false, true);
            }
            else
                document.Replace("||Horas||", "Sin Incidencias en el mes.", false, true);

            if (cedula.fiEtiquetas == 0)
            {
                document.Replace("||Vigencia||", "Los productos o el etiquetado de estos estaban caducos o no cumplian con la regulación vigente", false, true);
            }
            else
            {
                document.Replace("||Vigencia||", "Sin incidencias en el mes.", false, true);
            }


            if (entregable.fdCierreMes == null)
            {
                document.Replace("||FechaC||", "Fecha de cierre por definir", false, true);
            }
            else
            {
                document.Replace("||FechaC||", "Fecha de cierre establecida el " + entregable.fdCierreMes.GetValueOrDefault().ToString("dd/MM/yyy"), false, true);
            }

            if (entregable.fdReporteServ == null)
            {
                document.Replace("||FechaRS||", "No lo ha entregado", false, true);
            }
            else
            {
                document.Replace("||FechaRS||", "Fecha de entrega: " + entregable.fdReporteServ.GetValueOrDefault().ToString("dd/MM/yyy"), false, true);
            }

            if (entregable.fdListadePersonal == null)
            {
                document.Replace("||FechaListado||", "No lo ha entregado", false, true);
            }
            else
            {
                document.Replace("||FechaListado||", "Fecha de entrega: " + entregable.fdListadePersonal.GetValueOrDefault().ToString("dd/MM/yyy"), false, true);
            }

            if (entregable.fdSUAIMSS == null)
            {
                document.Replace("||FechaIMSS||", "No lo ha entregado", false, true);
            }
            else
            {
                document.Replace("||FechaIMSS||", "Fecha de entrega: " + entregable.fdSUAIMSS.GetValueOrDefault().ToString("dd/MM/yyy"), false, true);
            }

            if (entregable.fdActaEntrega == null)
            {
                document.Replace("||FechaActa||", "No lo ha entregado", false, true);
            }
            else
            {
                document.Replace("||FechaActa||", "Fecha de entrega: " + entregable.fdActaEntrega.GetValueOrDefault().ToString("dd/MM/yyy"), false, true);
            }

            document.Replace("||C||", cedula.fiCalificacion.ToString(), false, true);

            DateTime fecha;
            if (cedula.fdFechaEnviado == null)
            {
                fecha = (DateTime)cedula.fdFechaGuardado;
            }
            else
            {
                fecha = (DateTime)cedula.fdFechaEnviado;
            }
            document.Replace("||Mes||", cedula.fcMes + " " + cedula.fiAnio.ToString(), false, true);
            document.Replace("||Fecha||", fecha.Day.ToString() + "-" + intaMes(fecha.Month) + "-" + fecha.Year.ToString(), false, true);
            document.Replace("||Factura||", cedula.fcNumFactura, false, true);
            document.Replace("||Monto||", cedula.fiMontoFactura.ToString("C"), false, true);
            document.Replace("||F||", cedula.fcNoCed, false, true);

            if (cedula.fcUsuario == null)
            {
                document.Replace("||E||", "Por designar", false, true);
            }
            else
            {
                document.Replace("||E||", cedula.fcUsuario, false, true);
            }

            var administracion = (from admin in context.InmueblesCJF
                                  where admin.Administracion.Equals(cedula.fcAdministracion)
                                  select admin).FirstOrDefault();

            document.Replace("||Administracion||", "Administración: " + administracion.Nombre
                + "; Inmueble: " + cedula.fcInmueble, false, true);
            document.Replace("||Usuario||", cedula.fcUsuario, false, true);

            if (cedula.fcMes == "Enero")
            {
                string A, B, C, D;
                if (entregable.fdProgramaOperacion == null)
                {
                    A = "No se entregó";
                }
                else
                {
                    A = "Se entregó el " + entregable.fdProgramaOperacion.GetValueOrDefault().ToString("dd/MM/yyy");
                }

                if (entregable.fdUniforme == null)
                {
                    B = "No se entregó";
                }
                else
                {
                    B = "Se entregó el " + entregable.fdUniforme.GetValueOrDefault().ToString("dd/MM/yyy");
                }

                if (entregable.fdActaInicio == null)
                {
                    C = "No se entregó";
                }
                else
                {
                    C = "Se entregó el " + entregable.fdActaInicio.GetValueOrDefault().ToString("dd/MM/yyy");
                }

                if (entregable.fdListadePersonal == null)
                {
                    D = "No se entregó";
                }
                else
                {
                    D = "Se presentó el " + entregable.fdListadePersonal.GetValueOrDefault().ToString("dd/MM/yyy");
                }

                Paragraph paraInserted = new Paragraph(document);
                TextRange textRange = paraInserted.AppendText("\n" + "A. ¿Cúando se presentó el prestador del servicio a realizar la visita a las instalaciones?\n" +
                A + "\n" +
                "B. ¿Cúando entregó el prestador del servicio el Programa de Operación?\n" +
                B + "\n" +
                "C. ¿Cúando entregó el prestador del servicio los uniformes y gafetes a su personal?\n" +
                C + "\n" +
                "D. ¿Cúando entregó el prestador del servicio el Acta de Inicio del servicio?\n" +
                D + "\n");
                textRange.CharacterFormat.FontSize = 8;
                textRange.CharacterFormat.FontName = "Montserrat";
                textRange.CharacterFormat.Bold = true;

                Section section = document.Sections[0];
                TextSelection selection = document.FindString("||PrimerMes||", true, true);
                TextRange range = selection.GetAsOneRange();
                Paragraph paragraph = range.OwnerParagraph;
                Body body = paragraph.OwnerTextBody;
                int index = body.ChildObjects.IndexOf(paragraph);
                body.ChildObjects.Remove(paragraph);
                body.ChildObjects.Insert(index, paraInserted);
            }
            else
            {
                document.Replace("||PrimerMes||", "", false, true);
            }

            //Salvar y Lanzar

            byte[] toArray = null;
            using (MemoryStream ms1 = new MemoryStream())
            {
                document.SaveToStream(ms1, Spire.Doc.FileFormat.PDF);
                toArray = ms1.ToArray();
            }
            return File(toArray, "application/pdf", "ReporteFumigación_" + cedula.fcInmueble + cedula.fcMes + ".pdf");
        }

        public string intaMes(int entero)
        {
            string mes = null;
            switch (entero)
            {
                case 1:
                    mes = "Enero";
                    break;
                case 2:
                    mes = "Febrero";
                    break;
                case 3:
                    mes = "Marzo";
                    break;
                case 4:
                    mes = "Abril";
                    break;
                case 5:
                    mes = "Mayo";
                    break;
                case 6:
                    mes = "Junio";
                    break;
                case 7:
                    mes = "Julio";
                    break;
                case 8:
                    mes = "Agosto";
                    break;
                case 9:
                    mes = "Septiembre";
                    break;
                case 10:
                    mes = "Octubre";
                    break;
                case 11:
                    mes = "Noviembre";
                    break;
                case 12:
                    mes = "Diciembre";
                    break;
                default:
                    mes = "Hubo un error";
                    break;
            }
            return mes;
        }
    }
}
