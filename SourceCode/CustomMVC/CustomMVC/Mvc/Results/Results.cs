using CustomMVC.Core.Http;

namespace CustomMVC.Mvc.Results
{
    public interface IActionResult
    {
        Task ExecuteAsync(HttpContext ctx);
    }
}
