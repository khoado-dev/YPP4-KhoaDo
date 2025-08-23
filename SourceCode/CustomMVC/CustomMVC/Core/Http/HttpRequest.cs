using System.Net;
using System.Web;

namespace CustomMVC.Core.Http
{
    public sealed class HttpRequest
    {
        public HttpMethod Method { get; }
        public string Path { get; }
        public Dictionary<string, string> Query { get; } //store query after ? in path
        public Stream Body { get; } // handle large request bodies

        public HttpRequest(HttpListenerRequest raw)
        {
            Method = Enum.Parse<HttpMethod>(raw.HttpMethod, true); //true for ignore case, "get" and "GET" are the same
            Path = raw.Url?.AbsolutePath ?? "/"; //absolute path is the path not including the query string
            Body = raw.InputStream;

            var dict = HttpUtility.ParseQueryString(raw.Url?.Query ?? ""); // Parse the query string into a NameValueCollection
            Query = dict.AllKeys?.Where(k => k != null) //filter not null keys
                     .ToDictionary(k => k!, k => dict[k] ?? "", StringComparer.OrdinalIgnoreCase) //convert to Dictionary with case-INsensitive keys
                     ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
    }
}
