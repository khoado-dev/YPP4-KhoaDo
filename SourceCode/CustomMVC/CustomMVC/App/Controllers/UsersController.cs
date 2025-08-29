using CustomMVC.App.Models;
using CustomMVC.App.Service.IService;
using CustomMVC.Core.Mvc;
using CustomMVC.Core.Mvc.Results;
using CustomMVC.Core.Routing;

namespace CustomMVC.App.Controllers
{
    [Route("{controller}")]
    public sealed class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("")]
        public IActionResult GetUsers()
        {
            IEnumerable<UserDTO> users = _userService.GetAllUsers();
            return View(new
            {
                Users = users,
            },
            "ListUsers"
            );
        }
        public IActionResult GetUserByEmail(string email)
        {
            var user = _userService.GetUserByEmail(email);

            return user != null ? Json(user) : NotFound($"User with email {email} was not found!");
        }

        [HttpGet("{id}")]
        public IActionResult GetUserDetail(int id)
        {
            var user = _userService.GetUserById(id);

            return user != null ? View(user, "Details") : NotFound("User was not exist!"); // will find Views/Users/Profile.html
        }


    }
}
