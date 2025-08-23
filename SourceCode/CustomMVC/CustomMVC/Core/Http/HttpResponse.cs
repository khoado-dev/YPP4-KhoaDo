using System.Net;
using System.Text;

namespace CustomMVC.Core.Http
{
    public sealed class HttpResponse
    {
        private readonly HttpListenerResponse _raw;

        public int StatusCode { get => _raw.StatusCode; set => _raw.StatusCode = value; }
        public IDictionary<string, string> Headers { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public HttpResponse(HttpListenerResponse raw) => _raw = raw;

        public async Task WriteAsync(string text, string contentType = "text/plain; charset=utf-8")
        {
            int startOffset = 0;

            _raw.ContentType = contentType;

            foreach (var kv in Headers)
            {
                _raw.Headers[kv.Key] = kv.Value;
            }

            var bytes = Encoding.UTF8.GetBytes(text);
            _raw.ContentLength64 = bytes.Length;
            await _raw.OutputStream.WriteAsync(bytes, startOffset, bytes.Length);
        }
    }
}
