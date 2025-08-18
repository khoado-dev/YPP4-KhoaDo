using Dapper;
using System.Data;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories.IRepositories;

namespace UnitTestForTrello.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _con;

        public UserRepository(IDbConnection con)
        {
            _con = con;
        }

        public UserDTO? GetUserByEmail(string email)
        {
            const string sql = @"
            SELECT 
              Id, 
              PictureUrl, 
              Email, 
              Username, 
              Bio 
            FROM 
              Users 
            WHERE 
              Email = @Email;
            ";
            return _con.QueryFirstOrDefault<UserDTO>(sql, new { Email = email });
        }
    }
}
