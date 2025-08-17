using System;
using System.Collections.Generic;
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
            var raw = string.IsNullOrWhiteSpace(request.Path) ? "/" : request.Path.Trim();
            if (!raw.StartsWith("/")) raw = "/" + raw;

            // 1) Split path & query
            string pathOnly = raw;
            var query = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            int qIdx = raw.IndexOf('?', StringComparison.Ordinal);
            if (qIdx >= 0)
            {
                pathOnly = raw[..qIdx];
                var qs = raw[(qIdx + 1)..];
                foreach (var pair in qs.Split('&', StringSplitOptions.RemoveEmptyEntries))
                {
                    var kv = pair.Split('=', 2);
                    var k = Uri.UnescapeDataString(kv[0]);
                    var v = kv.Length > 1 ? Uri.UnescapeDataString(kv[1]) : "";
                    query[k] = v;
                }
            }

            // NEW: lấy params từ Request.Params
            var reqParams = request.Params ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            var parts = pathOnly.Split('/', StringSplitOptions.RemoveEmptyEntries);

            foreach (var (m, segs, handler) in _routes)
            {
                if (!string.Equals(m, method, StringComparison.OrdinalIgnoreCase)) continue;
                if (parts.Length > segs.Length) continue;

                var vals = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                bool ok = true;

                int j = 0;
                for (int i = 0; i < segs.Length; i++)
                {
                    var t = segs[i];
                    bool isParam = t.StartsWith("{") && t.EndsWith("}");

                    if (!isParam)
                    {
                        if (j >= parts.Length || !t.Equals(parts[j], StringComparison.OrdinalIgnoreCase))
                        { ok = false; break; }
                        j++;
                        continue;
                    }

                    // param / optional param
                    var nameRaw = t[1..^1];
                    bool optional = nameRaw.EndsWith("?");
                    var name = optional ? nameRaw[..^1] : nameRaw;

                    if (j < parts.Length)
                    {
                        if (optional)
                        {
                            // Lookahead: nếu segment tiếp theo trong template là literal
                            // và đúng bằng parts[j], thì KHÔNG ăn parts[j] cho param optional
                            bool nextIsLiteralMatch =
                                (i + 1 < segs.Length) &&
                                !(segs[i + 1].StartsWith("{") && segs[i + 1].EndsWith("}")) &&
                                segs[i + 1].Equals(parts[j], StringComparison.OrdinalIgnoreCase);

                            if (!nextIsLiteralMatch)
                            {
                                vals[name] = parts[j++]; // lấy từ path
                            }
                            else if (query.TryGetValue(name, out var qv) || reqParams.TryGetValue(name, out qv))
                            {
                                vals[name] = qv;         // lấy từ query hoặc Request.Params
                            }
                            else
                            {
                                ok = false; break;        // thiếu giá trị
                            }
                        }
                        else
                        {
                            vals[name] = parts[j++];      // required param phải lấy từ path
                        }
                    }
                    else
                    {
                        // path hết segment: thử lấy từ query hoặc Request.Params nếu optional
                        if (optional && (query.TryGetValue(name, out var qv) || reqParams.TryGetValue(name, out qv)))
                        {
                            vals[name] = qv;
                        }
                        else
                        {
                            ok = false; break;            // thiếu required
                        }
                    }
                }

                if (!ok) continue;

                // Merge các query/params còn lại: không ghi đè các key đã có từ path
                foreach (var kv in query)
                    if (!vals.ContainsKey(kv.Key)) vals[kv.Key] = kv.Value;
                foreach (var kv in reqParams)
                    if (!vals.ContainsKey(kv.Key)) vals[kv.Key] = kv.Value;

                try
                {
                    using var scope = _scopeFactory();
                    var data = handler(vals, scope.ServiceProvider);

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

            return new Response { StatusCode = HttpStatus.NotFound, Body = null };
        }

    }
}
