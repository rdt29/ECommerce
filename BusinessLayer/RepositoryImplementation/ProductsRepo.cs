using AutoMapper;
using BusinessLayer.Repository;
using DataAcessLayer.DBContext;
using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.AspNetCore.Mvc;


namespace BusinessLayer.RepositoryImplementation
{
    public class ProductsRepo : IProduct
    {
        private readonly EcDbContext _db;
        private readonly IMapper _mapper;
        public static IWebHostEnvironment _webHostEnvironment;


        public ProductsRepo(EcDbContext db, IMapper mapper ,IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ProductDTO> AddProductAsync(ProductDTO product, int userId, string Filepath)
        {
            try
            {
                var Pro = _mapper.Map<Products>(product);
                Pro.CreatedAt = DateTime.Now;
                Pro.CreatedBy = userId;
                Pro.UserId = userId;
                Pro.Id = product.Id;
                Pro.ImageURL = Filepath;

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
            var filename =await  _db.Products.Where(x=>x.Id == id).Select(x=>x.ImageURL).FirstOrDefaultAsync();
            return filename ;

            }
            catch (Exception)
            {

                throw;
            }
           
           
        }
    }
}