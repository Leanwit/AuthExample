namespace Web.Test.Services
{
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using Moq;
    using Web.Services;
    using Web.Utils;
    using Xunit;

    public class RoleAuthenticateServiceTest
    {
        [Fact]
        public async Task Execute_Return_Bad_Request()
        {
            //Arrange
            var service = new RoleAuthenticateService(new Mock<IOptions<WebApiSettings>>().Object);
            var context = new Mock<HttpContext>();
            context.Setup(x => x.Session).Returns(new Mock<ISession>().Object);

            //Act
            var response = await service.Execute("PageOne", context.Object);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response);
        }
    }
}