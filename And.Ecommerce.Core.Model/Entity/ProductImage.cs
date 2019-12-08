using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace And.Ecommerce.Core.Model.Entity
{
    public class ProductImage:EntityBase
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string ImageUrl { get; set; }
    }
}
