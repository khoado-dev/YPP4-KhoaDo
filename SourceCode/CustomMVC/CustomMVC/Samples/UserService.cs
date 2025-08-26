
namespace CustomMVC.Samples
{
    public class UserService : IUserService
    {
        private readonly List<UserDTO> _users;
        public UserService()
        {
            _users = new List<UserDTO>
        {
            new UserDTO { Id = 1, Name = "Alice", Email = "alice@example.com" },
            new UserDTO { Id = 2, Name = "Bob", Email = "bob@example.com" },
            new UserDTO { Id = 3, Name = "Charlie", Email = "charlie@example.com" }
        };
        }
        public IEnumerable<UserDTO> GetAllUsers()
        {
            return _users;
        }

        public UserDTO? GetUserByEmail(string email)
        {
            return _users.FirstOrDefault(u =>
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }
    }
}
