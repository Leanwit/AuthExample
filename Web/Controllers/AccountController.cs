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
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Utils;

    [Route("account")]
    public class AccountController : Controller
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        private readonly WebApiSettings _webApiSettings;

        public AccountController(IOptions<WebApiSettings> webApiSettings)
        {
            _webApiSettings = webApiSettings.Value;
        }

        [Route("")]
        [Route("index")]
        [Route("~/")]
        public IActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string returnUrl)
        {
            if (username != null && password != null)
            {
                var values = new Dictionary<string, string>
                {
                    {"username", username},
                    {"password", password}
                };

                var response = await client.PostAsync(new Uri(_webApiSettings.AuthenticateUrl),
                    new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json"));

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    HttpContext.Session.SetString("username", username);
                    HttpContext.Session.SetString("password", Md5Helper.EncryptString(password));
                    ViewBag.Logged = true;

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        var path = returnUrl.Split("/");
                        return RedirectToAction(path[2], path[1]);
                    }

                    return View("Success");
                }
            }

            ViewBag.error = "Invalid Credentials";
            return View("index");
        }

        [Route("logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("username");
            HttpContext.Session.Remove("password");
            ViewBag.Logged = false;
            return RedirectToAction("Index");
        }

        [Route("EndUserSession")]
        [HttpGet]
        public void EndUserSession()
        {
            HttpContext.Session.Remove("username");
            HttpContext.Session.Remove("password");
            ViewBag.Logged = false;
        }

        [Route("AccessDenied")]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }
    }
}