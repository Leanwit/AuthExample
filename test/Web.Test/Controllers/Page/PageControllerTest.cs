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
            var controller = new PageController();
            
            var result = controller.PageOne();
            
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(viewResult.ViewData.ContainsKey("PageTitle"));
            Assert.True(viewResult.ViewData.Values.Contains("PageOne"));
        }

        [Fact]
        public void PageTwo_Return_Correct_Page()
        {
            var controller = new PageController();
            
            var result = controller.PageTwo();
            
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(viewResult.ViewData.ContainsKey("PageTitle"));
            Assert.True(viewResult.ViewData.Values.Contains("PageTwo"));
        }

        [Fact]
        public void PageThree_Return_Correct_Page()
        {
            var controller = new PageController();
            
            var result = controller.PageThree();
            
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(viewResult.ViewData.ContainsKey("PageTitle"));
            Assert.True(viewResult.ViewData.Values.Contains("PageThree"));
        }
    }
}