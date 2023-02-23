using BusinessLayer.Repository;
using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Security.Claims;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProduct _product;
        public static IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProduct product, IWebHostEnvironment webHostEnvironment)
        {
            _product = product;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("AddingProducts")]
        [Authorize(Roles = "Admin , Supplier")]
        public async Task<IActionResult> AddProducts([FromForm] ProductDTO obj)
        {
            var filepath = "";

            string path = _webHostEnvironment.WebRootPath + "\\uploads\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (FileStream filestream = System.IO.File.Create(path + obj.fileupload.FileName))
            {
                obj.fileupload.CopyTo(filestream);
                filestream.Flush();
                //return ($"Uploaded Done {path + fileupload.FileName}");
                filepath = path + obj.fileupload.FileName;
            }

            if (obj == null)
            {
                return BadRequest("Details can't be null");
            }
            string id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = Convert.ToInt32(id);
            var product = await _product.AddProductAsync(obj, userId, filepath);
            return Ok(product);
        }

        [HttpGet("view-all-products")]
        public async Task<IActionResult> ViewProducts()
        {
            var res = await _product.View();
            return Ok(res);
        }

        [HttpGet("view-products-Image-id")]
        public async Task<IActionResult> GetImage(int id)
        {
            var filename = await _product.GetProductImage(id);

            //string path = _webHostEnvironment.WebRootPath + "\\uploads\\";

            //var filepath = path + filename + ".png";
            var filepath = filename;
            if (!System.IO.File.Exists(filepath))
            {
            }
                byte[] b = System.IO.File.ReadAllBytes(filepath);
                return File(b, "image/png");
            //return BadRequest("Not found");
        }

        [HttpDelete("delete-product-byid")]
        public async Task<IActionResult> DeleteProducts(int id)
        {
            var res = await _product.DeleteProducts(id);
            return Ok(res);
        }

        [HttpPut("update-products"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProdcuts(ProductDTO obj)
        {
            string Uid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = Convert.ToInt32(Uid);
            var res = await _product.UpdateProduct(obj, userId);
            return Ok(res);
        }
    }
}