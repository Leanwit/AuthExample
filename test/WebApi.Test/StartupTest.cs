namespace WebApi.Test
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using WebApi.Application;
    using WebApi.Domain;
    using WebApi.Domain.DTO;
    using Xunit;

    public class StartupTest
    {
        [Fact]
        public void Startup_Init()
        {
            //Arrange
            var startup = WebHost.CreateDefaultBuilder().UseStartup<Startup>();

            //Act
            var webHost = startup.Build();

            //Assert
            Assert.NotNull(webHost);
            Assert.NotNull(webHost.Services.GetRequiredService<IUserRepository>());
            Assert.NotNull(webHost.Services.GetRequiredService<IUserAuthenticate>());
            Assert.NotNull(webHost.Services.GetRequiredService<IUserFind<UserFindDto>>());
            Assert.NotNull(webHost.Services.GetRequiredService<IUserDelete<UserDto>>());
            Assert.NotNull(webHost.Services.GetRequiredService<IUserCreate<UserDto>>());
            Assert.NotNull(webHost.Services.GetRequiredService<IUserUpdate<UserDto>>());
        }
    }
}