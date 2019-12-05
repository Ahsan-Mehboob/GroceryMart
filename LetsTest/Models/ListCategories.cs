using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetsTest.Models
{
    public class category
    {
        public List<ListCategories> categories { get; set; }
    }
    public class ListCategories
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public string DES { get; set; }
    }
}