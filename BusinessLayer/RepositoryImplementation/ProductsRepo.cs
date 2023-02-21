﻿using BusinessLayer.Repository;
using DataAcessLayer.DBContext;
using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Products> AddProductAsync(ProductDTO product, int userId)
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
                    UserId = userId
                };
                await _db.Products.AddAsync(pro);
                await _db.SaveChangesAsync();

                return (pro);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProductDTO>> View()
        {
            try
            {
                var products = await _db.Products.Include(x => x.Category).ToListAsync();
                List<ProductDTO> Productresult = new List<ProductDTO>();
                foreach (var i in products)
                {
                    ProductDTO pro = new ProductDTO()
                    {
                        Id = i.Id,
                        ProductName = i.ProductName,
                        ProductDescription = i.ProductDescription,
                        CategoryID = i.CategoryID,

                        price = i.price,
                    };
                    Productresult.Add(pro);
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

        public async Task<ProductDTO> UpdateProduct(ProductDTO obj)
        {
            try
            {
                var productupdate = _db.Products.Where(x => x.Id == obj.Id).FirstOrDefault();

                productupdate.ProductName = obj.ProductName;
                productupdate.price = obj.price;
                productupdate.ProductDescription = obj.ProductDescription;
                productupdate.CategoryID = obj.CategoryID;

                _db.Products.Update(productupdate);
                _db.SaveChanges();

                return (obj);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}