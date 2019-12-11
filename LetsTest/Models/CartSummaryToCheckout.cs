using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetsTest.Models
{
    public class CartSummaryToCheckout
    {
        public List<int> Product_ID { get; set; }
        public List<int> Quantity { get; set; }
        public List<decimal> price { get; set; }
        public decimal Amount { get; set; }
        public List<string> ItemName { get; set; }
        public List<string> StoreName { get; set; }
        public List<int> TotalQuantity { get; set; }
    }
}