namespace Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Services.Filters;

    public class PageController : Controller
    {
        private IActionResult ExecutePage(string pageTitle)
        {
            ViewBag.PageTitle = pageTitle;
            ViewBag.Logged = true;
            return View("Index");
        }

        [RoleAuthenticateFilter(Page = nameof(PageOne))]
        public IActionResult PageOne()
        {
            return ExecutePage(nameof(PageOne));
        }

        [RoleAuthenticateFilter(Page = nameof(PageTwo))]
        public IActionResult PageTwo()
        {
            return ExecutePage(nameof(PageTwo));
        }

        [RoleAuthenticateFilter(Page = nameof(PageThree))]
        public IActionResult PageThree()
        {
            return ExecutePage(nameof(PageThree));
        }
    }
}