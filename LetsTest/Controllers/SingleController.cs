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

            using (var client = new HttpClient())
            {
                //passing service base url  
                client.BaseAddress = new Uri(baseurl);

                client.DefaultRequestHeaders.Clear();
                //define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage prod = await client.GetAsync("api/ProductsApi/GetProduct/?id=" + id);

                //checking the response is successful or not which is sent using httpclient  
                if (prod.IsSuccessStatusCode)
                {
                    //storing the response details recieved from web api   
                    var prodlist = await prod.Content.ReadAsStringAsync();
                    singleProductDetails = JsonConvert.DeserializeObject<SingleProductDetails>(prodlist);

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
                    prods = JsonConvert.DeserializeObject<List<ProductsList>>(prodlist);

                }
                return PartialView(prods);
            }
        }

        public async Task<ActionResult> PeopleBuy(int id)
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
                    prods = JsonConvert.DeserializeObject<List<ProductsList>>(prodlist);

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
                    productsLists = JsonConvert.DeserializeObject<List<ProductsList>>(prodlist);

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
    }
}