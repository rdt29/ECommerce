using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.DTO
{
    public class AllCategoryProductDTO
    {
        public int ProductId { get; set; }
        public string productName { get; set; }
        public int price { get; set; }
        public string productDescription { get; set; }
        public int ProductAddedById { get; set; }
        public string ProductAddedByName { get; set; }

        public ICollection<AllCategoriesDTO> AllCategories { get; set; }
    }
}