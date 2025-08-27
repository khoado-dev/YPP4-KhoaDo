using CustomMVC.Core.Http;

namespace CustomMVC.Core.Mvc.Results
{
    public interface IActionResult
    {
        Task ExecuteAsync(HttpContext ctx);
    }
}
