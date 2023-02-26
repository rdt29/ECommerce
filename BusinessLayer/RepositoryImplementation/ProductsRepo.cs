using AutoMapper;
using BusinessLayer.Repository;
using DataAcessLayer.DBContext;
using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs.Models;
using System.Reflection.Metadata;

namespace BusinessLayer.RepositoryImplementation
{
    public class ProductsRepo : IProduct
    {
        private readonly EcDbContext _db;
        private readonly IMapper _mapper;
        public static IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public ProductsRepo(EcDbContext db, IMapper mapper, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _db = db;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        public async Task<ProductDTO> AddProductAsync(ProductDTO product, int userId)
        {
            try
            {
                var CategoriesFolderName = await _db.Categories.Where(x => x.Id == product.CategoryID).Select(x => x.CategoryName).FirstOrDefaultAsync();

                #region AzureBlob

                var guid = Guid.NewGuid().ToString();

                var filepath = "";
                if (product.fileupload.Length > 0)
                {
                    //var container = new BlobContainerClient("DefaultEndpointsProtocol=https;AccountName=rdtecommerce121;AccountKey=B0b5OdXjAplPEKu6zimtq6uxPbwl2zYO+Kaw1S4xifpVSrf8fL25gTM08Fmcgppm7lS2jbfRyr7n+AStiN3fGQ==;EndpointSuffix=core.windows.net", "electronic");
                    var container = new BlobContainerClient(_configuration.GetConnectionString("AzureBlobStorage"), CategoriesFolderName.ToLower());

                    var createResponse = await container.CreateIfNotExistsAsync();
                    if (createResponse != null && createResponse.GetRawResponse().Status == 201)
                        await container.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

                    var Newfilename = guid + product.fileupload.FileName;
                    var blob = container.GetBlobClient(Newfilename);
                    //await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
                    using (var fileStream = product.fileupload.OpenReadStream())
                    {
                        await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = product.fileupload.ContentType });
                    }

                    filepath = blob.Uri.ToString();
                    //return Ok(blob.Uri.ToString());
                }

                //return BadRequest();

                #endregion AzureBlob

                var Pro = _mapper.Map<Products>(product);
                Pro.CreatedAt = DateTime.Now;
                Pro.CreatedBy = userId;
                Pro.UserId = userId;
                Pro.Id = product.Id;
                Pro.ImageURL = filepath;

                //Products pro = new Products()
                //{
                //    Id = product.Id,
                //    ProductName = product.ProductName,
                //    ProductDescription = product.ProductDescription,
                //    price = product.price,
                //    CategoryID = product.CategoryID,
                //    UserId = userId,
                //    CreatedAt = DateTime.Now,
                //    CreatedBy = userId,
                //};
                await _db.Products.AddAsync(Pro);
                await _db.SaveChangesAsync();

                return (_mapper.Map<ProductDTO>(Pro));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProductResponceDTO>> View()
        {
            try
            {
                var products = await _db.Products.Include(x => x.Category).ToListAsync();
                List<ProductResponceDTO> Productresult = new List<ProductResponceDTO>();
                foreach (var i in products)
                {
                    ProductResponceDTO pro = new ProductResponceDTO()
                    {
                        ImageURL = i.ImageURL,
                        Id = i.Id,
                        ProductName = i.ProductName,
                        ProductDescription = i.ProductDescription,
                        CategoryID = i.CategoryID,

                        price = i.price,
                    };
                    Productresult.Add(pro);

                    //var Pro = _mapper.Map<ProductDTO>(Productresult);
                    //Productresult.Add(Pro);
                }
                return Productresult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> DeleteProducts(int id)
        {
            try
            {
                var productDelete = _db.Products.Where(x => x.Id == id).FirstOrDefault();
                if (productDelete == null)
                {
                    throw new Exception("No Product Found");
                }

                _db.Products.Remove(productDelete);
                _db.SaveChangesAsync();
                return ($"Product {productDelete.ProductName} is Deleted Sucessfully ");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProductDTO> UpdateProduct(ProductDTO obj, int userId)
        {
            try
            {
                var productupdate = _db.Products.Where(x => x.Id == obj.Id).FirstOrDefault();

                var pro = _mapper.Map(obj, productupdate); //_mapper.Map<Products>(obj);

                pro.ModifiedAt = DateTime.Now;
                pro.ModifiedBy = userId;
                pro.UserId = userId;

                //productupdate.ProductName = obj.ProductName;
                //productupdate.price = obj.price;
                //productupdate.ProductDescription = obj.ProductDescription;
                //productupdate.CategoryID = obj.CategoryID;
                //productupdate.ModifiedAt = DateTime.Now;
                //productupdate.ModifiedBy = userId;

                _db.Products.Update(pro);
                await _db.SaveChangesAsync();

                return (obj);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> GetProductImage(int id)
        {
            try
            {
                var filename = await _db.Products.Where(x => x.Id == id).Select(x => x.ImageURL).FirstOrDefaultAsync();
                return filename;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public async Task<string> GetProductCategories(int id)
        //{
        //    try
        //    {
        //        var CategoriesName = await _db.Categories.Where(x => x.Id == id).Select(x => x.CategoryName).FirstOrDefaultAsync();
        //        return CategoriesName;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
    }
}