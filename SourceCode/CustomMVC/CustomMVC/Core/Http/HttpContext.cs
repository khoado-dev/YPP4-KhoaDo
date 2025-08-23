namespace CustomMVC.Core.Http
{
    public sealed class HttpContext
    {
        public HttpRequest Request { get; }
        public HttpResponse Response { get; }
        public IDictionary<string, object?> Items { get; } = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase); //for sharing data within the same request
        public IServiceProvider? Services { get; set; } //for Dependency Injection

        public HttpContext(HttpRequest req, HttpResponse res) { Request = req; Response = res; }
    }
}
