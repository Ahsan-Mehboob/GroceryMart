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
using static LetsTest.Models.IPClass;

namespace LetsTest.Controllers
{
    public class ApiDataController : Controller
    {
        //Hosted web API REST Service base url

        string baseurl = IPClass.Ip.ip;
        public async Task<ActionResult> Categorieslist()
        {
            var listcat = new category();
            var listsubcat = new SubCategory();

            using (var client = new HttpClient())
            {
                //passing service base url  
                client.BaseAddress = new Uri(baseurl);

                client.DefaultRequestHeaders.Clear();
                //define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage cat = await client.GetAsync("api/categoriesapi/listcategories");
                HttpResponseMessage subcat = await client.GetAsync("api/subcategoriesapi/listsubcategories");

                //checking the response is successful or not which is sent using httpclient  
                if (cat.IsSuccessStatusCode && subcat.IsSuccessStatusCode)
                {
                    //storing the response details recieved from web api   
                    var categorieslist = await cat.Content.ReadAsStringAsync();
                    var subcategorieslist = await subcat.Content.ReadAsStringAsync();

                    var list = new Alllists();
                    //deserializing the response recieved from web api and storing into the employee list  
                    listcat = JsonConvert.DeserializeObject<category>(categorieslist);
                    listsubcat = JsonConvert.DeserializeObject<SubCategory>(subcategorieslist);
                    List<AllCategories> listsub2 = new List<AllCategories>();

                    foreach (var item in listcat.categories)
                    {
                        List<subCatogary> listsub = new List<subCatogary>();
                       
                        var values = new AllCategories();
                        values.categoryName = item.NAME;
                        foreach (var item2 in listsubcat.s)
                        {
                            var valueObject = new subCatogary();
                            if (item2.CATEGORY_ID == item.ID)
                            {
                                valueObject.SubCatogaryID = item2.ID;
                                valueObject.SubCatogaryName = item2.NAME;
                                listsub.Add(valueObject);
                            }
                            values.Subcategory = listsub;
                        }
                        listsub2.Add(values);

                        list.objectList = listsub2;
                    }
                    ViewBag.allCategories = list.objectList;
                }
                return PartialView();
            }
        }
    }
}