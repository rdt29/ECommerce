using BusinessLayer.Repository;
using DataAcessLayer.DBContext;
using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> AddRoles(RoleDTO Role)
        {
            if (Role == null)
            {
                return BadRequest("Role Cant be Null");
            }
            var res = _role.GetRolesAsync(Role);
            return Ok(res);
        }

        [HttpGet("view-role")]
        public async Task<IActionResult> ViewRoles()
        {
            var res = await _role.ViewRolesAsync();
            return Ok(res);
        }
    }
}