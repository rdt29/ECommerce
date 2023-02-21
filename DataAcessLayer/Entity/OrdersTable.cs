using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Entity
{
    public class OrdersTable
    {
        [Key]
        public int ID { get; set; }

        public string? ShippingDetails { get; set; }
        public int CustomerID { get; set; }
        public int TotalPrice { get; set; }

        public ICollection<OrderDetails> OrderDetails { get; set; }

        #region Navigation

        [ForeignKey(nameof(CustomerID))]
        public User Users { get; set; }

        #endregion Navigation
    }
}