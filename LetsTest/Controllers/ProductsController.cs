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
        public async Task<ActionResult> Productslist(int id, int? page, string sort, string searchBy, string Search)
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
                return View(prods.ToPagedList(page ?? 1,8));
            }
        }

        public async Task<ActionResult> Search(string id)
        {
            List<Search> search = new List<Search>();

            using (var client = new HttpClient())
            { 
                //passing service base url  
                client.BaseAddress = new Uri(baseurl);

                client.DefaultRequestHeaders.Clear();
                //define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage prod = await client.GetAsync("api/ProductsApi/SearchProduct/?name=" + id);

                //checking the response is successful or not which is sent using httpclient  
                if (prod.IsSuccessStatusCode)
                {
                    //storing the response details recieved from web api   
                    var prodlist = await prod.Content.ReadAsStringAsync();
                    search = JsonConvert.DeserializeObject<List<Search>>(prodlist);

                }
                return View(search);
            }
        }
    }
}