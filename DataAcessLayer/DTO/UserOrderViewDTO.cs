using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.DTO
{
    public class UserOrderViewDTO
    {
        public string UserName { get; set; }
        public int UserId { get; set; }
        public int OrderID { get; set; }
        public int TotalPrice { get; set; }

        public ICollection<UserOrderProductsViewDTO> Productdet { get; set; }
        //public string[] ProductName { get; set; }

        //public int[] productprice { get; set; }
        //public Dictionary<string, string> ProductName = new Dictionary<string, string>();
    }
}