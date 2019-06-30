namespace Web.Test.Controllers.Account
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Moq;
    using Web.Controllers;
    using Web.Utils;
    using Xbehave;
    using Xunit;

    public class AccountControllerTest
    {
        [Scenario]
        public void Login_Invalid_Credentials_password(AccountController controller, IActionResult result, ViewResult viewResult)
        {
            "Given the account controller"
                .x(() => controller = CreateAccountController());

            "When I login without password value"
                .x(async () => result = await controller.Login("user", string.Empty, string.Empty));

            "Then the result is a ViewResult"
                .x(() => viewResult = Assert.IsType<ViewResult>(result));

            "And the view is index"
                .x(() => Assert.Equal("index", viewResult.ViewName));

            "And view data have invalid credentials message"
                .x(() =>
                {
                    Assert.True(viewResult.ViewData.ContainsKey("error"));
                    Assert.True(viewResult.ViewData.Values.Contains("Invalid Credentials"));
                });
        }

        [Scenario]
        public void Login_Invalid_Credentials_username(AccountController controller, IActionResult result, ViewResult viewResult)
        {
            "Given the account controller"
                .x(() => controller = CreateAccountController());

            "When I login without password value"
                .x(async () => result = await controller.Login(string.Empty, "password", string.Empty));

            "Then the result is a ViewResult"
                .x(() => viewResult = Assert.IsType<ViewResult>(result));

            "And the view is index"
                .x(() => Assert.Equal("index", viewResult.ViewName));

            "And view data have invalid credentials message"
                .x(() =>
                {
                    Assert.True(viewResult.ViewData.ContainsKey("error"));
                    Assert.True(viewResult.ViewData.Values.Contains("Invalid Credentials"));
                });
        }


        [Scenario]
        public void Access_Denied_Return_View(AccountController controller, IActionResult result, ViewResult viewResult)
        {
            "Given the account controller"
                .x(() => controller = CreateAccountController());

            "When I enter to access denied"
                .x(() => result = controller.AccessDenied());

            "Then the result is a ViewResult"
                .x(() => viewResult = Assert.IsType<ViewResult>(result));

            "And the view is AccessDenied"
                .x(() => Assert.Equal("AccessDenied", viewResult.ViewName));
        }

        [Scenario]
        public void Index_Should_Return_Page(AccountController controller, IActionResult result, ViewResult viewResult)
        {
            "Given the account controller"
                .x(() => controller = CreateAccountController());

            "When I enter to access denied"
                .x(() => result = controller.Index(string.Empty));

            "Then the result is a ViewResult"
                .x(() => viewResult = Assert.IsType<ViewResult>(result));

            "And the view is Index"
                .x(() => Assert.Equal("Index", viewResult.ViewName));
        }


        [Scenario]
        public void EndUserSession_Execute(AccountController controller)
        {
            "Given the account controller"
                .x(() => controller = CreateAccountControllerWithContext());

            "When I end the user session"
                .x(() => controller.EndUserSession());

            "Then username is null"
                .x(() =>
                {
                    controller.ControllerContext.HttpContext.Session.TryGetValue("username", out var username);
                    Assert.Null(username);
                });

            "And password is null"
                .x(() =>
                {
                    controller.ControllerContext.HttpContext.Session.TryGetValue("username", out var password);
                    Assert.Null(password);
                });
        }


        [Scenario]
        public void Logout_Redirect_To_Index(AccountController controller, IActionResult result, ViewResult viewResult)
        {
            "Given the account controller"
                .x(() => controller = CreateAccountControllerWithContext());

            "When I end the user session"
                .x(() => result = controller.Logout());

            "Then the result is a ViewResult"
                .x(() => viewResult = Assert.IsType<ViewResult>(result));

            "And the view is Index"
                .x(() => Assert.Equal("Index", viewResult.ViewName));
        }

        private AccountController CreateAccountController()
        {
            var ioSettings = new Mock<IOptions<WebApiSettings>>();
            return new AccountController(ioSettings.Object);
        }

        private AccountController CreateAccountControllerWithContext()
        {
            var ioSettings = new Mock<IOptions<WebApiSettings>>();
            var accountController = new AccountController(ioSettings.Object);
            accountController.ControllerContext = new ControllerContext();
            accountController.ControllerContext.HttpContext = new DefaultHttpContext();
            accountController.ControllerContext.HttpContext.Session = new Mock<ISession>().Object;

            return accountController;
        }
    }
}