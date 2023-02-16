using DataAcessLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Repository
{
    public interface IProduct
    {
        Task<ProductDTO> AddProductAsync(ProductDTO obj);
    }
}