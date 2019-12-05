using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetsTest.Models
{
    public class AddressList
    {
        public int ID { get; set; }
        public string STREETADDRESS { get; set; }
        public string PHONE { get; set; }
        public string CUSTOMERNAME { get; set; }
        public int CITY_ID { get; set; }
        public string CITY { get; set; }
        public string NAME { get; set; }
        public string CONTACT { get; set; }
        public string EMAIL { get; set; }
    }
}