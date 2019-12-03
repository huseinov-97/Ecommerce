using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace And.Ecommerce.Core.Model.Entity
{
    public class OrderPayment : EntityBase
    {
        public int OrderID { get; set; }
        public Order Order { get; set; }
        public _OrderType OrderType { get; set; }
        public decimal Price { get; set; }
        public string Bank { get; set; }
    }
    public enum _OrderType
    {
        Transfer = 0,
        Creditcard = 1
    }
}

       