using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetsTest.Models
{
    public class SubCategory
    {
        public List<SubCategories> s { get; set; }
    }
    public class SubCategories
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public string DES { get; set; }
        public int CATEGORY_ID { get; set; }
        public string CATEGORY { get; set; }
        public string CATEGORY_DES { get; set; }
    }

   
}