using PureDI;
using UnitTestForTrello.Models;
using HttpMethod = UnitTestForTrello.Models.HttpMethod;

namespace UnitTestForTrello.Routers
{
    // Please dont touch it
    public sealed class Router
    {
        private readonly List<(string method, string[] segs,
            Func<IDictionary<string, string>, IServiceProvider, object?> handler)> _routes = new();

        private readonly Func<IServiceScope> _scopeFactory;
        public Router(Func<IServiceScope> scopeFactory) => _scopeFactory = scopeFactory;

        // Map bằng string (giữ nguyên)
        public void Map(string method, string template,
            Func<IDictionary<string, string>, IServiceProvider, object?> handler)
        {
            var segs = template.Split('/', StringSplitOptions.RemoveEmptyEntries);
            _routes.Add((method.ToUpperInvariant(), segs, handler));
        }

        // Map bằng enum (tiện lợi)
        public void Map(HttpMethod method, string template,
            Func<IDictionary<string, string>, IServiceProvider, object?> handler)
            => Map(method.ToString(), template, handler);

        // Overload nhận Request (enum), trả về Response
        public Response Handle(Request request)
        {
            var method = request.Method.ToString();                 // enum -> "GET"
            var path = string.IsNullOrWhiteSpace(request.Path) ? "/" : request.Path;
            if (!path.StartsWith("/")) path = "/" + path;
            return Handle(method, path);
        }

        // Core: trả về Response với HttpStatus enum
        public Response Handle(string method, string path)
        {
            var parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);

            foreach (var (m, segs, handler) in _routes)
            {
                if (!string.Equals(m, method, StringComparison.OrdinalIgnoreCase)) continue; // if method not match then ignore
                if (segs.Length != parts.Length) continue; // if number of segments not match then ignore

                var vals = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                bool ok = true;

                for (int i = 0; i < segs.Length; i++) // go through each segment
                {
                    var t = segs[i]; //setment in registed routes 
                    var p = parts[i]; //segment in path of request

                    if (t.StartsWith("{") && t.EndsWith("}")) 
                        vals[t[1..^1]] = p; // => vals["userId"] = "123";
                    else if (!t.Equals(p, StringComparison.OrdinalIgnoreCase)) // if segment is not a variable and not match then ignore
                    { ok = false; break; }
                }
                if (!ok) continue;

                try
                {
                    using var scope = _scopeFactory(); // dispose scope per request
                    var data = handler(vals, scope.ServiceProvider); // call the handler/method with the matched segments

                    return new Response
                    {
                        StatusCode = HttpStatus.OK,
                        Body = data
                    };
                }
                catch (Exception ex)
                {
                    return new Response
                    {
                        StatusCode = HttpStatus.InternalServerError,
                        Body = ex
                    };
                }
            }

            return new Response
            {
                StatusCode = HttpStatus.NotFound,
                Body = null
            };
        }
    }
}
