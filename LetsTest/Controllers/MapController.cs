using LetsTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace LetsTest.Controllers
{
    public class MapController : Controller
    {
        string baseurl = IPClass.Ip.ip;
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetAllLocations()
        {
            List<StoreLocation> stores = new List<StoreLocation>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    //HTTP GET
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
                return Json(stores, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}