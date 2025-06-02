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
        /// Crea un nuevo equipo.
        /// </summary>
        /// <param name="createTeamDto">Datos del nuevo equipo.</param>
        /// <returns>Respuesta con el equipo creado.</returns>
        /// <response code="201">Equipo creado exitosamente.</response>
        /// <response code="400">Datos inválidos para la creación del equipo.</response>
        /// <response code="401">No autorizado: se requiere un token JWT válido.</response>
        /// <remarks>
        /// Este endpoint requiere autenticación JWT. El usuario autenticado será considerado el creador del equipo.
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
        /// Obtiene un equipo por su ID.
        /// </summary>
        /// <param name="id">ID del equipo a consultar.</param>
        /// <returns>Detalles del equipo solicitado.</returns>
        /// <response code="200">Equipo encontrado y devuelto exitosamente.</response>
        /// <response code="404">Equipo no encontrado.</response>
        /// <remarks>
        /// No requiere autenticación para consultar el equipo.
        /// </remarks>
        [HttpGet("{id}")]
        public ActionResult<TeamDTO?> Get(int id)
        {
            TeamDTO? dto = _teamService.GetById(id);
            return dto;
        }



        /// <summary>
        /// Actualiza un equipo existente.
        /// </summary>
        /// <param name="id">ID del equipo a actualizar.</param>
        /// <param name="dto">Datos actualizados del equipo.</param>
        /// <returns>Respuesta exitosa si el equipo se actualiza correctamente.</returns>
        /// <response code="200">Equipo actualizado exitosamente.</response>
        /// <response code="400">Datos inválidos para la actualización.</response>
        /// <response code="401">No autorizado: se requiere un token JWT válido.</response>
        /// <response code="403">Prohibido: el usuario no tiene permiso para actualizar el equipo.</response>
        /// <remarks>
        /// Este endpoint requiere autenticación JWT. Solo el propietario del equipo puede actualizarlo.
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
            if (_teamService.UpdateTeam(id,userId, dto) == true)
            {
                return Ok();
            }
            return Forbid();
            
        }


        /// <summary>
        /// Elimina un equipo existente.
        /// </summary>
        /// <param name="id">ID del equipo a eliminar.</param>
        /// <returns>Respuesta sin contenido si el equipo se elimina correctamente.</returns>
        /// <response code="204">Equipo eliminado exitosamente.</response>
        /// <response code="401">No autorizado: se requiere un token JWT válido.</response>
        /// <response code="403">Prohibido: el usuario no tiene permiso para eliminar el equipo.</response>
        /// <remarks>
        /// Este endpoint requiere autenticación JWT. Solo el propietario del equipo puede eliminarlo.
        /// </remarks>
        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult Delete(int id)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            bool deleted = _teamService.DeleteTeam(id, userId);
            if (deleted == true)
            {
                return NoContent();
            }
            return Forbid();
        }

    }
}
