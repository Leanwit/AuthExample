namespace WebApi.Test.Controllers.Page
{
    using Microsoft.AspNetCore.Mvc;
    using WebApi.Controllers;
    using Xbehave;
    using Xunit;

    public class PageControllerTest
    {
        [Scenario]
        public void PageOne_Ensure_Ok_Status(PageController controller, ActionResult result)
        {
            "Given the controller"
                .x(() => controller = new PageController());

            "When I executed a PageOne"
                .x(() => result = controller.PageOne());

            "Then the result is Ok"
                .x(() => Assert.IsType<OkResult>(result));
        }

        [Scenario]
        public void PageTwo_Ensure_Ok_Status(PageController controller, ActionResult result)
        {
            "Given the controller"
                .x(() => controller = new PageController());

            "When I executed a PageTwo"
                .x(() => result = controller.PageTwo());

            "Then the result is Ok"
                .x(() => Assert.IsType<OkResult>(result));
        }

        [Scenario]
        public void PageThree_Ensure_Ok_Status(PageController controller, ActionResult result)
        {
            "Given the controller"
                .x(() => controller = new PageController());

            "When I executed a PageThree"
                .x(() => result = controller.PageThree());

            "Then the result is Ok"
                .x(() => Assert.IsType<OkResult>(result));
        }
    }
}