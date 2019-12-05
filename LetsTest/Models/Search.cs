using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetsTest.Models
{
    public class Search
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public string DES { get; set; }
        public decimal PRICE { get; set; }
        public string CREATED_AT { get; set; }
        public string UPDATED_AT { get; set; }
        public int SUBCATEGORY_ID { get; set; }
        public int UOM_ID { get; set; }
        public string IMAGE1 { get; set; }
        public string IMAGE2 { get; set; }
        public string IMAGE3 { get; set; }
        public int STORE_ID { get; set; }
    }
}