using Dapper;
using System.Data;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories.IRepositories;

namespace UnitTestForTrello.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _con;
        private readonly IDbTransaction _tran;

        public UserRepository(IDbConnection con, IDbTransaction tran)
        {
            _con = con;
            _tran = tran;
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
            FROM [User]
            WHERE Email = @Email;
            ";
            return _con.QueryFirstOrDefault<UserDTO>(sql, new { Email = email }, _tran);
        }
    }
}
