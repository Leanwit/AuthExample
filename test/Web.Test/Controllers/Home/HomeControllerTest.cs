namespace Web.Test.Controllers.Home
{
    using Microsoft.AspNetCore.Mvc;
    using Web.Controllers;
    using Xunit;

    public class HomeControllerTest
    {
        [Fact]
        public void Index_Should_Return_Page()
        {
            var homeController = new HomeController();
            
            var result = homeController.Index();
            
            Assert.IsType<ViewResult>(result);
        }
    }
}