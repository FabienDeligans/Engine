namespace BlazorMongoTemplateApp.Pages
{
    public partial class Index
    {
        public string UserAgent { get; set; }
        public string IPAddress { get; set; }

        protected override void OnInitialized()
        {
            UserAgent = httpContextAccessor.HttpContext.Request.Headers["FromUser-Agent"];
            IPAddress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}
