using BusinessLayer.Repository;
using DataAcessLayer.DBContext;
using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RepositoryImplementation
{
    public class ProductsRepo : IProduct
    {
        private readonly EcDbContext _db;

        public ProductsRepo(EcDbContext db)
        {
            _db = db;
        }

        public async Task<ProductDTO> AddProductAsync(ProductDTO product)
        {
            try
            {
                Products pro = new Products()
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    price = product.price,
                    CategoryID = product.CategoryID,
                    UserId = product.UserId,
                };
                await _db.Products.AddAsync(pro);
                await _db.SaveChangesAsync();

                return (product);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}