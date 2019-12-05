using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetsTest.Models
{
    public class OrderStatus
    {
        public int ID { get; set; }
        public DateTime CREATED_AT { get; set; }
        public int SHIPPING_ID { get; set; }
        public string STREETADDRESS { get; set; }
        public string PHONE { get; set; }
        public string CUSTOMERNAME { get; set; }
        public string CITY { get; set; }
        public double AMOUNT { get; set; }
        public string ORDERSTATUS { get; set; }
        public int UID { get; set; }
        public double AMT_AFTER_DISC { get; set; }
        public int CITY_ID { get; set; }
        public int STORE_ID { get; set; }
        public string NAME { get; set; }

    }
}