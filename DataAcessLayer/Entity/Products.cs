using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Entity
{
    public class Products
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int price { get; set; }
        public string ProductDescription { get; set; }
        public int CategoryID { get; set; }

        public int UserId { get; set; }

        public ICollection<OrderDetails> OrderDetail { get; set; }

        #region Navigation

        [ForeignKey(nameof(UserId))]
        public User user { get; set; }

        [ForeignKey(nameof(CategoryID))]
        public Category Category { get; set; }

        #endregion Navigation
    }
}