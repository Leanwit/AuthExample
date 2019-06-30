namespace Web.Test.Controllers.Page
{
    using Microsoft.AspNetCore.Mvc;
    using Web.Controllers;
    using Xbehave;
    using Xunit;

    public class PageControllerTest
    {
        [Scenario]
        public void PageOne_Return_Correct_Page(PageController controller, IActionResult result, ViewResult viewResult)
        {
            "Given the page controller"
                .x(() => controller = new PageController());
            "When I execute a page one"
                .x(() => result = controller.PageOne());
            "Then the result is a View Result"
                .x(() => viewResult = Assert.IsType<ViewResult>(result));
            "And the page has view witch contain a PageTitle"
                .x(() => Assert.True(viewResult.ViewData.ContainsKey("PageTitle")));
            "And the page has PageOne as a title"
                .x(() => Assert.True(viewResult.ViewData.Values.Contains("PageOne")));
        }

        [Scenario]
        public void PageTwo_Return_Correct_Page(PageController controller, IActionResult result, ViewResult viewResult)
        {
            "Given the page controller"
                .x(() => controller = new PageController());
            "When I execute a page one"
                .x(() => result = controller.PageTwo());
            "Then the result is a View Result"
                .x(() => viewResult = Assert.IsType<ViewResult>(result));
            "And the page has view witch contain a PageTitle"
                .x(() => Assert.True(viewResult.ViewData.ContainsKey("PageTitle")));
            "And the page has PageOne as a title"
                .x(() => Assert.True(viewResult.ViewData.Values.Contains("PageTwo")));
        }

        [Scenario]
        public void PageThree_Return_Correct_Page(PageController controller, IActionResult result, ViewResult viewResult)
        {
            "Given the page controller"
                .x(() => controller = new PageController());
            "When I execute a page one"
                .x(() => result = controller.PageThree());
            "Then the result is a View Result"
                .x(() => viewResult = Assert.IsType<ViewResult>(result));
            "And the page has view witch contain a PageTitle"
                .x(() => Assert.True(viewResult.ViewData.ContainsKey("PageTitle")));
            "And the page has PageOne as a title"
                .x(() => Assert.True(viewResult.ViewData.Values.Contains("PageThree")));
        }
    }
}