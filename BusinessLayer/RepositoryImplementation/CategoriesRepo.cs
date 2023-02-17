using BusinessLayer.Repository;
using DataAcessLayer.DBContext;
using DataAcessLayer.Entity;
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

        public async Task<IList<Category>> GetAllCategories()
        {
            try
            {
                var cat = await _db.Categories.Include(x => x.Products).ToListAsync();
                return (cat);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}