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
            //Arrange
            var homeController = new HomeController();

            //Act
            var result = homeController.Index();

            //Assert
            Assert.IsType<ViewResult>(result);
        }
    }
}