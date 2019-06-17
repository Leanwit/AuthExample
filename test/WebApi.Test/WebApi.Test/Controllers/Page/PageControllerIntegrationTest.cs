namespace WebApi.Test.Controllers.Page
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class PageControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private const string NoRoleUser = "norole";
        private const string PageOneUser = "pageone";
        private const string PageTwoUser = "pagetwo";
        private const string PageThreeUser = "pagethree";
        private const string Admin = "admin";
        private readonly CustomWebApplicationFactory<Startup> _factory;


        public PageControllerIntegrationTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/api/Page/Get/1")]
        [InlineData("/api/Page/Get/2")]
        [InlineData("/api/Page/Get/3")]
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
            client.DefaultRequestHeaders.Authorization = CreateRoleAuthorizationHeader(user);

            // Act
            var response = await client.GetAsync(page);

            // Assert
            Assert.Equal(statusCode, response.StatusCode);
        }


        private static AuthenticationHeaderValue CreateRoleAuthorizationHeader(string user)
        {
            return new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(
                    Encoding.ASCII.GetBytes(
                        $"{user}:{user}")));
        }

        public class RoleTestData : IEnumerable<object[]>
        {
            private const string PageOne = "/api/Page/Get/1";
            private const string PageTwo = "/api/Page/Get/2";
            private const string PageThree = "/api/Page/Get/3";


            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] {PageOne, PageOneUser, HttpStatusCode.OK};
                yield return new object[] {PageOne, PageTwoUser, HttpStatusCode.Forbidden};
                yield return new object[] {PageOne, PageThreeUser, HttpStatusCode.Forbidden};
                yield return new object[] {PageOne, NoRoleUser, HttpStatusCode.Forbidden};
                yield return new object[] {PageOne, Admin, HttpStatusCode.OK};


                yield return new object[] {PageTwo, PageOneUser, HttpStatusCode.Forbidden};
                yield return new object[] {PageTwo, PageTwoUser, HttpStatusCode.OK};
                yield return new object[] {PageTwo, PageThreeUser, HttpStatusCode.Forbidden};
                yield return new object[] {PageTwo, NoRoleUser, HttpStatusCode.Forbidden};
                yield return new object[] {PageTwo, Admin, HttpStatusCode.OK};

                yield return new object[] {PageThree, PageOneUser, HttpStatusCode.Forbidden};
                yield return new object[] {PageThree, PageTwoUser, HttpStatusCode.Forbidden};
                yield return new object[] {PageThree, PageThreeUser, HttpStatusCode.OK};
                yield return new object[] {PageThree, NoRoleUser, HttpStatusCode.Forbidden};
                yield return new object[] {PageTwo, Admin, HttpStatusCode.OK};
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}