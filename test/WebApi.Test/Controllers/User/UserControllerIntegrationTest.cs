namespace WebApi.Test.Controllers.User
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Helper.Controller;
    using Helper.Database;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Newtonsoft.Json;
    using WebApi.Domain.DTO;
    using WebApi.Infrastructure.Persistence.Seed;
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
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = AutorizationHeader.CreateRoleAuthorizationHeader(UserDataGenerator.Admin);

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.NotEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/User/GetAll", "text/json")]
        [InlineData("/api/User/GetAll", "text/xml")]
        public async Task Get_Endpoint_Should_Return_Json_Content(string url, string contentType)
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = AutorizationHeader.CreateRoleAuthorizationHeader(UserDataGenerator.Admin);
            client.DefaultRequestHeaders.Add(HttpRequestHeader.Accept.ToString(), contentType);

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Contains(contentType, response.Content.Headers.ContentType.MediaType);
        }

        [Theory]
        [InlineData("/api/User/GetAll", "text/html")]
        [InlineData("/api/User/GetAll", "text/pdf")]
        public async Task Get_Endpoint_Should_Return_Not_Acceptable(string url, string contentType)
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = AutorizationHeader.CreateRoleAuthorizationHeader(UserDataGenerator.Admin);
            client.DefaultRequestHeaders.Add(HttpRequestHeader.Accept.ToString(), contentType);

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/User/GetAll")]
        [InlineData("/api/User/Get/id")]
        [InlineData("/api/User/GetByUsername/username")]
        public async Task Get_Endpoint_No_Admin(string url)
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = AutorizationHeader.CreateRoleAuthorizationHeader(UserDataGenerator.PageOne);

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }


        [Theory]
        [InlineData("/")]
        public async Task Swagger_Should_API_Home_Page(string url)
        {
            // Arrange
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

        [Theory]
        [InlineData("/api/User/Post")]
        public async Task Post_No_Admin_Forbidden(string url)
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = AutorizationHeader.CreateRoleAuthorizationHeader(UserDataGenerator.NoRole);

            var userDto = new UserDto();

            // Act
            var response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(userDto), Encoding.UTF8, "application/json"));

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/User/Post")]
        public async Task Post_Admin_Bad_Request(string url)
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = AutorizationHeader.CreateRoleAuthorizationHeader(UserDataGenerator.Admin);

            var userDto = new UserDto();

            // Act
            var response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(userDto), Encoding.UTF8, "application/json"));

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/User/Post")]
        public async Task Post_Admin_Success(string url)
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = AutorizationHeader.CreateRoleAuthorizationHeader(UserDataGenerator.Admin);

            var userDto = new UserDto
            {
                Username = UserSeed.Username,
                Password = UserSeed.Password,
                Id = Guid.NewGuid().ToString()
            };

            // Act
            var response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(userDto), Encoding.UTF8, "application/json"));

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("/api/User/Put/")]
        public async Task Put_No_Admin_Forbidden(string url)
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = AutorizationHeader.CreateRoleAuthorizationHeader(UserDataGenerator.NoRole);

            var userDto = new UserDto();

            // Act
            var response = await client.PutAsync(url, new StringContent(JsonConvert.SerializeObject(userDto), Encoding.UTF8, "application/json"));

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/User/Put/")]
        public async Task Put_Admin_User_Not_Exist(string url)
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = AutorizationHeader.CreateRoleAuthorizationHeader(UserDataGenerator.Admin);

            var userDto = new UserDto();
            userDto.Id = Guid.NewGuid().ToString();

            // Act
            var response = await client.PutAsync(url,
                new StringContent(JsonConvert.SerializeObject(userDto), Encoding.UTF8, "application/json"));

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/User/Put/")]
        public async Task Put_Admin_Success(string url)
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = AutorizationHeader.CreateRoleAuthorizationHeader(UserDataGenerator.Admin);

            var userDto = new UserDto
            {
                Username = UserSeed.Username,
                Password = UserSeed.Password,
                Id = UserSeed.Id
            };

            // Act
            var response = await client.PutAsync(url, new StringContent(JsonConvert.SerializeObject(userDto), Encoding.UTF8, "application/json"));

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/User/Delete/id")]
        public async Task Delete_No_Admin_Forbidden(string url)
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = AutorizationHeader.CreateRoleAuthorizationHeader(UserDataGenerator.NoRole);

            // Act
            var response = await client.DeleteAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/User/Delete/id")]
        public async Task Delete_Admin_User_Not_Exist(string url)
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = AutorizationHeader.CreateRoleAuthorizationHeader(UserDataGenerator.Admin);

            // Act
            var response = await client.DeleteAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/User/Delete/")]
        public async Task Delete_Admin_Success(string url)
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = AutorizationHeader.CreateRoleAuthorizationHeader(UserDataGenerator.Admin);

            // Act
            var response = await client.DeleteAsync(url + UserDataGenerator.GuidUserPageOne);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}