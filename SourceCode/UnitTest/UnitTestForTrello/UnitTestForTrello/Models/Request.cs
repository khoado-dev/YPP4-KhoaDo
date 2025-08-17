namespace UnitTestForTrello.Models
{
    public sealed class Request
    {
        public HttpMethod Method { set; get; }
        public string Path { set; get; } = string.Empty;
        public object? Body { get; set; }
        public Dictionary<string, string> Params { get; set; } =
            new(StringComparer.OrdinalIgnoreCase);
    }
    public enum HttpMethod
    {
        GET,
        POST,
        PUT,
        DELETE,
        PATCH
    }
}