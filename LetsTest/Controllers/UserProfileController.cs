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

                //checking the response is successful or not which is sent using httpclient  
                if (prod.IsSuccessStatusCode)
                {
                    //storing the response details recieved from web api   
                    var prodlist = await prod.Content.ReadAsStringAsync();
                    up = JsonConvert.DeserializeObject<UserProfile>(prodlist);
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
    }
}