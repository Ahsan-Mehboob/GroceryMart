using LetsTest.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using PagedList;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Linq;

namespace LetsTest.Controllers
{
    public class SingleController : Controller
    {
        // GET: Single
        string baseurl = IPClass.Ip.ip;
        public async Task<ActionResult> SingleProduct(int id)
        {
            SingleProductDetails singleProductDetails = new SingleProductDetails();
            List<Reviews> reviews = new List<Reviews>();
            TakeReview take = new TakeReview();
            take.PRODUCT_ID = id;
            take.UID = Convert.ToInt32(Session["UserId"]);

            using (var client = new HttpClient())
            {
                //passing service base url  
                client.BaseAddress = new Uri(baseurl);

                client.DefaultRequestHeaders.Clear();
                //define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage prod = await client.GetAsync("api/ProductsApi/GetProduct/?id=" + id);
                HttpResponseMessage review = await client.GetAsync("api/ProductsApi/GetReviews/?id=" + id);
                var takeReview = client.PostAsJsonAsync<TakeReview>("api/OrdersApi/CheckOrderStatusReview", take);
                takeReview.Wait();

                var result = takeReview.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<int>();
                    readTask.Wait();
                    var reviewid = readTask.Result;
                    ViewBag.Review = reviewid;
                }

                if (review.IsSuccessStatusCode)
                {
                    //storing the response details recieved from web api   
                    var reviews1 = await review.Content.ReadAsStringAsync();
                    var settings1 = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    reviews = JsonConvert.DeserializeObject<List<Reviews>>(reviews1, settings1);
                }
                ViewBag.Reviews = reviews;

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
                    singleProductDetails = JsonConvert.DeserializeObject<SingleProductDetails>(prodlist,settings);

                }
                return View(singleProductDetails);
            }
        }

        public async Task<ActionResult> RealtedProds(int id)
        {
            List<ProductsList> prods = new List<ProductsList>();

            using (var client = new HttpClient())
            {
                //passing service base url  
                client.BaseAddress = new Uri(baseurl);

                client.DefaultRequestHeaders.Clear();
                //define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage prod = await client.GetAsync("api/ProductsApi/RelatedProducts/?id=" + id);

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
                    prods = JsonConvert.DeserializeObject<List<ProductsList>>(prodlist,settings);

                }
                return PartialView(prods);
            }
        }

        public async Task<ActionResult> PeopleBuy()
        {
            List<ProductsList> prods = new List<ProductsList>();
            var SubcatId = Session["SubcategoryID"];
            using (var client = new HttpClient())
            {
                //passing service base url  
                client.BaseAddress = new Uri(baseurl);

                client.DefaultRequestHeaders.Clear();
                //define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage prod = await client.GetAsync("api/OrdersApi/TopProductsbyCategories?id=" + SubcatId);

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
                    prods = JsonConvert.DeserializeObject<List<ProductsList>>(prodlist,settings);

                }
                return PartialView(prods);
            }
        }

        public async Task<ActionResult> StoreInfo(int id, int? page, string sort, string searchBy, string Search)
        {
            List<ProductsList> productsLists = new List<ProductsList>();
            ViewBag.SortPriceParameter = string.IsNullOrEmpty(sort) ? "Price high to low" : "";

            using (var client = new HttpClient())
            {
                //passing service base url  
                client.BaseAddress = new Uri(baseurl);

                client.DefaultRequestHeaders.Clear();
                //define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage prod = await client.GetAsync("api/StoresApi/ShowStore?id=" + id + "&limit=" + 12);

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
                    productsLists = JsonConvert.DeserializeObject<List<ProductsList>>(prodlist,settings);

                }
                var prods = productsLists.AsQueryable();

                if (searchBy == "ProductName")
                {
                    prods = prods.Where(x => x.NAME.StartsWith(Search) || Search == null);
                }
                else if (searchBy == "StoreName")
                {
                    prods = prods.Where(x => x.STORE.StartsWith(Search) || Search == null);
                }

                switch (sort)
                {

                    case "Price high to low":
                        prods = prods.OrderByDescending(x => x.PRICE);
                        break;

                    case "Price low to high":
                        prods = prods.OrderBy(x => x.PRICE);
                        break;

                    default:
                        prods = prods.OrderBy(x => x.PRICE);
                        break;

                }
                return View(prods.ToPagedList(page ?? 1, 20));
            }
        }

        public ActionResult Reviews(TakeReview review)
        {
            review.UID = Convert.ToInt32(Session["UserId"]);
            int id = review.PRODUCT_ID;
            review.CREATED_AT = DateTime.Now;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var takeReview = client.PostAsJsonAsync<TakeReview>("api/OrdersApi/AddReview", review);
                takeReview.Wait();
                var result = takeReview.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("SingleProduct","Single",new { id= review.PRODUCT_ID});
                }
                else
                {
                    return RedirectToAction("SingleProduct", "Single", new { id = review.PRODUCT_ID });
                }
            }
        }

        //public async Task<ActionResult> Chat()
        //{
        //    List<Search> search = new List<Search>();
        //    using (var client = new HttpClient())
        //    {
        //        //passing service base url  
        //        client.BaseAddress = new Uri(baseurl);

        //        client.DefaultRequestHeaders.Clear();

        //        //define request data format  
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        HttpResponseMessage prod = await client.PostAsJsonAsync("api/ProductsApi/SearchProductbyDistance");

        //        //checking the response is successful or not which is sent using httpclient  
        //        if (prod.IsSuccessStatusCode)
        //        {
        //            //storing the response details recieved from web api   
        //            var prodlist = await prod.Content.ReadAsStringAsync();
        //            search = JsonConvert.DeserializeObject<List<Search>>(prodlist);

        //        }
        //        return View();
        //    }
        //}

        //public ActionResult ChatInterface()
        //{
        //    if(Session["UserName"] == null)
        //    {
        //        return RedirectToAction("Login", "LoginAndRegister");
        //    }
        //    else
        //    {
        //        return PartialView();
        //    }
        //}
        public async Task<ActionResult> sendmsg(Chat demo)
        {
            using (var client = new HttpClient())
            {
                //passing service base url  
                client.BaseAddress = new Uri(baseurl);

                client.DefaultRequestHeaders.Clear();

                //define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage prod = await client.PostAsJsonAsync("api/ChatApi/sendmsg", demo);

                //checking the response is successful or not which is sent using httpclient  
                if (prod.IsSuccessStatusCode)
                {
                    //storing the response details recieved from web api   
                    var prodlist = await prod.Content.ReadAsStringAsync();
                    return Json(null);
                }
                return Json(JsonRequestBehavior.AllowGet);
            }
        }
        //public async Task<ActionResult> receive()
        //{
        //    string userqueue = Session["UserName"].ToString();
        //    using (var client = new HttpClient())
        //    {
        //        //passing service base url  
        //        client.BaseAddress = new Uri(baseurl);

        //        client.DefaultRequestHeaders.Clear();
        //        //define request data format  
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        HttpResponseMessage prod = await client.GetAsync("api/ChatApi/receive?userqueue=" + userqueue);

        //        //checking the response is successful or not which is sent using httpclient  
        //        if (prod.IsSuccessStatusCode)
        //        {
        //            //storing the response details recieved from web api   
        //            var prodlist = await prod.Content.ReadAsStringAsync();
        //            return Json(prodlist);
        //        }
        //        return Json(null);
        //    }
        //}
        public ActionResult receive(string userqueue)
        {

            //string userqueue = Session["UserName"].ToString();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                var responseTask = client.GetAsync("api/ChatApi/receive?userqueue=" + userqueue);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<string>();
                    readTask.Wait();
                    var message = readTask.Result;
                    return Json(message);
                }
                return Json(null);
            }
        }
        public ActionResult receive1(string userqueue)
        {

            //string userqueue = Session["UserName"].ToString();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                var responseTask = client.GetAsync("api/ChatApi/receive?userqueue="+userqueue);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<string>();
                    readTask.Wait();
                    var message = readTask.Result;
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
    }
}