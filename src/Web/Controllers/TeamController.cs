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


        [HttpGet("{id}")]
        public ActionResult<TeamDTO?> Get(int id)
        {
            return Ok(_teamService.GetById(id));
        }



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
