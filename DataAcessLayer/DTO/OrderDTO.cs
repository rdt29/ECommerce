using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.DTO
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int[] ProductID { get; set; }
        public int ShipingID { get; set; }
    }
}