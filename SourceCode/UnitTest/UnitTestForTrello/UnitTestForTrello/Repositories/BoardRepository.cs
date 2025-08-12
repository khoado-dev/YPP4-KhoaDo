using Dapper;
using System.Data;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories.IRepositories;

namespace UnitTestForTrello.Repositories
{
    public class BoardRepository : IBoardRepository
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
                brd.Id BoardId,
                brd.BackgroundUrl,
                brd.BoardName,
                brd.BoardStatus,
                usb.StarredBoardsStatus,
                usb.CreatedAt
            FROM UserStarredBoard usb
            JOIN Board brd ON brd.Id = usb.BoardId
            WHERE UserId = @UserId AND brd.BoardStatus = 'active' AND usb.StarredBoardsStatus = 1
            ORDER BY usb.CreatedAt DESC";

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
            WHERE uvh.UserId = @UserId AND owt.OwnerTypeValue = 'BOARD' AND brd.BoardStatus = 'active'
            ORDER BY uvh.AccessedAt DESC;";

            return _con.Query<RecentlyBoardDTO>(sql, new { UserId = userId }, _tran);
        }

        public IEnumerable<BoardWithWorkspaceDTO> GetBoardsWithWorkspaceByUser(int userId)
        {
            const string sql = @"
            SELECT 
                brd.Id BoardId,
                brd.BoardName AS BoardName, 
                brd.BackgroundUrl AS BoardBackground,
                wo.WorkspaceName AS WorkspaceName,
                brd.CreatedAt
            FROM Board brd
            JOIN Members me ON me.OwnerId = brd.Id
            JOIN Workspace wo ON wo.Id = brd.WorkspaceId
            JOIN OwnerType owt ON owt.Id = me.OwnerTypeId
            WHERE me.UserId = @UserId AND owt.OwnerTypeValue = 'BOARD'
            ORDER BY brd.CreatedAt;";

            return _con.Query<BoardWithWorkspaceDTO>(sql, new { UserId = userId }, _tran);
        }
    }
}
