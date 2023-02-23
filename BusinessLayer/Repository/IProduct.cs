using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Repository
{
    public interface IProduct
    {
        Task<ProductDTO> AddProductAsync(ProductDTO obj, int userid, string Filepath);

        Task<List<ProductResponceDTO>> View();

        Task<string> GetProductImage(int id);

        Task<string> DeleteProducts(int id);

        Task<ProductDTO> UpdateProduct(ProductDTO obj, int userId);
    }
}