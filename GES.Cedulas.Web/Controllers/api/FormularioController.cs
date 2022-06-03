using Formulario.Model.ModelExt;
using Formulario.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace Formulario.Controllers.api
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    [Authorize]
    public class FormularioController : ControllerBase
    {
        private readonly IFormularioRepository repository;

        public FormularioController(IFormularioRepository repository)
        {
            this.repository = repository;
        }

        
        [HttpPost]
        [Route("enviar")]
        public IActionResult guardarFormulario([FromBody]Persona persona, string jsonIntitucion, string jsonCapacitacion, string jsonPago)
        {
            try
            {
                var institucion = JsonConvert.DeserializeObject<Institucion>(jsonIntitucion);
                var capacitacion = JsonConvert.DeserializeObject<Capacitacion>(jsonCapacitacion);
                var pago = JsonConvert.DeserializeObject<Pago>(jsonPago);
                repository.guardarFormulario(persona, institucion, capacitacion, pago);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("modificar")]
        public IActionResult modificarFormulario([FromBody] Persona persona, string jsonIntitucion, string jsonCapacitacion, string jsonPago, int id)
        {
            try
            {
                var institucion = JsonConvert.DeserializeObject<Institucion>(jsonIntitucion);
                var capacitacion = JsonConvert.DeserializeObject<Capacitacion>(jsonCapacitacion);
                var pago = JsonConvert.DeserializeObject<Pago>(jsonPago);
                repository.modificarFormulario(persona, institucion, capacitacion, pago, id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
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
        [Route("getPersona")]
        public dynamic getPersona(int folio)
        {
            try
            {
                return repository.getPersona(folio);


            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
