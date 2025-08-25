using System.Text.Json;
using CustomMVC.Core.Http;

namespace CustomMVC.Mvc.Results
{
    public sealed class JsonResult : IActionResult
    {
        private readonly object _obj;
        private readonly int _status;

        public JsonResult(object obj, int status = 200)
        {
            _obj = obj;
            _status = status;
        }

        public async Task ExecuteAsync(HttpContext ctx)
        {
            ctx.Response.StatusCode = _status;
            var json = JsonSerializer.Serialize(_obj);
            await ctx.Response.WriteAsync(json, "application/json; charset=utf-8");
        }
    }
}
