using BusinessLayer.Repository;
using DataAcessLayer.DBContext;
using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoles _role;

        public RolesController(IRoles role)
        {
            _role = role;
        }

        [HttpPost("add-role"), Authorize(Roles = "Admin")]
        public async Task<RoleDTO> AddRoles(RoleDTO obj)
        {
            int userId = 0;

            string Uid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            userId = Convert.ToInt32(Uid);

            //if (obj == null)
            //{
            //    //return ("Role Cant be Null")
            //    //return (obj);
            //}
            var res = await _role.AddRolesAsync(obj, userId);
            return (res);
        }

        [HttpGet("view-role")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> ViewRoles()
        {
            var res = await _role.ViewRolesAsync();
            return Ok(res);
        }
    }
}