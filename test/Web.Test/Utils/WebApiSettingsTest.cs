namespace Web.Test.Utils
{
    using Web.Utils;
    using Xunit;

    public class WebApiSettingsTest
    {
        [Theory]
        [InlineData("http://localhost/", "Page/{0}", "http://localhost/Page/{0}")]
        [InlineData("https://domain/", "/Page/", "https://domain//Page/")]
        public void PageUrl_Is_Correct_Formed(string baseUrl, string pageUrl, string result)
        {
            var settings = new WebApiSettings();
            settings.BaseUrl = baseUrl;
            settings.PageUrl = pageUrl;

            Assert.NotEmpty(settings.BaseUrl);
            Assert.NotEmpty(settings.PageUrl);
            Assert.Equal(settings.BaseUrl, baseUrl);
            Assert.Equal(settings.PageUrl, result);
        }

        [Theory]
        [InlineData("http://localhost/", "Api/authenticate/", "http://localhost/Api/authenticate/")]
        [InlineData("https://domain/", "authenticate/@", "https://domain/authenticate/@")]
        public void AuthenticateUrl_Is_Correct_Formed(string baseUrl, string authenticateUrl, string result)
        {
            var settings = new WebApiSettings();
            settings.BaseUrl = baseUrl;
            settings.AuthenticateUrl = authenticateUrl;

            Assert.NotEmpty(settings.BaseUrl);
            Assert.NotEmpty(settings.AuthenticateUrl);
            Assert.Equal(settings.BaseUrl, baseUrl);
            Assert.Equal(settings.AuthenticateUrl, result);
        }
    }
}