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
    public class ProductsController : Controller
    {
        string baseurl = IPClass.Ip.ip;
        public async Task<ActionResult> Productslist(int id, string sort, int? page, string storename, int? MinPrice, int? MaxPrice)
        {
            Session["SubcategoryID"] = id;
            List<ProductsList> productsLists = new List<ProductsList>();
            ViewBag.SortPriceParameter = string.IsNullOrEmpty(sort) ? "Price high to low" : "";

            using (var client = new HttpClient())
            {
                //passing service base url  
                client.BaseAddress = new Uri(baseurl);

                client.DefaultRequestHeaders.Clear();
                //define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage prod = await client.GetAsync("api/ProductsApi/ShowProduct/?id=" + id);
               
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
                    //Session["SubcategoryID"] = productsLists.Select(sub => sub.SUBCATEGORY_ID);
                }
                var prods = productsLists.AsQueryable();
                var stores = prods.Select(x => x.STORE).Distinct().ToList();
                ViewBag.store = stores;
                if (storename != null)
                {
                    prods = prods.Where(x => x.STORE == storename);
                }

                if(MinPrice != null && MaxPrice != null)
                {
                    prods = prods.Where(x => x.PRICE >= MinPrice && x.PRICE <= MaxPrice);
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
                return View(prods.ToPagedList(page ?? 1,8));
            }
        }
        public JsonResult storesProducts(string storename)
        {
            List<StoreLocation> stores = new List<StoreLocation>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                //HTTP GET
                try
                {
                    var responseTask = client.GetAsync("api/StoresApi/ListStore");
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<List<StoreLocation>>();
                        readTask.Wait();

                        stores = readTask.Result;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Occurs" + ex);
                }
            }
            return Json(stores, JsonRequestBehavior.AllowGet);
        }

        //public async Task<ActionResult> Search(string id, int[] ids)
        public async Task<ActionResult> Search(string id,string sort,string storename, int? MinPrice, int? MaxPrice)
        {
            List<Search> search = new List<Search>();
            ViewBag.SortPriceParameter = string.IsNullOrEmpty(sort) ? "Price high to low" : "";
            Session["search"] = id;
            using (var client = new HttpClient())
            { 
                //passing service base url  
                client.BaseAddress = new Uri(baseurl);

                client.DefaultRequestHeaders.Clear();

                //define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

               if(TempData["filterdata"] == null)
                {
                    HttpResponseMessage prod = await client.GetAsync("api/ProductsApi/SearchProduct/?name=" + id);

                    //checking the response is successful or not which is sent using httpclient  
                    if (prod.IsSuccessStatusCode)
                    {
                        //storing the response details recieved from web api   
                        var prodlist = await prod.Content.ReadAsStringAsync();
                        search = JsonConvert.DeserializeObject<List<Search>>(prodlist);

                    }
                }
                else
                {
                    search = TempData["filterdata"] as List<Search>;
                }
                var sear = search.AsQueryable();
                var stores = sear.Select(x => x.STORE).Distinct().ToList();
                ViewBag.store = stores;
                if (storename != null)
                {
                    sear = sear.Where(x => x.STORE == storename);
                }
                if (MinPrice != null && MaxPrice != null)
                {
                    sear = sear.Where(x => x.PRICE >= MinPrice && x.PRICE <= MaxPrice);
                }
                switch (sort)
                {
                    case "Price high to low":
                        sear = sear.OrderByDescending(x => x.PRICE);
                        break;

                    case "Price low to high":
                        sear = sear.OrderBy(x => x.PRICE);
                        break;

                    default:
                        sear = sear.OrderBy(x => x.PRICE);
                        break;

                }
                return View(sear);
            }
        }
        public ActionResult NearByFilter()
        {
            List<Search> data = TempData["filterdata"] as List<Search>;
            return View(data);
        }

        public async Task<ActionResult> SearchByDistance(SearchDemo demo)
        {
            List<Search> search = new List<Search>();
            demo.name = Session["search"].ToString();
            using (var client = new HttpClient())
            {
                //passing service base url  
                client.BaseAddress = new Uri(baseurl);

                client.DefaultRequestHeaders.Clear();

                //define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage prod = await client.PostAsJsonAsync("api/ProductsApi/SearchProductbyDistance" , demo);

                //checking the response is successful or not which is sent using httpclient  
                if (prod.IsSuccessStatusCode)
                {
                    //storing the response details recieved from web api   
                    var prodlist = await prod.Content.ReadAsStringAsync();
                    search = JsonConvert.DeserializeObject<List<Search>>(prodlist);
                    TempData["filterdata"] = search;
                    return RedirectToAction("Search");
                }
                return null;
            }
        }
    }
}