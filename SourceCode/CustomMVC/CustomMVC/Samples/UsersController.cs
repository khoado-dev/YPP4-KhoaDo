using CustomMVC.Mvc;
using CustomMVC.Mvc.Results;

namespace CustomMVC.Samples
{
    public sealed class UsersController : ControllerBase
    {
        // GET /users/{id}
        public IActionResult Show(int id) => Json(new { id, name = $"user-{id}" });

        // GET /users/find?name=alice
        public IActionResult Find(string name) => Ok($"Looking for user: {name}");

        // GET /users/notfound
        public IActionResult NotFoundDemo() => StatusCode(404, "User not found");

        // GET /users/redirect
        public IActionResult RedirectDemo() => Redirect("/hello"); // 302 by default

        // GET /users/file
        public IActionResult FileDemo()
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes("Sample file content");
            return File(bytes, "text/plain; charset=utf-8", "sample.txt");
        }
    }
}
