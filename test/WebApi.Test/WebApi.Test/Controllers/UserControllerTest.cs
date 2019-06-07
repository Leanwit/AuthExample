namespace WebApi.Test.Controllers
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using WebApi.Application;
    using WebApi.Controllers;
    using WebApi.Domain.DTO;
    using Xunit;

    public class UserControllerTest
    {
        [Fact]
        public void Get_Is_No_Authenticated()
        {
            var controller = new UserController(new Mock<IUserFinder>().Object);

            var result = controller.GetAll();

            Assert.IsType<ActionResult<IEnumerable<UserDto>>>(result);
            Assert.NotNull(result);
        }
    }
}