using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetsTest.Models
{
    public class Address
    {
        public int ShippingID { get; set; }
        public string STREETADDRESS { get; set; }
        public string PHONE { get; set; }
        public string CUSTOMERNAME { get; set; }
        public int CITY_ID { get; set; }
        public int USER_ID { get; set; }
    }
}