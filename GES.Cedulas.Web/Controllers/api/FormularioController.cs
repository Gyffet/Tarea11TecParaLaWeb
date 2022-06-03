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
    }
}
