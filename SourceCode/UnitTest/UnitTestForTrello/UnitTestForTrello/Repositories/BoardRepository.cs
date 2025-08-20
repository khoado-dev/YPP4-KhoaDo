using Dapper;
using System.Data;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories.IRepositories;
using UnitTestForTrello.Tests.Utility;

namespace UnitTestForTrello.Repositories
{
    public class BoardRepository : IBoardRepository
    {
        private readonly IDbConnection _con;
        private readonly ICustomCache _cache;

        public BoardRepository()
        {
            _con = TestStartup.Conn!;
            _cache = TestStartup.Cache;
        }

        public IEnumerable<StarredBoardDTO> GetStarredBoards(int userId)
        {
            var cacheKey = $"user:{userId}:starredboards";
            const string sql = @"
            SELECT 
              usb.UserId, 
              brd.Id BoardId, 
              brd.BackgroundUrl, 
              brd.BoardName, 
              brd.BoardStatus, 
              usb.StarredBoardsStatus, 
              usb.CreatedAt 
            FROM 
              UserStarredBoard usb 
              JOIN Board brd ON brd.Id = usb.BoardId 
            WHERE 
              UserId = @UserId 
              AND brd.BoardStatus = @Status 
              AND usb.StarredBoardsStatus = @StarredBoardsStatus 
            ORDER BY 
              usb.CreatedAt DESC
            ";

            var data = _con.Query<StarredBoardDTO>(sql, new
            {
                UserId = userId,
                Status = BoardStatus.ACTIVE.ToString(),
                StarredBoardsStatus = StaredBoardsStatus.ACTIVE
            });
            int cacheDurationMinutes = 5;
            _cache.Set(cacheKey, data, TimeSpan.FromMinutes(cacheDurationMinutes));
            return data;
        }

        public IEnumerable<RecentBoardDTO> GetRecentBoards(int userId)
        {
            const string sql = @"
            SELECT 
              uvh.UserId, 
              brd.Id BoardId, 
              brd.BoardName, 
              brd.BackgroundUrl, 
              uvh.AccessedAt, 
              brd.BoardStatus 
            FROM 
              UserViewHistory uvh 
              JOIN Board brd ON brd.Id = uvh.OwnerId 
              JOIN OwnerType owt ON owt.Id = uvh.OwnerTypeId 
            WHERE 
              uvh.UserId = @UserId 
              AND owt.OwnerTypeValue = @OwnerType 
              AND brd.BoardStatus = @BoardStatus 
            ORDER BY 
              uvh.AccessedAt DESC;
            ";

            return _con.Query<RecentBoardDTO>(sql, new 
            {
                UserId = userId,
                OwnerType = OwnerType.BOARD.ToString(),
                BoardStatus = BoardStatus.ACTIVE.ToString()
            });
        }

        public IEnumerable<BoardWithWorkspaceDTO> GetBoardsAsMember(int userId, int workspaceId)
        {
            const string sql = @"
            SELECT 
              brd.Id BoardId, 
              brd.BoardName AS BoardName, 
              brd.BackgroundUrl AS BoardBackground, 
              wo.WorkspaceName AS WorkspaceName, 
              wo.Id WorkspaceId, 
              brd.CreatedAt 
            FROM 
              Board brd 
              JOIN Members me ON me.OwnerId = brd.Id 
              JOIN Workspace wo ON wo.Id = brd.WorkspaceId 
              JOIN OwnerType owt ON owt.Id = me.OwnerTypeId 
            WHERE 
              me.UserId = @UserId 
              AND owt.OwnerTypeValue = @OwnerType 
              AND wo.Id = @WorkspaceId 
            ORDER BY 
              brd.CreatedAt;
            ";

            return _con.Query<BoardWithWorkspaceDTO>(sql, new
            {
                UserId = userId,
                OwnerType = OwnerType.BOARD.ToString(),
                WorkspaceId = workspaceId
            });
        }

        public IEnumerable<BoardWithWorkspaceDTO> GetBoardsAsOwner(int userId, int workspaceId)
        {
            const string sql = @"
            SELECT 
              brd.Id BoardId, 
              brd.BoardName AS BoardName, 
              brd.BackgroundUrl AS BoardBackground, 
              wo.WorkspaceName AS WorkspaceName, 
              wo.Id WorkspaceId, 
              brd.CreatedAt 
            FROM 
              Board brd 
              JOIN Members me ON me.OwnerId = brd.Id 
              JOIN Workspace wo ON wo.Id = brd.WorkspaceId 
              JOIN OwnerType owt ON owt.Id = me.OwnerTypeId 
            WHERE 
              me.UserId = @UserId 
              AND brd.CreatedBy = @UserId  
              AND owt.OwnerTypeValue = @OwnerType
              AND wo.Id = @WorkspaceId 
            ORDER BY 
              brd.CreatedAt;
            ";

            return _con.Query<BoardWithWorkspaceDTO>(sql, new 
            { 
                UserId = userId,
                OwnerType = OwnerType.BOARD.ToString(),
                WorkspaceId = workspaceId 
            });
        }
    }
}
