using CustomMVC.Core.Http;

namespace CustomMVC.Mvc.Results
{
    public sealed class ContentResult : IActionResult
    {
        private readonly string _text;
        private readonly int _status;
        private readonly string _contentType;

        public ContentResult(string text, int status = 200, string contentType = "text/plain; charset=utf-8")
        {
            _text = text;
            _status = status;
            _contentType = contentType;
        }

        public async Task ExecuteAsync(HttpContext ctx)
        {
            ctx.Response.StatusCode = _status;
            await ctx.Response.WriteAsync(_text, _contentType);
        }
    }
}
