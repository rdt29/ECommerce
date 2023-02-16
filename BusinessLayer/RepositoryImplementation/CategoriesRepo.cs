using BusinessLayer.Repository;
using DataAcessLayer.DBContext;
using DataAcessLayer.Entity;

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
    }
}