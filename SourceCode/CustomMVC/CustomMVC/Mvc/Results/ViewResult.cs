using CustomMVC.Core.Http;
using CustomMVC.Mvc.Views;

namespace CustomMVC.Mvc.Results
{
    public sealed class ViewResult : IActionResult
    {
        private readonly string _viewName;
        private readonly object? _model;

        // Use SimpleViewEngine Default
        private static readonly IViewEngine _engine = new SimpleViewEngine();

        public ViewResult(string viewName, object? model)
        {
            _viewName = viewName;   // example "Users/Profile"
            _model = model;
        }

        public async Task ExecuteAsync(HttpContext ctx)
        {
            var html = _engine.Render(_viewName, _model);
            ctx.Response.StatusCode = 200;
            await ctx.Response.WriteAsync(html, "text/html; charset=utf-8");
        }
    }
}
