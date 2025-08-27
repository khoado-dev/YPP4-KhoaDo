using CustomMVC.Core.Http;

namespace CustomMVC.Core.Mvc.Results
{
    public sealed class RedirectResult : IActionResult
    {
        private readonly string _url;
        private readonly bool _permanent; // 301 vs 302

        public RedirectResult(string url, bool permanent = false)
        {
            _url = url;
            _permanent = permanent;
        }

        public async Task ExecuteAsync(HttpContext ctx)
        {
            ctx.Response.StatusCode = _permanent ? 301 : 302;
            ctx.Response.Headers["Location"] = _url;
            await ctx.Response.WriteAsync(string.Empty); // body optional
        }
    }
}
