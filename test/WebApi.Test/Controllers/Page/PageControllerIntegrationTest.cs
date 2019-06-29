namespace WebApi.Test.Controllers.Page
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Helper.Controller;
    using WebApi.Infrastructure.Persistence;
    using Xunit;

    public class PageControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private const string PageOne = "/api/Page/PageOne";
        private const string PageTwo = "/api/Page/PageTwo";
        private const string PageThree = "/api/Page/PageThree";

        private readonly CustomWebApplicationFactory<Startup> _factory;

        public PageControllerIntegrationTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData(PageOne)]
        [InlineData(PageTwo)]
        [InlineData(PageThree)]
        public async Task Get_Endpoint_Should_Return_Unauthorized(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [ClassData(typeof(RoleTestData))]
        public async Task Get_Endpoint_Test_Role_Permission(string page, string user, HttpStatusCode statusCode)
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = AutorizationHeader.CreateRoleAuthorizationHeader(user);

            // Act
            var response = await client.GetAsync(page);

            // Assert
            Assert.Equal(statusCode, response.StatusCode);
        }

        public class RoleTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] {PageOne, UserDataGenerator.PageOne, HttpStatusCode.OK};
                yield return new object[] {PageOne, UserDataGenerator.PageTwo, HttpStatusCode.Forbidden};
                yield return new object[] {PageOne, UserDataGenerator.PageThree, HttpStatusCode.Forbidden};
                yield return new object[] {PageOne, UserDataGenerator.NoRole, HttpStatusCode.Forbidden};
                yield return new object[] {PageOne, UserDataGenerator.Admin, HttpStatusCode.OK};
                yield return new object[] {PageOne, UserDataGenerator.PageOneTwo, HttpStatusCode.OK};
                yield return new object[] {PageOne, UserDataGenerator.PageThreeAdmin, HttpStatusCode.OK};

                yield return new object[] {PageTwo, UserDataGenerator.PageOne, HttpStatusCode.Forbidden};
                yield return new object[] {PageTwo, UserDataGenerator.PageTwo, HttpStatusCode.OK};
                yield return new object[] {PageTwo, UserDataGenerator.PageThree, HttpStatusCode.Forbidden};
                yield return new object[] {PageTwo, UserDataGenerator.NoRole, HttpStatusCode.Forbidden};
                yield return new object[] {PageTwo, UserDataGenerator.Admin, HttpStatusCode.OK};
                yield return new object[] {PageTwo, UserDataGenerator.PageOneTwo, HttpStatusCode.OK};
                yield return new object[] {PageTwo, UserDataGenerator.PageThreeAdmin, HttpStatusCode.OK};

                yield return new object[] {PageThree, UserDataGenerator.PageOne, HttpStatusCode.Forbidden};
                yield return new object[] {PageThree, UserDataGenerator.PageTwo, HttpStatusCode.Forbidden};
                yield return new object[] {PageThree, UserDataGenerator.PageThree, HttpStatusCode.OK};
                yield return new object[] {PageThree, UserDataGenerator.NoRole, HttpStatusCode.Forbidden};
                yield return new object[] {PageThree, UserDataGenerator.Admin, HttpStatusCode.OK};
                yield return new object[] {PageThree, UserDataGenerator.PageOneTwo, HttpStatusCode.Forbidden};
                yield return new object[] {PageThree, UserDataGenerator.PageThreeAdmin, HttpStatusCode.OK};
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}