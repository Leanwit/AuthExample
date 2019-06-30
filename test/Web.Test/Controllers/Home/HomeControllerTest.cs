namespace Web.Test.Controllers.Home
{
    using Microsoft.AspNetCore.Mvc;
    using Web.Controllers;
    using Xbehave;
    using Xunit;

    public class HomeControllerTest
    {
        [Scenario]
        public void Index_Should_Return_Page(HomeController controller, IActionResult result)
        {
            "Given the home controller"
                .x(() => controller = new HomeController());

            "When I executed a Index"
                .x(() => result = controller.Index());

            "Then the result is a View Result"
                .x(() => Assert.IsType<ViewResult>(result));
        }
    }
}