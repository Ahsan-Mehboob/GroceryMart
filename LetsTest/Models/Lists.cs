using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetsTest.Models
{
    public class AllCategories
    {
        public string categoryName { get; set; }
        public List<subCatogary> Subcategory { get; set; }
    }
    public class Alllists
    {
        public List<AllCategories> objectList { get; set; }
    }

    public class subCatogary
    {
        public string SubCatogaryName { get; set; }
        public int SubCatogaryID { get; set; }
    }
}