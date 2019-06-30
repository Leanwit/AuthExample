namespace Web.Test
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Web.Services;
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
            Assert.NotNull(webHost.Services.GetRequiredService<RoleAuthenticateService>());
        }
    }
}