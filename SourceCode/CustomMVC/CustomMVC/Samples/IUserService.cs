namespace CustomMVC.Samples
{
    public interface IUserService
    {
        IEnumerable<UserDTO> GetAllUsers();
        UserDTO? GetUserByEmail(string email);
    }
}