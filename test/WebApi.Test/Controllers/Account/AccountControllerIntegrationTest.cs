namespace WebApi.Test.Controllers.Account
{
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Helper.Controller;
    using Newtonsoft.Json;
    using WebApi.Domain.DTO;
    using WebApi.Infrastructure.Persistence.Seed;
    using Xunit;

    public class AccountControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public AccountControllerIntegrationTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly string _urlAuthenticate = "/api/Account/Authenticate";

        [Fact]
        public async Task Authenticate_Bad_Password()
        {
            // Arrange
            var client = _factory.CreateClient();

            var userDto = new UserDto
            {
                Username = UserDataGenerator.Admin,
                Password = string.Empty
            };

            // Act
            var response = await client.PostAsync(_urlAuthenticate, new StringContent(JsonConvert.SerializeObject(userDto), Encoding.UTF8, "application/json"));

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Authenticate_Bad_Username()
        {
            // Arrange
            var client = _factory.CreateClient();

            var userDto = new UserDto
            {
                Username = string.Empty,
                Password = UserDataGenerator.Admin
            };

            // Act
            var response = await client.PostAsync(_urlAuthenticate, new StringContent(JsonConvert.SerializeObject(userDto), Encoding.UTF8, "application/json"));

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Authenticate_Existing_User()
        {
            // Arrange
            var client = _factory.CreateClient();

            var userDto = new UserDto
            {
                Username = UserDataGenerator.Admin,
                Password = UserDataGenerator.Admin
            };

            // Act
            var response = await client.PostAsync(_urlAuthenticate, new StringContent(JsonConvert.SerializeObject(userDto), Encoding.UTF8, "application/json"));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Authenticate_No_Existing_User()
        {
            // Arrange
            var client = _factory.CreateClient();

            var userDto = new UserDto
            {
                Username = "user",
                Password = "user"
            };

            // Act
            var response = await client.PostAsync(_urlAuthenticate, new StringContent(JsonConvert.SerializeObject(userDto), Encoding.UTF8, "application/json"));

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}