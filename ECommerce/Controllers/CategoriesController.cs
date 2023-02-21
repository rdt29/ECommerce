using BusinessLayer.Repository;
using DataAcessLayer.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategories _categories;

        public CategoriesController(ICategories categories)
        {
            _categories = categories;
        }

        [HttpPost("add-categories"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCategories(String Name, int Id)
        {
            var res = await _categories.CategoryAdd(Name, Id);
            return Ok(res);
        }

        [HttpGet("view-categories")]
        public async Task<IActionResult> ViewCategories()
        {
            var res = await _categories.GetAllCategories();
            return Ok(res);
        }

        [HttpDelete("Delete-by-id")]
        public async Task<IActionResult> DeleteCategories(int id)
        {
            var delcat = await _categories.DeleteCategories(id);
            return Ok(delcat);
        }

        [HttpPut("update-categories")]
        public async Task<IActionResult> DeleteCategories(string name, int id)
        {
            var update = await _categories.UpdateCategories(name, id);
            return Ok(update);
        }
    }
}