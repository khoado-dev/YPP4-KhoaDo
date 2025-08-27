namespace CustomMVC.Core.Routing
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class RouteAttribute : Attribute
    {
        public string Template { get; }
        public RouteAttribute(string template) => Template = template ?? "";
    }

    // Base attribute for HTTP methods
    public abstract class HttpMethodAttribute : Attribute
    {
        public Http.HttpMethod Method { get; }
        public string Template { get; }
        protected HttpMethodAttribute(Http.HttpMethod method, string? template = null)
        {
            Method = method;
            Template = template ?? "";
        }
    }

    // Specific HTTP method attributes
    public sealed class HttpGetAttribute : HttpMethodAttribute { public HttpGetAttribute(string? t = null) : base(Http.HttpMethod.GET, t) { } }
    public sealed class HttpPostAttribute : HttpMethodAttribute { public HttpPostAttribute(string? t = null) : base(Http.HttpMethod.POST, t) { } }
    public sealed class HttpPutAttribute : HttpMethodAttribute { public HttpPutAttribute(string? t = null) : base(Http.HttpMethod.PUT, t) { } }
    public sealed class HttpDeleteAttribute : HttpMethodAttribute { public HttpDeleteAttribute(string? t = null) : base(Http.HttpMethod.DELETE, t) { } }
    public sealed class HttpPatchAttribute : HttpMethodAttribute { public HttpPatchAttribute(string? t = null) : base(Http.HttpMethod.PATCH, t) { } }
}
