using Dapper;
using System.Data;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories.IRepositories;

namespace UnitTestForTrello.Repositories
{
    public class BoardRepository : IBoardRepository
    {
        private readonly IDbConnection _con;

        public BoardRepository(IDbConnection con)
        {
            _con = con;
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

            return _con.Query<StarredBoardDTO>(sql, new { UserId = userId });
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

            return _con.Query<RecentlyBoardDTO>(sql, new { UserId = userId });
        }

        public IEnumerable<BoardWithWorkspaceDTO> GetBoardsWhereUserIsMemberInWorkspace(int userId, int workspaceId)
        {
            const string sql = @"
            SELECT 
                brd.Id BoardId,
                brd.BoardName AS BoardName, 
                brd.BackgroundUrl AS BoardBackground,
                wo.WorkspaceName AS WorkspaceName,
                wo.Id WorkspaceId,
                brd.CreatedAt
            FROM Board brd
            JOIN Members me ON me.OwnerId = brd.Id
            JOIN Workspace wo ON wo.Id = brd.WorkspaceId
            JOIN OwnerType owt ON owt.Id = me.OwnerTypeId
            WHERE me.UserId = @UserId AND owt.OwnerTypeValue = 'BOARD' AND wo.Id = @WorkspaceId
            ORDER BY brd.CreatedAt;";

            return _con.Query<BoardWithWorkspaceDTO>(sql, new { UserId = userId, WorkspaceId = workspaceId });
        }

        public IEnumerable<BoardWithWorkspaceDTO> GetBoardsWhereUserIsOwnerInWorkspace(int userId, int workspaceId)
        {
            const string sql = @"
            SELECT 
                brd.Id BoardId,
                brd.BoardName AS BoardName, 
                brd.BackgroundUrl AS BoardBackground,
                wo.Id WorkspaceId,
                wo.WorkspaceName AS WorkspaceName,
                brd.CreatedBy,
                brd.CreatedAt
            FROM Board brd
            JOIN Members me ON me.OwnerId = brd.Id
            JOIN Workspace wo ON wo.Id = brd.WorkspaceId
            JOIN OwnerType owt ON owt.Id = me.OwnerTypeId
            WHERE me.UserId = @UserId  AND brd.CreatedBy = me.UserId AND owt.OwnerTypeValue = 'BOARD' AND wo.Id = @WorkspaceId
            ORDER BY brd.CreatedAt;";

            return _con.Query<BoardWithWorkspaceDTO>(sql, new { UserId = userId, WorkspaceId = workspaceId });
        }
    }
}
