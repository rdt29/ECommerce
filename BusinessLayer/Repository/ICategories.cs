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
        public Task<Category> CategoryAdd(string name, int id);

        public Task<IList<Category>> GetAllCategories();
    }
}