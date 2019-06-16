namespace WebApi.Test.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Xunit;

    public class UserControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public UserControllerIntegrationTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/api/User/GetAll")]
        [InlineData("/api/User/Get/id")]
        [InlineData("/api/User/GetByUsername/username")]
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
        [InlineData("/api/User/GetAll")]
        [InlineData("/api/User/Get/id")]
        [InlineData("/api/User/GetByUsername/username")]
        public async Task Get_Endpoint_Admin_Should_No_Return_Unauthorized(string url)
        {
            var client = _factory.CreateClient();

            client.DefaultRequestHeaders.Authorization = CreateValidAuthorizationHeader();
            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.NotEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("/")]
        public async Task Swagger_Should_API_Home_Page(string url)
        {
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true
            });
            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html",
                response.Content.Headers.ContentType.ToString());
        }

        private static AuthenticationHeaderValue CreateValidAuthorizationHeader()
        {
            return new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(
                    Encoding.ASCII.GetBytes(
                        "admin:admin")));
        }
    }
}