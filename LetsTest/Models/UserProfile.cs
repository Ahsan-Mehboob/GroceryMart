using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetsTest.Models
{
    public class UserProfile
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public string EMAIL { get; set; }
        public string PASSWORD { get; set; }
        public string CONTACT { get; set; }
        public string USERTYPE { get; set; }
        public int UID { get; set; }
        public DateTime CREATED_AT { get; set; }
    }
}