namespace UnitTestForTrello.Models
{
    public sealed class Response
    {
        public HttpStatus StatusCode { set; get; }
        public object? Body { set; get; }
        public bool IsSuccess => (int)StatusCode >= 200 && (int)StatusCode < 300;
    }
    public enum HttpStatus
    {
        // 1XX - Informational
        Continue = 100,
        SwitchingProtocols = 101,

        // 2XX - Success
        OK = 200,
        Created = 201,
        NoContent = 204,

        // 3XX - Redirection
        MovedPermanently = 301,
        Found = 302,
        NotModified = 304,

        // 4XX - Client Error
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        TooManyRequests = 429,

        // 5XX - Server Error
        InternalServerError = 500,
        NotImplemented = 501,
        BadGateway = 502,
        ServiceUnavailable = 503,
        GatewayTimeout = 504
    }
}
