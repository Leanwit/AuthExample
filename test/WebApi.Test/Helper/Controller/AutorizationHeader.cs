namespace WebApi.Test.Helper.Controller
{
    using System;
    using System.Net.Http.Headers;
    using System.Text;

    public static class AutorizationHeader
    {
        public static AuthenticationHeaderValue CreateRoleAuthorizationHeader(string user)
        {
            return new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(
                    Encoding.ASCII.GetBytes(
                        $"{user}:{user}")));
        }
    }
}