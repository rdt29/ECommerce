using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Repository
{
    public interface ICategories
    {
        public Task<Category> CategoryAdd(string name, int id , int userId);

        public Task<IList<Category>> GetAllCategories();

        public Task<Category> DeleteCategories(int id);

        public Task<string> UpdateCategories(string name, int id, int UserId);
    }
}