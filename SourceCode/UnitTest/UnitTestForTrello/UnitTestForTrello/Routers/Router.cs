
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

            if (templateSegs.Length != requestSegs.Length)
                return false;

            // Pair each template segment with the corresponding request segment (by index)
            var pairs = templateSegs.Zip(requestSegs);

            foreach (var (tmpl, req) in pairs)
            {
                if (tmpl.StartsWith("{") && tmpl.EndsWith("}"))
                    values[tmpl.Trim('{', '}')] = req;
                else if (!string.Equals(tmpl, req))
                    return false;
            }

            return true;
        }


    }
}