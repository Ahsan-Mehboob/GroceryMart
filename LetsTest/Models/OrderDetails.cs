using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetsTest.Models
{
    public class OrderDetails
    {
        public int PRODUCT_ID { get; set; }
        public string NAME { get; set; }
        public string IMAGE1 { get; set; }
        public decimal PRICE { get; set; }
        public int QUANTITY { get; set; }
    }
}