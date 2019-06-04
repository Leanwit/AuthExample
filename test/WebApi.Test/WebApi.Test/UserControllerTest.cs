namespace WebApi.Test
{
    using System.Collections.Generic;
    using Application;
    using Controllers;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using WebApi.Domain;
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