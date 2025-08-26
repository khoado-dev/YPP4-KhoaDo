using System.Net;
using System.Text.Json;
using System.Web;
using UnitTestForTrello.Tests;
using UnitTestForTrello.Tests.Utility;
namespace UnitTestForTrello
{

    public static class HttpRequestAdapter
    {
        public static RequestDTO From(HttpListenerRequest req)
        {
            var query = HttpUtility.ParseQueryString(req.Url!.Query);
            var queryDict = query.AllKeys!
                                 .Where(k => k != null)
                                 .ToDictionary(k => k!, k => query[k]!);

            string body = "";
            using (var sr = new StreamReader(req.InputStream, req.ContentEncoding))
                body = sr.ReadToEnd();

            return new RequestDTO
            {
                Method = req.HttpMethod switch
                {
                    "GET" => RequestMethod.GET,
                    "POST" => RequestMethod.POST,
                    "PUT" => RequestMethod.PUT,
                    "DELETE" => RequestMethod.DELETE,
                    _ => RequestMethod.GET
                },
                Path = req.Url!.AbsolutePath + req.Url!.Query,
                Headers = req.Headers.AllKeys.ToDictionary(k => k!, k => req.Headers[k]!),
                Query = queryDict,
                Body = body
            };
        }
    }

    public static class HttpResponseAdapter
    {
        static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);

        public static async Task WriteJson(HttpListenerResponse res, ResponseDTO dto)
        {
            WriteCors(res);

            res.ContentType = "application/json; charset=utf-8";
            res.StatusCode = dto.StatusCode == 0 ? 200 : dto.StatusCode;

            var payload = dto.Data is null ? "" : JsonSerializer.Serialize(dto.Data, JsonOpts);
            using var sw = new StreamWriter(res.OutputStream);
            await sw.WriteAsync(payload);
        }

        public static async Task WriteProblem(HttpListenerResponse res, int status, string title, string detail)
        {
            WriteCors(res);
            res.ContentType = "application/json; charset=utf-8";
            res.StatusCode = status;

            var problem = new { title, detail, status };
            var payload = JsonSerializer.Serialize(problem, JsonOpts);
            using var sw = new StreamWriter(res.OutputStream);
            await sw.WriteAsync(payload);
        }

        public static void WriteCors(HttpListenerResponse res)
        {
            res.Headers["Access-Control-Allow-Origin"] = "*";
            res.Headers["Access-Control-Allow-Headers"] = "Content-Type, Authorization";
            res.Headers["Access-Control-Allow-Methods"] = "GET,POST,PUT,DELETE,OPTIONS";
        }
    }
}
