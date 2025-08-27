using CustomMVC.App.Models;
using CustomMVC.App.Repositories.IRepository;

namespace CustomMVC.App.Repositories
{
    public class UserRepository : IUserRepository
    {
        // Seed data at repository
        private readonly List<UserDTO> _users = new()
        {
            new UserDTO { Id = 1, Name = "Alice",   Email = "alice@example.com" },
            new UserDTO { Id = 2, Name = "Bob",     Email = "bob@example.com" },
            new UserDTO { Id = 3, Name = "Charlie", Email = "charlie@example.com" }
        };

        public IEnumerable<UserDTO> GetAll() => _users;

        public UserDTO? GetByEmail(string email) =>
            _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        public UserDTO? GetById(int id) =>
            _users.FirstOrDefault(u => u.Id == id);
    }
}
