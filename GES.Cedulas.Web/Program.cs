using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Formulario
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.Limits.MaxRequestBodySize = Int32.MaxValue;
                    });
                    webBuilder.UseStartup<Startup>();
                    
                });
    }
}



//Scaffold-DbContext -Connection name=DatabaseConnection -Provider Microsoft.EntityFrameworkCore.SqlServer -UseDatabaseNames -Force –Tables "EvaluacionMensajeria", "CedulasAreaInmueble", "InmueblesCJF", "CedMenRecoleccion", "CedMenEntrega", "CedMenAcuse", "CedMenMaterial", "CedMenExtravio", "CedMenMalEstado", "EvaluacionLimpieza", "CedLimpActividades", "CedLimpEquipo", "EvaluacionFumigacion", "CedFumFechas", "CedFumHoras", "CedFumFumigacion", "EntregablesLimpieza", "ContratoCedula", "CatalogoServicios", "EntregablesFumigacion", "ArchivosCedulas" -OutputDir Model

//EvaluacionMensajeria-
//CedulasAreaInmueble-
//InmueblesCJF-
//CedMenRecoleccion-
//CedMenEntrega-
//CedMenAcuse-
//CedMenMaterial-
//CedMenExtravio-
//CedMenMalEstado-
//EvaluacionLimpieza-
//CedLimpActividades-
//CedLimpEquipo
//EvaluacionFumigacion
//CedFumFechas
//CedFumHoras
//CedFumFumigacion
//EntregablesLimpieza
//ContratoCedula
//EntregablesFumigacion
//ArchivosCedulas