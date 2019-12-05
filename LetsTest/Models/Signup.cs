using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetsTest.Models
{
    public class Signup
    {
        public string NAME { get; set; }
        public string PASSWORD { get; set; }
        public string EMAIL { get; set; }
        public string CONTACT { get; set; }
        public int USERTYPE { get; set; }
        public DateTime CREATED_AT { get; set; }
    }
}