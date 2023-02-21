using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Entity
{
    public class OrderDetails : Audit
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int? ProductId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public OrdersTable OrdersTable { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Products? Product { get; set; }
    }
}