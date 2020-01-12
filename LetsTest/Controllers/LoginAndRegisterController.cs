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
    public class LoginAndRegisterController : Controller
    {
        string baseurl = IPClass.Ip.ip;
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Signup data)
        {
            data.USERTYPE = 1;
            data.CREATED_AT = DateTime.Now.Date;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);                

                var postTask = client.PostAsJsonAsync<Signup>("/api/UsersApi/Create", data);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    TempData.Add("AlertMessage", new AlertModel("Please Verify Your Email to Login into Your Account.", AlertModel.AlertType.Success));
                    return View();
                }
                else
                {
                    TempData.Add("AlertMessage", new AlertModel("Sorry! Your account has not been created, Please try again with correct information.", AlertModel.AlertType.Error));
                    return RedirectToAction("Register");
                }
            }
        }

        public ActionResult IsEmailExist_F(string Email)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                var responseTask = client.GetAsync("/api/UsersApi/IsEmailExist_F?EMAIL=" + Email);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<int>();
                    readTask.Wait();
                    var list = readTask.Result;
                    return Json(list, JsonRequestBehavior.AllowGet);
                }
                return Json(JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult IsUserExist_F(string username)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                var responseTask = client.GetAsync("/api/UsersApi/IsUserExist_F?username=" + username);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<int>();
                    readTask.Wait();
                    var list = readTask.Result;
                    return Json(list, JsonRequestBehavior.AllowGet);
    
                }
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult IsContactExist_F(string contact)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                var responseTask = client.GetAsync("/api/UsersApi/IsContactExist_F?contact=" + contact);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<int>();
                    readTask.Wait();
                    var list = readTask.Result;
                    return Json(list, JsonRequestBehavior.AllowGet);
                }
                return Json(JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Login data)
        {
            using (var client = new HttpClient())
            {
                UserLoginList userlogin = new UserLoginList();
                client.BaseAddress = new Uri(baseurl);

                try
                {
                    var postTask = client.PostAsJsonAsync<Login>("/api/LoginApi/Create", data);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<UserLoginList>();
                        readTask.Wait();

                        var userlog = readTask.Result;
                        Session["UserId"] = userlog.ID;
                        Session["UserName"] = userlog.NAME;
                        if(Session["cartItem"] != null)
                        {
                            return RedirectToAction("CartSummary", "Home");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        TempData.Add("AlertMessage", new AlertModel("It seems you misspell something. Or Please Signup First.", AlertModel.AlertType.Warning));
                        return RedirectToAction("Login");
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Exception occured"+ex);
                }
                return RedirectToAction("Login");
            }
        }
        [HttpGet]
        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPassword(string email)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                //HTTP POST
                var postTask = client.PostAsJsonAsync("/api/LoginApi/FindPassword?EMAIL="+email,email);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<int>();
                    readTask.Wait();
                    TempData.Add("AlertMessage", new AlertModel("An Email to reset your password has been sent to your Account.", AlertModel.AlertType.Success));
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "LoginAndRegister");
        }
    }
}