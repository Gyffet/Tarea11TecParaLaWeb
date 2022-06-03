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
        public IActionResult guardarFormulario(string jsonPersona, string jsonIntitucion, string jsonCapacitacion)
        {
            try
            {
                //var a = JsonConvert.DeserializeObject<Entregables>(jsonPersona);
                repository.guardarFormulario();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
