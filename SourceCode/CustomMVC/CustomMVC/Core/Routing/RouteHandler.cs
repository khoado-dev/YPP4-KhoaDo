using CustomMVC.Core.Http;

namespace CustomMVC.Core.Routing
{
    public delegate Task RouteHandler(HttpContext httpContext, IDictionary<string, string> routeValues);
}
