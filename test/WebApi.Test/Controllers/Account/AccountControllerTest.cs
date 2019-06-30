namespace WebApi.Test.Controllers.Account
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using WebApi.Application;
    using WebApi.Controllers;
    using WebApi.Domain.DTO;
    using Xbehave;
    using Xunit;

    public class AccountControllerTest
    {
        [Scenario]
        public void Authenticate_Existing_User(AccountController controller, string username, string password, ActionResult result)
        {
            "Given the account controller"
                .x(() => controller = CreateControllerReturnDto());

            "And the username admin"
                .x(() => username = "admin");

            "And the password admin"
                .x(() => password = "admin");

            "When I authenticate this credentials"
                .x(async () => result = await controller.Authenticate(new UserDto {Username = username, Password = password}));

            "Then the result is ok"
                .x(() => Assert.IsType<OkResult>(result));
        }

        [Scenario]
        public void Authenticate_No_Existing_User(AccountController controller, string username, string password, ActionResult result)
        {
            "Given the account controller"
                .x(() => controller = CreateControllerNoReturnDto());

            "And the username admin"
                .x(() => username = "admin");

            "And the password admin"
                .x(() => password = "admin");

            "When I authenticate this credentials"
                .x(async () => result = await controller.Authenticate(new UserDto {Username = username, Password = password}));

            "Then the result is not found"
                .x(() => Assert.IsType<NotFoundResult>(result));
        }

        [Scenario]
        public void Authenticate_Bad_Username(AccountController controller, string username, string password, ActionResult result)
        {
            "Given the account controller"
                .x(() => controller = CreateControllerReturnDto());

            "And the username empty"
                .x(() => username = string.Empty);

            "And the password admin"
                .x(() => password = "admin");

            "When I authenticate this credentials"
                .x(async () => result = await controller.Authenticate(new UserDto {Username = username, Password = password}));

            "Then the result is bad request "
                .x(() => Assert.IsType<BadRequestObjectResult>(result));
        }

        [Scenario]
        public void Authenticate_Bad_Password(AccountController controller, string username, string password, ActionResult result)
        {
            "Given the account controller"
                .x(() => controller = CreateControllerReturnDto());

            "And the username admin"
                .x(() => username = "admin");

            "And the password empty"
                .x(() => password = string.Empty);

            "When I authenticate this credentials"
                .x(async () => result = await controller.Authenticate(new UserDto {Username = username, Password = password}));

            "Then the result is bad request object"
                .x(() => Assert.IsType<BadRequestObjectResult>(result));
        }

        private AccountController CreateControllerReturnDto()
        {
            var mock = new Mock<IUserAuthenticate>();
            mock.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(new UserDto()));
            return new AccountController(mock.Object);
        }

        private AccountController CreateControllerNoReturnDto()
        {
            var mock = new Mock<IUserAuthenticate>();
            mock.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult<UserDto>(null));
            return new AccountController(mock.Object);
        }
    }
}