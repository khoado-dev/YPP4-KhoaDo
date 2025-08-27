using CustomMVC.App.Models;

namespace CustomMVC.App.Service.IService
{
    public interface IUserService
    {
        IEnumerable<UserDTO> GetAllUsers();
        UserDTO? GetUserByEmail(string email);
        UserDTO? GetUserById(int id);
    }
}