using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Entity
{
    public class Orders
    {
        public int OrderID { get; set; }

        public string ShippingDetails { get; set; }
        public int TotalPrice { get; set; }
    }
}