namespace Web.Test.Controllers.Page
{
    using Microsoft.AspNetCore.Mvc;
    using Web.Controllers;
    using Xunit;

    public class PageControllerTest
    {
        [Fact]
        public void PageOne_Return_Correct_Page()
        {
            //Arrange
            var controller = new PageController();

            //Act
            var result = controller.PageOne();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(viewResult.ViewData.ContainsKey("PageTitle"));
            Assert.True(viewResult.ViewData.Values.Contains("PageOne"));
        }

        [Fact]
        public void PageThree_Return_Correct_Page()
        {
            //Arrange
            var controller = new PageController();

            //Act
            var result = controller.PageThree();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(viewResult.ViewData.ContainsKey("PageTitle"));
            Assert.True(viewResult.ViewData.Values.Contains("PageThree"));
        }

        [Fact]
        public void PageTwo_Return_Correct_Page()
        {
            //Arrange
            var controller = new PageController();

            //Act
            var result = controller.PageTwo();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(viewResult.ViewData.ContainsKey("PageTitle"));
            Assert.True(viewResult.ViewData.Values.Contains("PageTwo"));
        }
    }
}