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


        #region GetAlluser
        [HttpGet("get-all-users"), Authorize(Roles = "Admin")]
        public async Task<IList<UserResponseDTO>> GetAll()
        {
            var res = await _user.AllUsers();

            return res;
        }

        #endregion GetAllUser

        #region delete-user-by-id

        [HttpDelete("delete-user-by-id")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var res = await _user.DeleteUser(id);

            return Ok(res);
        }

        #endregion delete-user-by-id

        #region AddCustomer

        [HttpPost("add/customer")]
        public async Task<UserDTO> AddCustomer(UserDTO obj)
        {
            //if (obj == null)
            //{
            //    return BadRequest("Name cant be null");
            //}
            int role = 3;
            var res = await _user.AddUserasync(obj, role);

            return (res);
        }

        #endregion AddCustomer 


        #region AddSuppiler 

        [HttpPost("add-suppiler"), Authorize(Roles = "Admin")]
        public async Task<UserDTO> AddSuppiler(UserDTO obj)
        {
            //if (obj == null)
            //{
            //    return BadRequest("Name cant be null");
            //}
            int role = 2;
            var res = await _user.AddUserasync(obj, role);

            return (res);
        }

        #endregion AddSuplier


        #region Login

        [HttpPost("login")]
        public IActionResult Login(int UserId)
        {
            if (UserId == null)
            {
                return BadRequest("UserId can't be blank");
            }
            var res = _user.Login(UserId);
            return Ok(res);
        }


        #endregion login
    }
}