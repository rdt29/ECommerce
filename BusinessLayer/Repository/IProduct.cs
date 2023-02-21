using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Repository
{
    public interface IProduct
    {
        Task<Products> AddProductAsync(ProductDTO obj, int userid);

        Task<List<ProductDTO>> View();

        Task<string> DeleteProducts(int id);

        Task<ProductDTO> UpdateProduct(ProductDTO obj);
    }
}