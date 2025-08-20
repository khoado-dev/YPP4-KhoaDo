using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories;
using UnitTestForTrello.Repositories.IRepositories;

namespace UnitTestForTrello.Services.IServices
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserDTO? GetUserByEmail(string email)
        {
            return _userRepository.GetUserByEmail(email);
        }
    }
}
