using CustomMVC.Mvc;
using CustomMVC.Mvc.Results;

namespace CustomMVC.Samples
{
    public sealed class UsersController : ControllerBase
    {
        // GET /users/{id}
        public IActionResult Show(int id)
        {
            // example logic (mock)
            return Json(new { id, name = $"user-{id}" });
        }

        // GET /users/find?name=alice
        public IActionResult Find(string name)
        {
            // read from query binder set (name)
            return Ok($"Looking for user: {name}");
        }
    }
}
