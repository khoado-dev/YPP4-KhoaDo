using CustomMVC.Mvc;
using CustomMVC.Mvc.Results;

namespace CustomMVC.Samples
{
    public sealed class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult GetUserByEmail(string email)
        {
            var user = _userService.GetUserByEmail(email); //call api here next time

            return user != null ? Json(user) : NotFound($"User with email {email} was not found!");
        }
        
        public IActionResult GetUsers()
        {
            return Json(_userService);
        }
        public IActionResult Profile(int id)
        {
            var user = _userService.GetUserById(id); //call api here next time

            return user != null ? View(user) : NotFound("User was not exist!"); // will find Views/Users/Profile.html
        }


    }
}
