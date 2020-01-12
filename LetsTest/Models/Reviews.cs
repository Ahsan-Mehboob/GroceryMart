using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetsTest.Models
{
    public class Reviews
    {
        public int ID { get; set; }
        public int PRODUCT_ID { get; set; }
        public int UID { get; set; }
        public string DES { get; set; }
        public int RATINGS { get; set; }
        public DateTime CREATED_AT { get; set; }
        public DateTime UPDATED_AT { get; set; }
        public string NAME { get; set; }
    }
}