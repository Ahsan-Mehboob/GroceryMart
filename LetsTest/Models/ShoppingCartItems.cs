using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetsTest.Models
{
    public class ShoppingCartItems
    {
        //public SingleProductDetails prods { get; set; }
        //public int quantity { get; set; }
        public List<int> ItemId { get; set; }
        public List<int> Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int Total { get; set; }
        public string ItemName { get; set; }
        public decimal lineTotal { get;set; }
        public string ImgPath { get; set; }
        public int STORE_ID { get; set; }
    }
}