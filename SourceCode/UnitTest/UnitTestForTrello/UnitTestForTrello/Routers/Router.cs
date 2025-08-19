using System;
using System.Collections.Generic;
using System.Linq;
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

        // Map by string (kept)
        public void Map(string method, string template,
            Func<IDictionary<string, string>, IServiceProvider, object?> handler)
        {
            var segs = template.Split('/', StringSplitOptions.RemoveEmptyEntries);
            _routes.Add((method.ToUpperInvariant(), segs, handler));
        }

        // Map by enum (convenience)
        public void Map(HttpMethod method, string template,
            Func<IDictionary<string, string>, IServiceProvider, object?> handler)
            => Map(method.ToString(), template, handler);

        public Response Handle(Request request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var method = request.Method.ToString();
            var (pathOnly, query) = SplitPathAndQuery(request.Path);
            var reqParams = request.Params ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var parts = pathOnly.Split('/', StringSplitOptions.RemoveEmptyEntries);

            var match = _routes
                .Where(r => string.Equals(r.method, method, StringComparison.OrdinalIgnoreCase))
                .Select(r => TryMatch(r, parts, query, reqParams))
                .FirstOrDefault(r => r.ok);

            if (match.ok)
            {
                try
                {
                    using var scope = _scopeFactory();
                    var data = match.handler(match.vals, scope.ServiceProvider);
                    return new Response { StatusCode = HttpStatus.OK, Body = data };
                }
                catch (Exception ex)
                {
                    return new Response { StatusCode = HttpStatus.InternalServerError, Body = ex };
                }
            }

            return new Response { StatusCode = HttpStatus.NotFound, Body = null };
        }

        private static (string path, Dictionary<string, string> query) SplitPathAndQuery(string? raw)
        {
            raw = string.IsNullOrWhiteSpace(raw) ? "/" : raw.Trim();
            if (!raw.StartsWith("/")) raw = "/" + raw;

            var query = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var path = raw;
            var qIdx = raw.IndexOf('?', StringComparison.Ordinal);
            if (qIdx >= 0)
            {
                path = raw[..qIdx];
                var qs = raw[(qIdx + 1)..];
                foreach (var pair in qs.Split('&', StringSplitOptions.RemoveEmptyEntries))
                {
                    var kv = pair.Split('=', 2);
                    var k = Uri.UnescapeDataString(kv[0]);
                    var v = kv.Length > 1 ? Uri.UnescapeDataString(kv[1]) : "";
                    query[k] = v;
                }
            }
            return (path, query);
        }

        private static (bool ok, IDictionary<string, string> vals,
            Func<IDictionary<string, string>, IServiceProvider, object?> handler)
            TryMatch((string method, string[] segs,
                      Func<IDictionary<string, string>, IServiceProvider, object?> handler) route,
                     string[] parts,
                     Dictionary<string, string> query,
                     Dictionary<string, string> reqParams)
        {
            var vals = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            int pi = 0; // pointer for parts

            foreach (var (seg, si) in route.segs.Select((s, i) => (s, i)))
            {
                // literal segment
                if (!IsParam(seg))
                {
                    if (pi >= parts.Length || !seg.Equals(parts[pi], StringComparison.OrdinalIgnoreCase))
                        return (false, vals, route.handler);
                    pi++;
                    continue;
                }

                // parameter segment
                var (name, optional) = ParseParam(seg);
                var canConsumePath = pi < parts.Length;

                // For optional param: if next segment is a literal equal to parts[pi], don't consume path here
                var nextIsLiteralMatch =
                    optional &&
                    si + 1 < route.segs.Length &&
                    !IsParam(route.segs[si + 1]) &&
                    canConsumePath &&
                    route.segs[si + 1].Equals(parts[pi], StringComparison.OrdinalIgnoreCase);

                if (canConsumePath && !nextIsLiteralMatch)
                {
                    // take from path
                    vals[name] = parts[pi++];
                    continue;
                }

                // try query/params for optional
                if (optional && (query.TryGetValue(name, out var qv) || reqParams.TryGetValue(name, out qv)))
                {
                    vals[name] = qv;
                    continue;
                }

                // missing required or optional has no value anywhere
                if (!optional) return (false, vals, route.handler);
                // if optional but no value, just skip this seg
            }

            // If leftover path segments → fail
            if (pi != parts.Length) return (false, vals, route.handler);

            // merge query & params without overriding path-captured keys
            foreach (var kv in query) if (!vals.ContainsKey(kv.Key)) vals[kv.Key] = kv.Value;
            foreach (var kv in reqParams) if (!vals.ContainsKey(kv.Key)) vals[kv.Key] = kv.Value;

            return (true, vals, route.handler);
        }

        private static bool IsParam(string s) => s.StartsWith("{") && s.EndsWith("}");
        private static (string name, bool optional) ParseParam(string seg)
        {
            var raw = seg[1..^1];
            return raw.EndsWith("?")
                ? (raw[..^1], true)
                : (raw, false);
        }
    }
}
