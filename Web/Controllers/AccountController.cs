namespace Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Utils;

    [Route("account")]
    public class AccountController : Controller
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();


        [Route("")]
        [Route("index")]
        [Route("~/")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (username != null && password != null)
            {
                var values = new Dictionary<string, string>
                {
                    {"username", username},
                    {"password", password}
                };

                var response = await client.PostAsync(new Uri("https://localhost:5001/api/Account/Authenticate"),
                    new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json"));

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    HttpContext.Session.SetString("username", username);
                    HttpContext.Session.SetString("password", Md5Helper.EncryptString(password));
                    return View("Success");
                }
            }

            ViewBag.error = "Invalid Account";
            return View("Index");
        }

        [Route("logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("username");
            HttpContext.Session.Remove("password");

            return RedirectToAction("Index");
        }
    }
}