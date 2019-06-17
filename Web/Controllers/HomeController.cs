namespace Web.Controllers
{
    using System.Diagnostics;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Utils;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public IActionResult Page1()
        {
            var username = HttpContext.Session.GetString("username");
            var passwordMd5 = HttpContext.Session.GetString("password");

            if (username == null || passwordMd5 == null) return RedirectToAction("Index", "Account");

            var password = Md5Helper.DecryptString(passwordMd5);

            return View("Index");
        }
    }
}