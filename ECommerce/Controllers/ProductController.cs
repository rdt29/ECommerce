using BusinessLayer.Repository;
using DataAcessLayer.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProduct _product;

        public ProductController(IProduct product)
        {
            _product = product;
        }

        [HttpPost("Adding Products")]
        //[Authorize(Roles = "Admin")]
        [Authorize(Roles = "Supplier")]
        public async Task<IActionResult> AddProducts(ProductDTO obj)
        {
            if (obj == null)
            {
                return BadRequest("Details can't be null");
            }
            var product = await _product.AddProductAsync(obj, User.Identity);
            return Ok(product);
        }
    }
}