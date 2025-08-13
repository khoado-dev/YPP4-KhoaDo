using Dapper;
using System.Data;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories.IRepositories;

namespace UnitTestForTrello.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly IDbConnection _con;
        private readonly IDbTransaction _tran;

        public MemberRepository(IDbConnection con, IDbTransaction tran)
        {
            _con = con;
            _tran = tran;
        }

        public IEnumerable<BoardMemberDTO> GetMembersByBoardId(int boardId)
        {
            const string sql = @"
            SELECT 
                usr.Id UserId,
                usr.PictureUrl UserPicture,
                owt.OwnerTypeValue,
                mmb.OwnerId BoardId
            FROM Members mmb
            JOIN OwnerType owt ON owt.Id = mmb.OwnerTypeId
            JOIN [User] usr ON usr.Id = mmb.UserId
            WHERE owt.OwnerTypeValue = 'BOARD' AND mmb.OwnerId = @BoardId;
            ";
            return _con.Query<BoardMemberDTO>(sql, new { BoardId = boardId }, _tran);
        }
    }
}
