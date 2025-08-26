using CustomMVC.Mvc;
using CustomMVC.Mvc.Results;

namespace CustomMVC.Samples
{
    public sealed class UsersController : ControllerBase
    {
        private readonly List<UserDTO> _users;
        public UsersController()
        {
            // Seed some users
            _users = new List<UserDTO>
            {
                new UserDTO { Id = 1, Name = "Alice", Email = "alice@example.com" },
                new UserDTO { Id = 2, Name = "Bob", Email = "bob@example.com" },
                new UserDTO { Id = 3, Name = "Charlie", Email = "charlie@example.com" }
            };
        }

        public IActionResult GetUserByEmail(string email)
        {
            var user = _users.FirstOrDefault(u =>
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return user != null ? Json(user) : NotFound($"User with email {email} was not found!");
        }

        public IActionResult GetUsers()
        {
            return Json(_users);
        }
    }
}
