using System.Security.Claims;
using Application.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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


        [HttpPost("{id}")]
        [Authorize]
        public ActionResult Invite(int id, int groupId)
        {
            int invitatedId = id;
            int ownerId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
           bool invited =  _invitationService.InviteUser(invitatedId, ownerId, groupId);
            if (invited == true)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
