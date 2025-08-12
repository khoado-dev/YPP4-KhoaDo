using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Repositories
{
    public class BoardRepository
    {
        private readonly IDbConnection _con;
        private readonly IDbTransaction _tran;

        public BoardRepository(IDbConnection con, IDbTransaction tran)
        {
            _con = con;
            _tran = tran;
        }

        public IEnumerable<StarredBoardDTO> GetStarredBoardsByUser(int userId)
        {
            const string sql = @"
            SELECT
                usb.UserId,
                brd.Id AS BoardId,
                brd.BackgroundUrl,
                brd.BoardName,
                brd.BoardStatus
            FROM UserStarredBoard usb
            JOIN Board brd ON brd.Id = usb.BoardId
            WHERE usb.UserId = @UserId
              AND brd.BoardStatus = 'active'
            ORDER BY usb.CreatedAt DESC;";

            return _con.Query<StarredBoardDTO>(sql, new { UserId = userId }, _tran);
        }

        public IEnumerable<RecentlyBoardDTO> GetRecentlyBoardsByUser(int userId)
        {
            const string sql = @"
            SELECT 
                uvh.UserId,
                brd.Id BoardId,
                brd.BoardName, 
                brd.BackgroundUrl,
                uvh.AccessedAt,
                brd.BoardStatus
            FROM UserViewHistory uvh
            JOIN Board brd ON brd.Id = uvh.OwnerId
            JOIN OwnerType owt ON owt.Id = uvh.OwnerTypeId
            WHERE uvh.UserId = @UserId AND owt.OwnerTypeValue = 'BOARD'
            ORDER BY uvh.AccessedAt DESC;";

            return _con.Query<RecentlyBoardDTO>(sql, new { UserId = userId }, _tran);
        }
    }
}
