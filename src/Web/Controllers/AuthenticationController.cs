using Application.dto.request;
using Application.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticationService _authenticationService;

        private readonly IConfiguration _config;

        public AuthenticationController(IAuthenticationService authenticationService, IConfiguration config)
        {
            _authenticationService = authenticationService;
            _config = config;
        }

        /// <summary>
        /// Autentica a un usuario y devuelve un token JWT.
        /// </summary>
        /// <param name="authenticationRequest">Credenciales del usuario (email y contraseña).</param>
        /// <returns>Un token JWT como string.</returns>
        /// <response code="200">Autenticación exitosa, devuelve el token JWT.</response>
        /// <response code="400">Credenciales inválidas o error en la autenticación.</response>
        /// <remarks>
        /// Este endpoint no requiere autenticación previa. Proporciona un token JWT que puede usarse en endpoints protegidos.
        /// </remarks>  

        [HttpPost]
        public ActionResult<string> Authenticate([FromBody] AuthenticationRequest authenticationRequest)
        {
            try
            {
                //Llama a un metodo que devuelve un string-Token
                string token = _authenticationService.Authenticate(authenticationRequest);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}