namespace CustomMVC.Samples
{
    public interface IUserRepository
    {
        IEnumerable<UserDTO> GetAll();
        UserDTO? GetByEmail(string email);
        UserDTO? GetById(int id);
    }
}