using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetsTest.Models
{
    public class Chat
    {
        public int STORE_ID { get; set; }
        public int USER_ID { get; set; }
        public string MESSAGE { get; set; }
        public string friend { get; set; }
    }
}