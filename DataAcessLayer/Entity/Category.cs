using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Entity
{
    public class Category : Audit
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }

        public ICollection<Products> Products { get; set; }
    }
}