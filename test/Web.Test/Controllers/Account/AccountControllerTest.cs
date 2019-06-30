namespace Web.Test.Controllers.Account
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Moq;
    using Web.Controllers;
    using Web.Utils;
    using Xunit;

    public class AccountControllerTest
    {
        [Theory]
        [InlineData("user", "")]
        [InlineData("", "pass")]
        public async Task Login_Invalid_Credentials(string user, string password)
        {
            //Arrange
            var ioSettings = new Mock<IOptions<WebApiSettings>>();
            var accountController = new AccountController(ioSettings.Object);

            //Act
            var result = await accountController.Login(user, password, string.Empty);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("index", viewResult.ViewName);
            Assert.True(viewResult.ViewData.ContainsKey("error"));
            Assert.True(viewResult.ViewData.Values.Contains("Invalid Credentials"));
        }

        [Fact]
        public void Access_Denied_Return_View()
        {
            //Arrange
            var ioSettings = new Mock<IOptions<WebApiSettings>>();
            var accountController = new AccountController(ioSettings.Object);

            //Act
            var result = accountController.AccessDenied();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("AccessDenied", viewResult.ViewName);
        }

        [Fact]
        public void EndUserSession_Execute()
        {
            //Arrange
            var ioSettings = new Mock<IOptions<WebApiSettings>>();
            var accountController = new AccountController(ioSettings.Object);
            accountController.ControllerContext = new ControllerContext();
            accountController.ControllerContext.HttpContext = new DefaultHttpContext();
            accountController.ControllerContext.HttpContext.Session = new Mock<ISession>().Object;

            //Act
            accountController.EndUserSession();
            accountController.ControllerContext.HttpContext.Session.TryGetValue("username", out var username);
            accountController.ControllerContext.HttpContext.Session.TryGetValue("password", out var password);

            //Assert
            Assert.Null(username);
            Assert.Null(password);
        }

        [Fact]
        public void Index_Should_Return_Page()
        {
            //Arrange
            var ioSettings = new Mock<IOptions<WebApiSettings>>();
            var accountController = new AccountController(ioSettings.Object);

            //Act
            var result = accountController.Index(string.Empty);

            //Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Logout_Redirect_To_Index()
        {
            //Arrange
            var ioSettings = new Mock<IOptions<WebApiSettings>>();
            var accountController = new AccountController(ioSettings.Object);
            accountController.ControllerContext = new ControllerContext();
            accountController.ControllerContext.HttpContext = new DefaultHttpContext();
            accountController.ControllerContext.HttpContext.Session = new Mock<ISession>().Object;

            //Act
            var result = accountController.Logout();

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);
        }
    }
}