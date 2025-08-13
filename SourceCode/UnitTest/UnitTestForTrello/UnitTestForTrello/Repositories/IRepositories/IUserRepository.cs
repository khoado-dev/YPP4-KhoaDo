using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Repositories.IRepositories
{
    public interface IUserRepository
    {
        public UserDTO? GetUserByEmail(string email);
    }
}
