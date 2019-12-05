using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using LetsTest.Models;
using Newtonsoft.Json;

namespace LetsTest.Controllers
{
    public class HomeController : Controller
    {
        string baseurl = IPClass.Ip.ip;
        List<ShoppingCartItems> shop = new List<ShoppingCartItems>();
        List<AddtoCart> addtocart = new List<AddtoCart>();
        public ActionResult Index()
        {
            List<ProductsList> productsLists = new List<ProductsList>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                //HTTP GET
                var responseTask = client.GetAsync("/api/OrdersApi/ProductsBuyUsers");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<ProductsList>>();
                    readTask.Wait();

                    productsLists = readTask.Result;
                }
            }
            return View(productsLists);

        }
        [HttpPost]
        public JsonResult Index(int ItemId)
        {
            SingleProductDetails productDetail = new SingleProductDetails();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                //HTTP GET
                var responseTask = client.GetAsync("api/ProductsApi/GetProduct/?id=" + ItemId);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<SingleProductDetails>();
                    readTask.Wait();

                    productDetail = readTask.Result;
                }
            }
            var product = productDetail;
            AddtoCart add = new AddtoCart();
            if(Session["CartCounter"] != null)
            {
                addtocart = Session["CartItem"] as List<AddtoCart>;
            }
            if(addtocart.Any(model => model.ItemId == ItemId))
            {
                add = addtocart.Single(model => model.ItemId == ItemId);
                add.Quantity = add.Quantity + 1;
                add.Total = add.Quantity * add.UnitPrice;
            }
            else
            {
                add.ItemId = ItemId;
                add.ImgPath = product.IMAGE1;
                add.ItemName = product.NAME;
                add.Quantity = 1;
                add.Total = product.PRICE;
                add.UnitPrice = product.PRICE;
                add.StoreName = product.SNAME;
                addtocart.Add(add);
            }
            Session["CartCounter"] = addtocart.Count;
            Session["CartItem"] = addtocart;

            return Json(new {Success = true, Counter = addtocart.Count }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckOutdetails()
        {
            if (Session["UserId"] != null)
            {
                List<AddtoCart> order = Session["CartItem"] as List<AddtoCart>;
                return View(order);
            }
            else
            {
                return RedirectToAction("Login", "LoginAndRegister");
            }
        }

        [HttpPost]
        public ActionResult AddToOrder(Order order)
        {
            order.ShippingID = Convert.ToInt32(Session["AddressId"]);
            order.CreatedAt = DateTime.Now;
            order.UpdatedAt = DateTime.Now;
            order.OrderStatus = 0;
            order.TotalShippingCharges = 200;
            order.UID = Convert.ToInt32(Session["UserId"]);
            order.AmountAfterDiscount = 100;
            order.city_id = 6;
            if(order.ShippingID != 0)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);

                    //HTTP POST
                    var postTask = client.PostAsJsonAsync<Order>("/api/OrdersApi/Create", order);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<int>();
                        readTask.Wait();
                        var userlog = readTask.Result;
                        Session["OrderID"] = userlog;
                    }
                    Session["CartCounter"] = null;
                    return RedirectToAction("OrderStatus");
                }
            }
            else
            {
                TempData.Add("AlertMessage", new AlertModel("Please provide Your Shipping Address.", AlertModel.AlertType.Error));
                return RedirectToAction("CheckOutdetails");
            }

        }
        public ActionResult OrderStatus()
        {
            Session["CartCounter"] = null;
            List<OrderStatus> ords = new List<OrderStatus>();
            var ordstat = Session["UserId"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);

                //HTTP POST
                var responseTask = client.GetAsync("api/OrdersApi/AllOrderInformation?id=" + ordstat);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<OrderStatus>>();
                    readTask.Wait();

                    ords = readTask.Result;
                }
                return View(ords);
            }
        }

        public ActionResult AddAddress()
        {
           return View();
        }
        [HttpPost]
        public ActionResult AddAddress(Address data)
        {
            data.CITY_ID = 6;
            var id = Session["UserId"];
            data.USER_ID = Convert.ToInt32(id);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);

                //HTTP POST
                var responseTask = client.PostAsJsonAsync<Address>("api/OrdersApi/AddAddress", data);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<int>();
                    readTask.Wait();

                    var ide = readTask.Result;
                    Session["AddressId"] = ide;
                    TempData.Add("AlertMessage", new AlertModel("Your Address has been added successfully.", AlertModel.AlertType.Success));
                    return RedirectToAction("CheckOutdetails");
                }
                return View();
            }
        }
        public ActionResult ShippingId(int id)
        {
            Session["AddressId"] = id;
            return RedirectToAction("CheckOutdetails");
        }

        public ActionResult ListAddress()
        {
            var id = Session["UserId"];
            List<AddressList> address = new List<AddressList>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);

                //HTTP POST
                var responseTask = client.GetAsync("api/OrdersApi/AllAddresses?id=" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<AddressList>>();
                    readTask.Wait();

                    address = readTask.Result;
                }
                return View(address);
            }
        }
        public ActionResult CartSummary()
        {
            return View();
        }

        public ActionResult VoucherCode(string promo)
        {
            PromoCode proms = new PromoCode();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);

                //HTTP POST
                var responseTask = client.GetAsync("api/PromoApi/GetPromo?promo=" + promo);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<PromoCode>();
                    readTask.Wait();

                    proms = readTask.Result;
                }
                return View();
            }
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Events()
        {
            return View();
        }

        public ActionResult FaQs()
        {
            return View();
        }

        public ActionResult Mail()
        {
            return View();
        }

        public ActionResult Payment()
        {
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }

        public ActionResult Services()
        {
            return View();
        }
    }
}