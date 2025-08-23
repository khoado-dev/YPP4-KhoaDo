using HttpMethod = CustomMVC.Core.Http.HttpMethod;

namespace CustomMVC.Core.Routing;

public sealed record RouteEntry(HttpMethod Method, string[] Segments, RouteHandler Handler);
