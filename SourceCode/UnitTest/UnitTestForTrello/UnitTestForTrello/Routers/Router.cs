
using System.IO;
using UnitTestForTrello.Tests.Utility;

namespace UnitTestForTrello.Tests
{
    public class Router
    {
        private readonly List<(RequestMethod method,
        string[] segs,
        Func<IDictionary<string, string>, 
            object?> handler)> _registeredRoutes = new();

        public Router()
        {
        }

        public void Map(RequestMethod method, 
            string templatePath, 
            Func<IDictionary<string, string>, object?> hander)
        {
            var segs = templatePath.Trim('/').Split('/');
            _registeredRoutes.Add((method, segs, hander));
        }

        public ResponseDTO Handle(RequestDTO request)
        {
            var reqSegs = request.Path.Trim('/').Split('/');

            var match = _registeredRoutes
                .Where(r => r.method == request.Method)
                .Select(r => (r, matched: IsMatchRoute(r.segs, reqSegs, out var vals), vals))
                .FirstOrDefault(x => x.matched); 

            var data = match.r.handler(match.vals);
            return new ResponseDTO { Data = data };
        }

        private bool IsMatchRoute(string[] templateSegs, string[] requestSegs,
                          out Dictionary<string, string> values)
        {
            values = new(StringComparer.OrdinalIgnoreCase);

            // 1) normalized path segments + separate query
            var pathSegs = requestSegs
                .Select(seg =>
                {
                    var parts = seg.Split('?', 2);
                    return (path: parts[0],
                            query: parts.Length > 1 ? parts[1] : null);
                })
                .ToList();

            // 2) Parse all query params in segment have '?'
            var queryParams = pathSegs
                .Where(x => !string.IsNullOrEmpty(x.query)) //ensure query is not null or empty
                .SelectMany(x => x.query!.Split('&', StringSplitOptions.RemoveEmptyEntries)) // Split by '&' to get each query param and remove empty entries
                .Select(piece => piece.Split('=', 2)) // Split by '=' to separate key and value
                .ToDictionary(kv => kv[0], kv => kv.Length > 1 ? kv[1] : "", // Ensure value is not null, if no value then use empty string
                              StringComparer.OrdinalIgnoreCase);

            // 3) only get path
            var purePathSegs = pathSegs
                .Select(x => x.path)
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();

            if (templateSegs.Length != purePathSegs.Count)
                return false;

            // 4) Match template vs path
            foreach (var (tmpl, req) in templateSegs.Zip(purePathSegs))
            {
                if (tmpl.StartsWith("{") && tmpl.EndsWith("}"))
                {
                    values[tmpl[1..^1]] = req; 
                }
                else if (!tmpl.Equals(req, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            // 5) Merge query params (query override route)
            foreach (var (k, v) in queryParams)
                values[k] = v;

            return true;
        }
    }
}