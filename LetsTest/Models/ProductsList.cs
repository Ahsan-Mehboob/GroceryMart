using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetsTest.Models
{
    public class ProductsList
    {
        public int ID { get; set; }
        public int PRODUCT_ID { get; set; }
        public string NAME { get; set; }
        public string DES { get; set; }
        public decimal PRICE { get; set; }
        public DateTime CREATED_AT { get; set; }
        public string UPDATED_AT { get; set; }
        public int SUBCATEGORY_ID { get; set; }
        public string SUBCATEGORY_NAME { get; set; }
        public string SUBCATEGORY_DES { get; set; }
        public int CATEGORY_ID { get; set; }
        public string CATEGORY { get; set; }
        public string CATEGORY_DES { get; set; }
        public int UOM_ID { get; set; }
        public string UOM_NAME { get; set; }
        public string IMAGE1 { get; set; }
        public int STORE_ID { get; set; }
        public string STORE { get; set; }
        public string DISCOUNT { get; set; }
        public decimal percentage { get; set; }
        public decimal DESC { get; set; }
        public decimal DESC_Price { get; set; }
        public string TITLE { get; set; }
        public DateTime START_DATE { get; set; }
        public DateTime END_DATE { get; set; }
        public string SUBCATEGORY_DISCOUNT { get; set; }
        public decimal SUB_DISCOUNT { get; set; }
        public DateTime S_START_DATE { get; set; }
        public DateTime S_END_DATE { get; set; }
        public decimal CATEGORY_DISCOUNT { get; set; }
        public DateTime C_START_DATE { get; set; }
        public DateTime C_END_DATE { get; set; }
    }
}