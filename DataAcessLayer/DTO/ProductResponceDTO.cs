using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.DTO
{
    public class ProductResponceDTO
    {
        public int Id { get; set; }

        public string ProductName { get; set; }
        public string ImageURL { get; set; }
        public int price { get; set; }
        public string ProductDescription { get; set; }
        public int CategoryID { get; set; }
    }
}