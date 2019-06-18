namespace Web.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    public class PageController : Controller
    {
        private async Task<IActionResult> ExecutePage(string pageTitle)
        {
            ViewBag.PageTitle = pageTitle;
            ViewBag.Logged = true;
            return View("Index");
        }

        [RoleAuthenticateFilter(Page = nameof(PageOne))]
        public async Task<IActionResult> PageOne()
        {
            return await ExecutePage(nameof(PageOne));
        }

        [RoleAuthenticateFilter(Page = nameof(PageTwo))]
        public async Task<IActionResult> PageTwo()
        {
            return await ExecutePage(nameof(PageTwo));
        }

        [RoleAuthenticateFilter(Page = nameof(PageThree))]
        public async Task<IActionResult> PageThree()
        {
            return await ExecutePage(nameof(PageThree));
        }
    }
}