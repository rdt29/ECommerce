using BusinessLayer.Repository;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Net;
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

        #region AddingProducts

        [HttpPost("AddingProducts"), Authorize(Roles = "Admin , Supplier")]
        public async Task<IActionResult> AddProducts([FromForm] ProductDTO obj)
        {
            var CategoriesFolderName = await _product.GetProductCategories(obj.CategoryID);

            #region localfile

            //string path = _webHostEnvironment.WebRootPath + "\\uploads\\";
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}
            //using (FileStream filestream = System.IO.File.Create(path + obj.fileupload.FileName))
            //{
            //    obj.fileupload.CopyTo(filestream);
            //    filestream.Flush();
            //    //return ($"Uploaded Done {path + fileupload.FileName}");
            //    filepath = path + obj.fileupload.FileName;
            //}

            //if (obj == null)
            //{
            //    return BadRequest("Details can't be null");
            //}

            #endregion localfile

            #region Cloudfair

            var cloudinary = new Cloudinary(new Account("dzwma7qcb", "591491493691445", "2bJuC3k7biQk4uZBwVu8Ok9YpPk"));

            // Upload

            var stream = obj.fileupload.OpenReadStream();

            var uploadParams = new ImageUploadParams()
            {
                //File = new FileDescription(@fileupload),
                File = new FileDescription(obj.fileupload.Name, stream),
                PublicId = obj.fileupload.FileName,
                Folder = CategoriesFolderName,
            };

            var uploadResult = cloudinary.Upload(uploadParams);

            //return Ok(uploadResult);

            var filepath = uploadResult.SecureUrl.ToString();

            #endregion Cloudfair

            string id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            int userId = Convert.ToInt32(id);
            var product = await _product.AddProductAsync(obj, userId, filepath);
            return Ok(product);
        }

        #endregion AddingProducts

        #region ViewProducts

        [HttpGet("view-all-products")]
        public async Task<IActionResult> ViewProducts()
        {
            var res = await _product.View();
            return Ok(res);
        }

        #endregion ViewProducts

        #region VIewProductsImage

        [HttpGet("view-products-Image-id")]
        public async Task<IActionResult> GetImage(int id)
        {
            var filename = await _product.GetProductImage(id);

            #region localpath

            //string path = _webHostEnvironment.WebRootPath + "\\uploads\\";

            //var filepath = path + filename + ".png";
            //var filepath = filename;
            //if (!System.IO.File.Exists(filepath))
            //{
            //    return BadRequest("No image  found");
            //}
            //byte[] b = System.IO.File.ReadAllBytes(filepath);
            //return File(b, "image/png");

            #endregion localpath

            #region cloudfare

            // get the image from the URL

            //var imageData = new WebClient().DownloadData(filename);
            var imageData = new WebClient().DownloadData(filename);

            // return the image as a file
            return File(imageData, "image/png");

            #endregion cloudfare
        }

        #endregion VIewProductsImage

        #region DeleteProducts

        [HttpDelete("delete-product-byid")]
        public async Task<IActionResult> DeleteProducts(int id)
        {
            var res = await _product.DeleteProducts(id);
            return Ok(res);
        }

        #endregion DeleteProducts

        #region UpdateProducts

        [HttpPut("update-products"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProdcuts(ProductDTO obj)
        {
            string Uid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = Convert.ToInt32(Uid);
            var res = await _product.UpdateProduct(obj, userId);
            return Ok(res);
        }

        #endregion UpdateProducts
    }
}