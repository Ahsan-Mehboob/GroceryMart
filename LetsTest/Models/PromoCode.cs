using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetsTest.Models
{
    public class PromoCode
    {
        public int ID { get; set; }
        public string COUPEN_CODE { get; set; }
        public decimal AMOUNT { get; set; }
        public DateTime START_DATE { get; set; }
        public DateTime END_DATE { get; set; }
        public string AMOUNT_TYPE { get; set; }
        public int STATUS { get; set; }
        public int STORE_ID { get; set; }
        public string STORE_NAME { get; set; }
        public string Status { get; set; }
    }
}