using HttpMethod = CustomMVC.Core.Http.HttpMethod;

namespace CustomMVC.Core.Routing;

public sealed class RouteTable
{
    private readonly List<RouteEntry> _routes = new();

    public void Map(HttpMethod method, string template, RouteHandler handler)
    {
        var segs = NormalizePath(template).Split('/', StringSplitOptions.RemoveEmptyEntries);
        _routes.Add(new RouteEntry(method, segs, handler));
    }

    public (RouteEntry entry, Dictionary<string, string> values)? Match(HttpMethod method, string path)
    {
        var reqSegs = NormalizePath(path).Split('/', StringSplitOptions.RemoveEmptyEntries);

        foreach (var r in _routes.Where(r => r.Method == method))
        {
            if (IsMatch(r.Segments, reqSegs, out var vals))
                return (r, vals);
        }
        return null;
    }

    private static string NormalizePath(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return "/";
        var p = raw.Trim();
        if (!p.StartsWith('/')) p = "/" + p;
        return p;
    }

    private static bool IsMatch(string[] tmplSegs, string[] reqSegs, out Dictionary<string, string> values)
    {
        values = new(StringComparer.OrdinalIgnoreCase);

        if (tmplSegs.Length != reqSegs.Length)
            return false;

        for (int i = 0; i < tmplSegs.Length; i++)
        {
            var t = tmplSegs[i];
            var s = reqSegs[i];

            if (t.Length >= 2 && t[0] == '{' && t[^1] == '}')
            {
                var key = t.Trim('{', '}'); 
                values[key] = s;
                continue;
            }

            if (!t.Equals(s, StringComparison.OrdinalIgnoreCase))
                return false;
        }
        return true;
    }
}
