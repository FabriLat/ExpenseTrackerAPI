using System.Security.Claims;
using Application.dto.request;
using Application.dto.response;
using Application.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class TeamController : ControllerBase
    {

        private readonly ITeamService _teamService;
        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }


        /// <summary>
        /// Crea un nuevo grupo.
        /// </summary>
        /// <param name="createTeamDto">Datos del nuevo grupo.</param>
        /// <returns>Respuesta con el grupo creado.</returns>
        /// <response code="201">Equipo creado exitosamente.</response>
        /// <response code="400">Datos inválidos para la creación del grupo.</response>
        /// <response code="401">No autorizado: se requiere un token JWT válido.</response>
        /// <remarks>
        /// Este endpoint requiere autenticación JWT. El usuario autenticado será considerado el creador del grupo.
        /// </remarks>
        [HttpPost]
        [Authorize]
        public ActionResult Create([FromBody]CreateTeamDTO createTeamDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int creatorId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            TeamDTO? created = _teamService.CreateTeam(createTeamDto, creatorId);
            if (created != null)
            {

                return CreatedAtAction("Get", "Team", new { id = created.Id }, created);
            }
            return BadRequest();
        }


        /// <summary>
        /// Obtiene un grupo por su ID.
        /// </summary>
        /// <param name="id">ID del grupo a consultar.</param>
        /// <returns>Detalles del grupo solicitado.</returns>
        /// <response code="200">Equipo encontrado y devuelto exitosamente.</response>
        /// <response code="404">Equipo no encontrado.</response>
        /// <remarks>
        /// No requiere autenticación para consultar el grupo.
        /// </remarks>
        [HttpGet("{id}")]
        public ActionResult<TeamDTO?> Get(int id)
        {
            TeamDTO? dto = _teamService.GetById(id);

            if (dto != null)
                return dto;

            return NotFound();
        }



        /// <summary>
        /// Actualiza un grupo existente.
        /// </summary>
        /// <param name="id">ID del grupo a actualizar.</param>
        /// <param name="dto">Datos actualizados del grupo.</param>
        /// <returns>Respuesta exitosa si el grupo se actualiza correctamente.</returns>
        /// <response code="200">Equipo actualizado exitosamente.</response>
        /// <response code="400">Datos inválidos para la actualización.</response>
        /// <response code="401">No autorizado: se requiere un token JWT válido.</response>
        /// <response code="403">Prohibido: el usuario no tiene permiso para actualizar el grupo.</response>
        /// <remarks>
        /// Este endpoint requiere autenticación JWT. Solo el propietario del grupo puede actualizarlo.
        /// </remarks>
        [HttpPut("{id}")]
        [Authorize]
        public ActionResult Update(int id, UpdateTeamDTO dto)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_teamService.UpdateTeam(id, userId, dto))
            {
                return Ok();
            }
            return Forbid();
        }


        /// <summary>
        /// Permite a un usuario salir de un grupo.
        /// </summary>
        /// <param name="leaveTeamDTO">Datos necesarios para salir del grupo, incluyendo el ID del grupo y, si es el propietario, el ID del nuevo propietario.</param>
        /// <returns>Respuesta exitosa si el usuario sale del grupo correctamente.</returns>
        /// <response code="200">Usuario salió del grupo exitosamente.</response>
        /// <response code="400">Datos inválidos, el usuario no está en el grupo o no se especificó un nuevo propietario válido.</response>
        /// <response code="401">No autorizado: se requiere un token JWT válido.</response>
        /// <remarks>
        /// Este endpoint requiere autenticación JWT. Si el usuario es el propietario del grupo, debe especificar un nuevo propietario en el DTO.
        /// </remarks>
        [HttpPut("[action]")]
        [Authorize]
        public ActionResult Leave(LeaveTeamDTO leaveTeamDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            if(_teamService.LeaveTeam(userId, leaveTeamDTO))
            {
                return Ok();
            }
            return BadRequest();
        }



        /// <summary>
        /// Elimina un grupo existente.
        /// </summary>
        /// <param name="id">ID del grupo a eliminar.</param>
        /// <returns>Respuesta sin contenido si el grupo se elimina correctamente.</returns>
        /// <response code="204">Equipo eliminado exitosamente.</response>
        /// <response code="401">No autorizado: se requiere un token JWT válido.</response>
        /// <response code="403">Prohibido: el usuario no tiene permiso para eliminar el grupo.</response>
        /// <remarks>
        /// Este endpoint requiere autenticación JWT. Solo el propietario del grupo puede eliminarlo.
        /// </remarks>
        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult Delete(int id)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            bool deleted = _teamService.DeleteTeam(id, userId);
            if (deleted)
            {
                return NoContent();
            }
            return Forbid();
        }


        /// <summary>
        /// Elimina a un usuario de un equipo.
        /// </summary>
        /// <param name="teamId">El ID del equipo del cual se eliminará al usuario.</param>
        /// <param name="userId">El ID del usuario a eliminar del equipo.</param>
        /// <returns>Respuesta exitosa si el usuario es eliminado del equipo correctamente.</returns>
        /// <response code="200">Usuario eliminado del equipo exitosamente.</response>
        /// <response code="400">No se pudo eliminar al usuario, el equipo no existe o el usuario no está en el equipo.</response>
        /// <response code="401">No autorizado: se requiere un token JWT válido.</response>
        /// <response code="403">Prohibido: solo el propietario del equipo puede eliminar usuarios.</response>
        /// <remarks>
        /// Este endpoint requiere autenticación JWT. Solo el propietario del equipo puede eliminar a un usuario del equipo.
        /// </remarks>
        [HttpPut("RemoveUser/{userId}")]
        [Authorize]
        public ActionResult RemoveUser(int teamId, int userId)
        {
            int ownerId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            bool removed = _teamService.RemoveUser(userId, teamId, ownerId);
            if(removed)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
