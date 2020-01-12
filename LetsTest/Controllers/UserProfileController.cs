using LetsTest.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LetsTest.Controllers
{
    public class UserProfileController : Controller
    {
        string baseurl = IPClass.Ip.ip;
        // GET: UserProfile
        public async Task<ActionResult> Profiles()
        {
            var id = Session["UserId"];
            UserProfile up = new UserProfile();
            using (var client = new HttpClient())
            {
                //passing service base url  
                client.BaseAddress = new Uri(baseurl);

                client.DefaultRequestHeaders.Clear();
                //define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage prod = await client.GetAsync("/api/UsersApi/FindUser?id=" + id);
                HttpResponseMessage totalord = await client.GetAsync("api/OrdersApi/TotalOrders?id=" + id);
                HttpResponseMessage orderdel = await client.GetAsync("api/OrdersApi/OrderDelivered?id=" + id);
                HttpResponseMessage orderplace = await client.GetAsync("api/OrdersApi/OrderPlaced?id=" + id);
                HttpResponseMessage orderinpro = await client.GetAsync("api/OrdersApi/OrderInprocess?id=" + id);

                //checking the response is successful or not which is sent using httpclient  
                if (prod.IsSuccessStatusCode && totalord.IsSuccessStatusCode && orderdel.IsSuccessStatusCode && orderplace.IsSuccessStatusCode && orderinpro.IsSuccessStatusCode)
                {
                    //storing the response details recieved from web api   
                    var prodlist = await prod.Content.ReadAsStringAsync();
                    up = JsonConvert.DeserializeObject<UserProfile>(prodlist);
                    var total = await totalord.Content.ReadAsStringAsync();
                    ViewBag.total = total;
                    var orderdeliver = await orderdel.Content.ReadAsStringAsync();
                    ViewBag.orderdeliver = orderdeliver;
                    var orderplaces = await orderplace.Content.ReadAsStringAsync();
                    ViewBag.orderplaces = orderplaces;
                    var orderinprocess = await orderinpro.Content.ReadAsStringAsync();
                    ViewBag.orderinprocess = orderinprocess;
                }
                return View(up);
            }
        }
        [HttpGet]
        public async Task<ActionResult> UserProfileEdit()
        {
            var id = Session["UserId"];
            UserLoginList up = new UserLoginList();
            using (var client = new HttpClient())
            {
                //passing service base url  
                client.BaseAddress = new Uri(baseurl);

                client.DefaultRequestHeaders.Clear();
                //define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage prod = await client.GetAsync("/api/UsersApi/FindUser?id=" + id);

                //checking the response is successful or not which is sent using httpclient  
                if (prod.IsSuccessStatusCode)
                {
                    //storing the response details recieved from web api   
                    var prodlist = await prod.Content.ReadAsStringAsync();
                    up = JsonConvert.DeserializeObject<UserLoginList>(prodlist);
                }
            }
            return PartialView("~/Views/UserProfile/UserProfileEdit.cshtml",up);
        }
        public async Task<ActionResult> OrderStatus()
        {
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
                    ords = JsonConvert.DeserializeObject<List<OrderStatus>>(prodlist,settings);
                }
                var order = ords.AsQueryable();
                order = order.Take(3);
                return PartialView("~/Views/UserProfile/OrderStatus.cshtml", order);
            }
        }
        [HttpPost]
        public ActionResult UserProfileEdit(UserLoginList user)
        {
            user.USERTYPE = 1;
            UserLoginList users = new UserLoginList();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);

                //HTTP POST
                var postTask = client.PostAsJsonAsync<UserLoginList>("/api/UsersApi/UpdateUser", user);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Profiles");
                }
            }
            return RedirectToAction("Profiles");
        }

        public async Task<ActionResult> OrderDetails()
        {
            var id = Session["UserId"];
            UserLoginList users = new UserLoginList();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                
                HttpResponseMessage cat = await client.GetAsync("api/OrdersApi/TotalOrders?id="+id);
                HttpResponseMessage subcat = await client.GetAsync("api/OrdersApi/OrderDelivered?id=" + id);
                HttpResponseMessage cat1 = await client.GetAsync("api/OrdersApi/OrderPlaced?id=" + id);
                HttpResponseMessage subcat1 = await client.GetAsync("api/OrdersApi/OrderInprocess?id=" + id);
                if (cat.IsSuccessStatusCode)
                {
                    var total = await cat.Content.ReadAsStringAsync();
                    ViewBag.total = total;

                }
                if (subcat.IsSuccessStatusCode)
                {
                    var delivered = await cat.Content.ReadAsStringAsync();
                    ViewBag.delivered = delivered;
                }
                if (subcat1.IsSuccessStatusCode)
                {
                    var inprocess = await cat.Content.ReadAsStringAsync();
                    ViewBag.inprocess = inprocess;
                }
                if (cat1.IsSuccessStatusCode)
                {
                    var placed = await cat.Content.ReadAsStringAsync();
                    ViewBag.placed = placed;
                }
            }
            return RedirectToAction("Profiles");
        }
    }
}