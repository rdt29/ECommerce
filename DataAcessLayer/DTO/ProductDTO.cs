using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int price { get; set; }
        public string ProductDescription { get; set; }
        public int CategoryID { get; set; }
        public int UserId { get; set; }
    }
}