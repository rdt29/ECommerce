using BusinessLayer.Repository;
using DataAcessLayer.DBContext;
using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.RepositoryImplementation
{
    public class CategoriesRepo : ICategories
    {
        private readonly EcDbContext _db;

        public CategoriesRepo(EcDbContext db)
        {
            _db = db;
        }

        public async Task<Category> CategoryAdd(string Name, int Id)
        {
            try
            {
                Category obj = new Category
                {
                    CategoryName = Name,
                    Id = Id
                };
                await _db.Categories.AddAsync(obj);
                await _db.SaveChangesAsync();
                return (obj);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Category> DeleteCategories(int id)
        {
            try
            {
                var category = _db.Categories.Where(x => x.Id == id).FirstOrDefault();

                if (category == null)
                {
                    throw new Exception("Category not Found");
                }
                _db.Categories.Remove(category);
                await _db.SaveChangesAsync();
                return (category);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IList<Category>> GetAllCategories()
        {
            try
            {
                //var cat = await _db.Products.Include(x => x.Category)
                //    .Select(x => new AllCategoriesDTO
                //    {
                //        categoryName = x.Category.CategoryName,
                //        ID = x.CategoryID,
                //        CatProduct = new AllCategoryProductDTO()
                //        {
                //            price = x.price,
                //            ProductAddedByName = x.ProductName,
                //            productName = x.ProductName,
                //            ProductId = x.Id,
                //        }
                //    }).ToListAsync();
                //return (cat);

                var cat = await _db.Categories.Include(x => x.Products).ToListAsync();

                return (cat);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> UpdateCategories(string name, int id)
        {
            try
            {
                var cat = _db.Categories.Where(x => x.Id == id).FirstOrDefault();
                if (cat == null)
                {
                    throw new Exception("Categories not Found");
                }

                cat.CategoryName= name;
                
                _db.Categories.Update(cat);
                await _db.SaveChangesAsync();
                return ($"Categories Name {name} is Updates Sucessfully");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}