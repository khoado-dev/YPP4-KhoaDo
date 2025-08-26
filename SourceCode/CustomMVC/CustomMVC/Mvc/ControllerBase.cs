using CustomMVC.Core.Http;
using CustomMVC.Mvc.Results;

namespace CustomMVC.Mvc
{
    public abstract class ControllerBase
    {
        public HttpContext HttpContext { get; internal set; } = default!;

        // Helpers
        protected IActionResult Ok(string text) =>
            new ContentResult(text, 200, "text/plain; charset=utf-8");

        protected IActionResult Json(object obj, int status = 200) =>
            new JsonResult(obj, status);

        protected IActionResult NotFound(string? message = null) =>
            new ContentResult(message ?? "Not Found", 404, "text/plain; charset=utf-8");
        protected IActionResult StatusCode(int status, string? message = null) =>
            new StatusCodeResult(status, message);

        protected IActionResult Redirect(string url, bool permanent = false) =>
            new RedirectResult(url, permanent);

        protected IActionResult File(byte[] bytes, string contentType, string? downloadFileName = null) =>
            new FileResult(bytes, contentType, downloadFileName);

    }
}
