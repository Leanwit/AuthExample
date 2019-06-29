namespace WebApi.Test.Controllers.Page
{
    using Helper.Database;
    using Microsoft.AspNetCore.Mvc;
    using WebApi.Controllers;
    using Xunit;

    public class PageControllerTest
    {
        [Fact]
        public void PageOne_Ensure_Ok_Status()
        {
            //Arrange
            var controller = new PageController();

            //Act
            var response = controller.PageOne();

            //Assert
            Assert.IsType<OkResult>(response);
        }
        
        [Fact]
        public void PageTwo_Ensure_Ok_Status()
        {
            //Arrange
            var controller = new PageController();

            //Act
            var response = controller.PageTwo();

            //Assert
            Assert.IsType<OkResult>(response);
        }
        
        [Fact]
        public void PageThree_Ensure_Ok_Status()
        {
            //Arrange
            var controller = new PageController();

            //Act
            var response = controller.PageThree();

            //Assert
            Assert.IsType<OkResult>(response);
        }
    }
}