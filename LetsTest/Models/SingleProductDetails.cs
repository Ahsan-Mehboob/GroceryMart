using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetsTest.Models
{
    public class SingleProductDetails
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public string DES { get; set; }
        public string IMAGE1 { get; set; }
        public string IMAGE2 { get; set; }
        public string IMAGE3 { get; set; }
        public decimal PRICE { get; set; }
        public int STORE_ID { get; set; }
        public string SNAME { get; set; }
        public int CATEGORY_ID { get; set; }
    }
}