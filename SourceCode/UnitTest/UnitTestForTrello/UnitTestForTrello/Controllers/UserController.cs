using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Services.IServices;

namespace UnitTestForTrello.Controllers
{
    public class UserController
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }
        public UserDTO? GetUserByEmail(string email)
        {
            return _userService.GetUserByEmail(email);
        }

    }
}
