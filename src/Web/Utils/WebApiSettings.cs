namespace Web.Utils
{
    public class WebApiSettings
    {
        public string _authenticateUrl;
        public string _pageUrl;
        public string BaseUrl { get; set; }


        public string PageUrl
        {
            get => BaseUrl + _pageUrl;
            set => _pageUrl = value;
        }

        public string AuthenticateUrl
        {
            get => BaseUrl + _authenticateUrl;
            set => _authenticateUrl = value;
        }
    }
}