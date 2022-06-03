using Formulario.Entities.Login;
using Formulario.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SISEGReferenceProduccion;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Formulario.Controllers.api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AutorizacionController : ControllerBase
    {
        private readonly IOptions<AppSettings> _appSettings;
        public AutorizacionController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult> Auth([FromBody] LoginUser user)
        {
            try
            {
                var client = new ServicioSeguridadClient();
                ValidaUsuarioPorSistemaCompletoResponse validacion = await client.ValidaUsuarioPorSistemaCompletoAsync(277, user.User, user.Password, "");

                TServicioValidacion validacionRespuesta = validacion.ValidaUsuarioPorSistemaCompletoResult;
                if (validacionRespuesta.EsValido)
                {
                    diffgram datosGenerales = Utilerias.Deserialize<diffgram>(validacionRespuesta.DatosUsuario.Nodes[1].ToString());

                    //cambio de clase por acento en Á (Áreas)
                    DataUserAux aux = new DataUserAux();
                    aux.datosGenerales = datosGenerales.NewDataSet.DatosGenerales;
                    aux.areas = datosGenerales.NewDataSet.Áreas;
                    aux.roles = datosGenerales.NewDataSet.Roles;


                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_appSettings.Value.Secret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, datosGenerales.NewDataSet.Roles.cve_sistema.ToString()),//clave del sistema
                            new Claim(ClaimTypes.Role, datosGenerales.NewDataSet.Roles.rol_rd.ToString()),//rol 1, Administrador, 2 enlace, 3 usuario
                            new Claim(ClaimTypes.UserData, datosGenerales.NewDataSet.DatosGenerales.exp.ToString()),//numero de expediente
                            new Claim(ClaimTypes.Email, datosGenerales.NewDataSet.DatosGenerales.correo_electronico.ToString()),//correo electronico
                            new Claim(ClaimTypes.GivenName, datosGenerales.NewDataSet.Áreas.cveArea.ToString())//administración general
                        }),
                        Expires = DateTime.UtcNow.AddDays(15),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    Usuario userR = new Usuario();
                    userR.newDataSet = aux;
                    userR.AccessToken = tokenHandler.WriteToken(token);

                    return Ok(userR);
                }
                else
                {
                    return BadRequest(validacion.ValidaUsuarioPorSistemaCompletoResult.MensajeError);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("fecha")]
        public IActionResult fecha()
        {
            try
            {
                return Ok(DateTime.Now);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<ActionResult> DatosUsuario()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}