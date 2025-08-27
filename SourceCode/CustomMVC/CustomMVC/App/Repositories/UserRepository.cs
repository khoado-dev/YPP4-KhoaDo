using CustomMVC.App.Data;
using CustomMVC.App.Models;
using CustomMVC.App.Repositories.IRepository;
using Dapper;

namespace CustomMVC.App.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _db;

        public UserRepository(IDbConnection db)
        {
            _db = db;
        }

        public IEnumerable<UserDTO> GetAll() {
            const string sql = @"
            SELECT 
              Id, 
              Name,
              Email
            FROM 
              Users;
            ";
            using var conn = _db.Open();
            return conn.Query<UserDTO>(sql);
        }

        public UserDTO? GetByEmail(string email)
        {
            const string sql = @"
            SELECT 
              Id, 
              Name, 
              Email
            FROM 
              Users 
            WHERE 
              Email = @Email;
            ";
            using var conn = _db.Open();
            return conn.QueryFirstOrDefault<UserDTO>(sql, new { Email = email });
        }
        public UserDTO? GetById(int id) {
            const string sql = @"
            SELECT 
              Id, 
              Name, 
              Email
            FROM 
              Users 
            WHERE 
              Id = @UserId;
            ";
            using var conn = _db.Open();
            return conn.QueryFirstOrDefault<UserDTO>(sql, new { UserId = id });
        }
    }
}
