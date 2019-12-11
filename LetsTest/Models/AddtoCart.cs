using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetsTest.Models
{
    public class AddtoCart
    {
        public  int ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string DISCOUNT { get; set; }
        public decimal Total { get; set; }
        public string ItemName { get; set; }
        public decimal lineTotal { get; set; }
        public string ImgPath { get; set; }
        //public int quantityT { get; set; }
        public string StoreName { get; set; }
        public decimal DESC_Price { get; set; }
        //public int TotalQ { get; set; }

    }
}