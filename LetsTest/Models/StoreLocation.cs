using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetsTest.Models
{
    public class StoreLocation
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public string DES { get; set; }
        public string CELLNO { get; set; }
        public string LONGITUDE { get; set; }
        public string LATITUDE { get; set; }
        public string STREETADDRESS { get; set; }
        public string CITY { get; set; }
        public string PROVINCE { get; set; }
        public string COUNTRY { get; set; }
        public int CITY_ID { get; set; }
        public int PID { get; set; }
        public int CID { get; set; }
        public int TYPE_ID { get; set; }
        public string TYPE_NAME { get; set; }
        public int UID { get; set; }
    }
}