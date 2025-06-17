using System.Security.Claims;
using Application.interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class InvitationController : ControllerBase
    {

        private readonly IInvitationService _invitationService;
        public InvitationController(IInvitationService invitationService)
        {
            _invitationService = invitationService;
        }


        /// <summary>
        /// Invita a un usuario a un grupo.
        /// </summary>
        /// <param name="userId">ID del usuario a invitar.</param>
        /// <param name="teamId">ID del grupo al que se invita.</param>
        /// <returns>Respuesta exitosa si la invitación se crea correctamente.</returns>
        /// <response code="200">Invitación creada exitosamente.</response>
        /// <response code="400">Datos inválidos o no se pudo crear la invitación.</response>
        /// <response code="401">No autorizado: se requiere un token JWT válido.</response>
        /// <remarks>
        /// Este endpoint requiere autenticación JWT. El usuario autenticado será considerado el propietario de la invitación.
        /// </remarks>
        [HttpPost("{userId}")]
        [Authorize]
        public ActionResult Invite(int userId, int teamId)
        {
            int invitatedId = userId;
            int ownerId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
           bool invited =  _invitationService.InviteUser(invitatedId, ownerId, teamId);
            if (invited == true)
            {
                return Ok();
            }
            return BadRequest();
        }


        /// <summary>
        /// Acepta una invitación pendiente.
        /// </summary>
        /// <param name="id">ID de la invitación a aceptar.</param>
        /// <returns>Respuesta exitosa si la invitación se acepta correctamente.</returns>
        /// <response code="200">Invitación aceptada exitosamente.</response>
        /// <response code="400">Invitación no encontrada o no se pudo aceptar.</response>
        /// <response code="401">No autorizado: se requiere un token JWT válido.</response>
        /// <remarks>
        /// Este endpoint requiere autenticación JWT. Solo el usuario invitado puede aceptar la invitación.
        /// </remarks>
        [HttpPut("Accept/{id}")]
        [Authorize]
        public ActionResult AcceptInvitation(int id)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            bool accepted = _invitationService.AcceptInvite(userId, id);
            if (accepted == true)
            {
                return Ok();
            }
            return BadRequest();
        }

        /// <summary>
        /// Obtiene todas las invitaciones pendientes de un usuario.
        /// </summary>
        /// <param name="userId">ID del usuario cuyas invitaciones pendientes se desean consultar.</param>
        /// <returns>Lista de invitaciones pendientes.</returns>
        /// <response code="200">Invitaciones pendientes obtenidas exitosamente.</response>
        /// <response code="401">No autorizado: se requiere un token JWT válido.</response>
        /// <remarks>
        /// Este endpoint requiere autenticación JWT.
        /// </remarks>
        [HttpGet("[action]/{userId}")]
        [Authorize]
        public ActionResult<List<Invitation>> GetPending(int userId)
        {
            return Ok(_invitationService.GetByUserId(userId));
        }




        /// <summary>
        /// Rechaza una invitación pendiente.
        /// </summary>
        /// <param name="id">ID de la invitación a rechazar.</param>
        /// <returns>Respuesta exitosa si la invitación se rechaza correctamente.</returns>
        /// <response code="200">Invitación rechazada exitosamente.</response>
        /// <response code="400">Invitación no encontrada o no se pudo rechazar.</response>
        /// <response code="401">No autorizado: se requiere un token JWT válido.</response>
        /// <remarks>
        /// Este endpoint requiere autenticación JWT. Solo el usuario invitado puede rechazar la invitación.
        /// </remarks>
        [HttpPut("reject/{id}")]
        [Authorize]
        public ActionResult RejectInvitation(int id)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            bool rejected = _invitationService.RejectInvitation(userId, id);
            if (rejected == true)
            {
                return Ok();
            }
            return BadRequest();
        }

    }
}
