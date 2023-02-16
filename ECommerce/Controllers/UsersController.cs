using BusinessLayer.Repository;
using DataAcessLayer.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUser _user;

        public UsersController(IUser user)
        {
            _user = user;
        }

        [HttpGet("Get All Users"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var res = await _user.AllUsers();

            return Ok(res);
        }

        [HttpPost("Add Customer")]
        public IActionResult AddCustomer(UserDTO obj)
        {
            if (obj == null)
            {
                return BadRequest("Name cant be null");
            }
            int role = 1;
            var res = _user.AddUserasync(obj, role);

            return Ok(res);
        }

        [HttpPost("Add Suppiler"), Authorize(Roles = "Admin")]
        public IActionResult AddSuppiler(UserDTO obj)
        {
            if (obj == null)
            {
                return BadRequest("Name cant be null");
            }
            int role = 3;
            var res = _user.AddUserasync(obj, role);

            return Ok(res);
        }

        [HttpPost("Login")]
        public IActionResult Login(int UserId)
        {
            if (UserId == null)
            {
                return BadRequest("UserId can't be blank");
            }
            var res = _user.Login(UserId);
            return Ok(res);
        }
    }
}