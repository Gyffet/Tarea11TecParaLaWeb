using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentDateTime;
using Formulario.Model;
using Formulario.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Formulario.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CedulasController : ControllerBase
    {
        private readonly ICedulasRepository repository;
        private readonly IHostingEnvironment environment;

        public CedulasController(ICedulasRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [Route("getTodas")]
        public dynamic getTodas()
        {
            try
            {
                return repository.getTodas();
                

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet]
        [Route("getAceptadas")]
        public IActionResult getAceptadas()
        {
            try
            {
                var a = repository.getAceptadas();
                return Ok(a);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("getRechazadas")]
        public IActionResult getRechazadas()
        {
            try
            {
                var a = repository.getRechazadas();
                return Ok(a);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("getPendientes")]
        public IActionResult getPendientes()
        {
            try
            {
                var a = repository.getPendientes();
                return Ok(a);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("getInmuebles")]
        public List<InmueblesCJF> getInmuebles(int area)
        {
            try
            {
                return repository.getInmuebles(area);
            }
            catch(Exception ex)
            {
                List<InmueblesCJF> inmuebles = new List<InmueblesCJF>();
                return inmuebles;
            }
            
        }

        [HttpGet]
        [Route("getCedula")]
        public IActionResult getCedula(string servicio, string folio)
        {
            try
            {
                var cedula = repository.getCedula(servicio, folio);
                return Ok(cedula);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        

        [HttpGet]
        [Route("getPendRech")]
        public dynamic getPendRech(int area)
        {
            return repository.pendientesRechazadas(area);
        }

        [HttpGet]
        [Route("getAceptadasAdmin")]
        public dynamic getAceptadasAdmin(int area)
        {
            return repository.aceptadasAdmin(area);
        }

        [HttpGet]
        [Route("getCedulasPend")]
        //Se tiene que modificar el tipo de valor devuelto
        public dynamic getCedulasPend(int area)
        {
            return repository.getCedulasPend(area);
        }

        
    }
}
