using CustomMVC.Core.Http;

namespace CustomMVC.Mvc.Results
{
    public sealed class StatusCodeResult : IActionResult
    {
        private readonly int _status;
        private readonly string? _message;
        private readonly string _contentType;

        public StatusCodeResult(int status, string? message = null, string contentType = "text/plain; charset=utf-8")
        {
            _status = status;
            _message = message;
            _contentType = contentType;
        }

        public async Task ExecuteAsync(HttpContext ctx)
        {
            ctx.Response.StatusCode = _status;

            await ctx.Response.WriteAsync(
                string.IsNullOrEmpty(_message) ? string.Empty : _message!,
                _contentType
            );
        }
    }
}
