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
            var raw = string.IsNullOrWhiteSpace(request.Path) ? "/" : request.Path.Trim(); // Ensure path is not null or empty
            if (!raw.StartsWith("/")) raw = "/" + raw; // Ensure path starts with '/'

            // 1) Split path & query
            string pathOnly = raw;
            var query = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            int qIdx = raw.IndexOf('?', StringComparison.Ordinal); //get index of '?'
            if (qIdx >= 0)
            {
                pathOnly = raw[..qIdx]; //raw.Substring(0, qIdx);
                var qs = raw[(qIdx + 1)..]; //raw.Substring(qIdx + 1);
                foreach (var pair in qs.Split('&', StringSplitOptions.RemoveEmptyEntries))
                {
                    var kv = pair.Split('=', 2); // Split at first '=' only
                    var k = Uri.UnescapeDataString(kv[0]); //"first%20name" → "first name"
                    var v = kv.Length > 1 ? Uri.UnescapeDataString(kv[1]) : "";
                    query[k] = v; //add to query dictionary
                }
            }

            // NEW: lấy params từ Request.Params
            var reqParams = request.Params ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase); // create dictionary when have params

            var parts = pathOnly.Split('/', StringSplitOptions.RemoveEmptyEntries);

            foreach (var (m, segs, handler) in _routes) // Loop through all registered routes
            {
                if (!string.Equals(m, method, StringComparison.OrdinalIgnoreCase)) continue; // skip if method does not match
                if (parts.Length > segs.Length) continue; // skip if path has more segments registered route

                var vals = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                bool ok = true;

                int j = 0; // j is the index for parts (path segments)
                for (int i = 0; i < segs.Length; i++) // Loop through each segment in the registered route
                {
                    var t = segs[i]; // current segment in the registered route
                    bool isParam = t.StartsWith("{") && t.EndsWith("}"); // check if segment is a parameter

                    if (!isParam) // literal segment
                    {
                        if (j >= parts.Length || !t.Equals(parts[j], StringComparison.OrdinalIgnoreCase)) // over path length or segment mismatch will break
                        { 
                            ok = false; break; 
                        }
                        j++; // move to next part
                        continue; 
                    }

                    // param / optional param
                    var nameRaw = t[1..^1]; // "/UserId?/" -> "UserId?"
                    bool optional = nameRaw.EndsWith("?"); // check if param is optional
                    var name = optional ? nameRaw[..^1] : nameRaw; // remove '?' if optional

                    if (j < parts.Length) // still have parts left to match
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
