using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Services.IServices
{
    public interface IUserService
    {
        public UserDTO? GetUserByEmail(string email);
    }
}