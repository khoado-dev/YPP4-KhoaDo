using Dapper;
using System.Data;
using UnitTestForTrello.Models;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories.IRepositories;

namespace UnitTestForTrello.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly IDbConnection _con;

        public MemberRepository(IDbConnection con)
        {
            _con = con;
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
            return _con.Query<BoardMemberDTO>(sql, new { BoardId = boardId });
        }

        public IEnumerable<CardMemberDTO> GetMembersByCardId(int cardId)
        {
            const string sql = @"
            SELECT 
                usr.Id UserId,
                usr.PictureUrl UserPicture,
                crd.Id CardId
            FROM Cards crd
            JOIN Members mmb ON mmb.OwnerId = crd.Id
            JOIN OwnerType owt ON owt.Id = mmb.OwnerTypeId
            JOIN [User] usr ON usr.Id = mmb.UserId
            WHERE owt.OwnerTypeValue = 'CARD' AND crd.Id = @CardId;
            ";
            return _con.Query<CardMemberDTO>(sql, new { CardId = cardId });
        }
    }
}
