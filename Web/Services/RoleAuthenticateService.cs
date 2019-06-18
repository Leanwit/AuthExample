namespace Web.Services
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using Utils;

    public class RoleAuthenticateService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly WebApiSettings _webApiSettings;

        public RoleAuthenticateService(IOptions<WebApiSettings> webApiSettings)
        {
            _webApiSettings = webApiSettings.Value;
        }

        public async Task<HttpStatusCode> Execute(string page, HttpContext context)
        {
            var username = context.Session.GetString("username");
            var passwordMd5 = context.Session.GetString("password");

            if (username == null || passwordMd5 == null) return HttpStatusCode.BadRequest;

            var password = Md5Helper.DecryptString(passwordMd5);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(
                    Encoding.ASCII.GetBytes(
                        $"{username}:{password}")));


            var response = await _httpClient.GetAsync(string.Format(_webApiSettings.PageUrl, page));

            return response.StatusCode;
        }
    }
}