using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetsTest.Models
{
    public class Order
    {
        public int ID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public decimal TotalShippingCharges { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountAfterDiscount { get; set; }
        public int OrderStatus { get; set; }
        public int UID { get; set; }
        public int TotalQuantity { get; set; }
        public List<int> Product_ID { get; set; }
        public List<int> Quantity { get; set; }
        public string CUSTOMER { get; set; }
        public string CONTACT { get; set; }
        public int city_id { get; set; }
        public string STREETADDRESS { get; set; }
        public int ShippingID { get; set; }
        public int STORE_ID { get; set; }
    }
}