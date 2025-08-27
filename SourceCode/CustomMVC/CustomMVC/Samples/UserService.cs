
namespace CustomMVC.Samples
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        // Inject repository qua constructor
        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<UserDTO> GetAllUsers() => _repo.GetAll();
        public UserDTO? GetUserByEmail(string email) => _repo.GetByEmail(email);
        public UserDTO? GetUserById(int id) => _repo.GetById(id);
    }
}
