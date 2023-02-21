using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.DTO
{
    public class UserOrderProductsViewDTO
    {
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public int OrderID { get; set; }
        public int price { get; set; }
    }
}