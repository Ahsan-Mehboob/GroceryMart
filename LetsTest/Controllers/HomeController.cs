using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Net.Http;
using LetsTest.Models;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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
                try
                {
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
                catch(Exception ex)
                {
                    Console.WriteLine("Error Occurs" + ex);
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
                if(product.DISCOUNT != null)
                {
                    decimal discount = Convert.ToDecimal(product.DISCOUNT);
                    var percentage = (discount / 100)*product.PRICE;
                    int Des = Convert.ToInt32(product.PRICE - percentage);
                    add.UnitPrice = Des;
                    add.Total = Des;
                }
                else
                {
                    add.Total = product.PRICE;
                    add.UnitPrice = product.PRICE;
                }
                
                add.StoreName = product.SNAME;
                addtocart.Add(add);
            }
            Session["CartCounter"] = addtocart.Count;
            Session["CartItem"] = addtocart;

            return Json(new {Success = true, Counter = addtocart.Count }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult newCart(CartSummaryToCheckout data)
        {
            Session["ords"] = data;
            return RedirectToAction("CheckOutdetails");
        }
        public ActionResult CheckOutdetails()
        {
            if (Session["UserId"] != null)
            {
                CartSummaryToCheckout order = Session["ords"] as CartSummaryToCheckout;
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
                        Session["AddressId"] = null;
                        return RedirectToAction("OrderStatus");
                    }
                }
                else
                {
                    TempData.Add("AlertMessage", new AlertModel("Please provide Your Shipping Address.", AlertModel.AlertType.Error));
                    return RedirectToAction("AddAddress");                    
                }

        }
        public async Task<ActionResult> OrderStatus()
        {
            Session["CartCounter"] = null;
            List<OrderStatus> ords = new List<OrderStatus>();
            var ordstat = Session["UserId"];
            using (var client = new HttpClient())
            {
                //passing service base url  
                client.BaseAddress = new Uri(baseurl);

                client.DefaultRequestHeaders.Clear();
                //define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage prod = await client.GetAsync("api/OrdersApi/AllOrderInformation?id=" + ordstat);

                //checking the response is successful or not which is sent using httpclient  
                if (prod.IsSuccessStatusCode)
                {
                    //storing the response details recieved from web api   
                    var prodlist = await prod.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ords = JsonConvert.DeserializeObject<List<OrderStatus>>(prodlist, settings);
                }
            }
            return View(ords);
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
                    return RedirectToAction("ListAddress");
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
                try
                {
                    var responseTask = client.GetAsync("api/OrdersApi/AllAddresses?id=" + id);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<List<AddressList>>();
                        readTask.Wait();

                        address = readTask.Result;
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Exception Occured" + ex);
                }
                return View(address);
            }
        }
        public ActionResult DeleteOrder(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                try
                {
                    var responseTask = client.GetAsync("api/OrdersApi/DeleteOrder?id=" + id);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("OrderStatus");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception Occured" + ex);
                }
                return RedirectToAction("OrderStatus");
            }
        }
        public ActionResult ReOrder(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                try
                {
                    var responseTask = client.GetAsync("api/OrdersApi/ReOrder?orderid=" + id);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("OrderStatus");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception Occured" + ex);
                }
                return RedirectToAction("OrderStatus");
            }
        }
        public ActionResult ViewOrderDetails(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                try
                {
                    var responseTask = client.GetAsync("api/OrdersApi/CheckOrderDetails?orderid=" + id);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<List<OrderDetails>>();
                        readTask.Wait();

                        var OrderDetail = readTask.Result;
                        return View(OrderDetail);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception Occured" + ex);
                }
                return RedirectToAction("OrderStatus");
            }
        }
        public ActionResult CartSummary()
        {
            return View();
        }

        public ActionResult VoucherCode(string id)
        {
            PromoCode proms = new PromoCode();
            PromoCode1 proms1 = new PromoCode1();
            proms1.promo = id;
            proms1.UID = Convert.ToInt32(Session["UserId"]);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);

                //HTTP POST
                var responseTask = client.PostAsJsonAsync<PromoCode1>("api/StoresApi/GetPromo/", proms1);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<PromoCode>();
                    readTask.Wait();
                    proms = readTask.Result;
                    return Json(proms, JsonRequestBehavior.AllowGet);
                }
                return Json(0,JsonRequestBehavior.AllowGet);
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

        public ActionResult Mail()
        {
            return View();
        }

        public ActionResult Services()
        {
            return View();
        }
    }
}