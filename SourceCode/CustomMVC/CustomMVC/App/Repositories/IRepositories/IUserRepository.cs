using CustomMVC.App.Models;

namespace CustomMVC.App.Repositories.IRepository
{
    public interface IUserRepository
    {
        IEnumerable<UserDTO> GetAll();
        UserDTO? GetByEmail(string email);
        UserDTO? GetById(int id);
    }
}