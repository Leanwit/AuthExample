namespace Web.Test
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Services;
    using Xunit;

    public class StartupTest
    {
        [Fact]
        public void Startup_Init()
        {
            var webHost = WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();
            Assert.NotNull(webHost);
            Assert.NotNull(webHost.Services.GetRequiredService<RoleAuthenticateService>());
        }
    }
}