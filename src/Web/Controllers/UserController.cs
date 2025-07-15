using System.Security.Claims;
using Application.dto.request;
using Application.dto.response;
using Application.interfaces;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        public UserController(IUserService UserService)
        {
            _userService = UserService;
        }

        /// <summary>
        /// Obtiene la lista de todos los usuarios.
        /// </summary>
        /// <returns>Una lista de usuarios.</returns>
        /// <response code="200">Devuelve la lista de usuarios.</response>
        [HttpGet]
        public ActionResult<List<UserDTO>>? GetAll()
        {
            List<UserDTO>? users = _userService.GetAll();
            return Ok(users);
        }


        /// <summary>
        /// Obtiene un usuario por su ID.
        /// </summary>
        /// <param name="id">El ID del usuario.</param>
        /// <returns>El usuario encontrado.</returns>
        /// <response code="200">Usuario encontrado.</response>
        /// <response code="404">Usuario no encontrado.</response>
        [HttpGet("{id}")]
        public ActionResult<UserDTO?> Get(int id)
        {
            var user = _userService.GetById(id);
            if(user != null)
            {
                return user;
            }
            return NotFound();
        }


        /// <summary>
        /// Crea un nuevo usuario.
        /// </summary>
        /// <param name="newUserData">Datos del nuevo usuario.</param>
        /// <returns>El usuario creado.</returns>
        /// <response code="201">Usuario creado exitosamente.</response>
        /// <response code="400">Datos inválidos o error al crear el usuario.</response>
        /// <remarks>
        /// Este endpoint solo permite acceso a usuarios no autenticados (política "AnonymousOnly").
        /// No requiere token JWT.
        /// </remarks>
        [HttpPost]
        [Authorize(Policy = "AnonymousOnly")]
        public ActionResult Add([FromBody]CreateUserDTO newUserData)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            UserDTO? created = _userService.AddUser(newUserData);
            if (created != null)
            {
                return CreatedAtAction("Get", "User", new { id = created.Id }, created);
            }
            return BadRequest(new { Message = "No se pudo crear el cliente" });
            
        }



        /// <summary>
        /// Actualiza los datos de un usuario.
        /// </summary>
        /// <param name="newUserData">Datos actualizados del usuario.</param>
        /// <returns>Respuesta con los datos actualizados del usuario si la operación es exitosa.</returns>
        /// <response code="200">Usuario actualizado exitosamente.</response>
        /// <response code="400">Datos inválidos o no se pudo actualizar el usuario.</response>
        /// <response code="401">No autorizado: se requiere un token JWT válido.</response>
        /// <remarks>
        /// Este endpoint requiere autenticación JWT. Solo el usuario autenticado puede actualizar sus propios datos.
        /// </remarks>
        [HttpPut]
        [Authorize]
        public ActionResult Update(UpdateUserDTO newUserData)
        {

            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            var userData = _userService.UpdateUser(userId, newUserData);
            if (userData != null)
                return Ok(userData);
            return NotFound();

        }



        /// <summary>
        /// Elimina un usuario por su ID.
        /// </summary>
        /// <param name="id">El ID del usuario a eliminar.</param>
        /// <returns>No content si se elimina correctamente.</returns>
        /// <response code="204">Usuario eliminado exitosamente.</response>
        /// <response code="400">Error al eliminar el usuario.</response>
        /// <remarks>
        /// Este endpoint requiere autenticación JWT.
        /// </remarks>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            bool deleted = _userService.Delete(id);
            if (deleted == true)
            {
                return NoContent();
            }
            return BadRequest();
        }

        /// <summary>
        /// Obtiene una lista de usuarios que coinciden con el nombre completo proporcionado.
        /// </summary>
        /// <param name="fullName">El nombre completo del usuario a buscar.</param>
        /// <returns>Una lista de objetos UserDTO que coinciden con el nombre proporcionado.</returns>
        /// <response code="200">Devuelve la lista de usuarios encontrados.</response>
        /// <remarks>
        /// Este endpoint no requiere autenticación.
        /// </remarks>
        [HttpGet("[action]")]
        public ActionResult<List<UserDTO>> GetByName(string fullName)
        {
            List<UserDTO> users = _userService.GetByName(fullName);
            return Ok(users);
        }

    }
}
