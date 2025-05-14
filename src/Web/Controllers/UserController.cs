using Application.dto.response;
using Application.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        [HttpGet]
        public ActionResult<List<UserDTO>>? GetAll()
        {
            List<UserDTO>? users = _userService.GetAll();
            return Ok(users);
        }


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

        [HttpPost]
        [Authorize(Policy = "AnonymousOnly")]
        public ActionResult Add([FromBody]CreateUserDTO newUserData)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                UserDTO? created = _userService.AddUser(newUserData);

                if (created != null)
                {
                    return CreatedAtAction("Get", "User", new { id = created.Id }, created);
                }

                return BadRequest(new { Message = "No se pudo crear el cliente" });

            }catch (Exception e)
            {
                return BadRequest(e.Message);
            }
           
        }

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

    }
}
