using Dapper;
using System.Data;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Tests.Utility;

namespace UnitTestForTrello.Repositories
{
    public class CollectionRepository
    {
        private readonly IDbConnection _con;
        private readonly ICustomCache _cache;

        public CollectionRepository()
        {
            _con = TestStartup.Conn!;
            _cache = TestStartup.Cache;
        }
        public IEnumerable<BoardWithCollectionDTO>? GetBoardsWithCollectionsInWorkspace(int workspaceId)
        {
            var cacheKey = $"worksapce:{workspaceId}:boardsInCollection";
            const string sql = @"
            SELECT 
              bo.Id BoardId, 
              bo.BoardName BoardName, 
              bo.BackgroundUrl BoardBackgroundImage, 
              co.Id CollectionId, 
              co.CollectionName, 
              bo.WorkspaceId WorkspaceId 
            FROM 
              Board bo 
              JOIN BoardCollection bc ON bc.BoardId = bo.Id 
              JOIN Collections co ON co.Id = bc.CollectionId 
            WHERE 
              bo.WorkspaceId = @WorkspaceId
              AND co.WorkspaceId = bo.WorkspaceId 
            ORDER BY 
              bo.CreatedAt
            ";

            var data = _con.Query<BoardWithCollectionDTO>(sql, new
            {
                WorkspaceId = workspaceId
            });
            int cacheDurationDay = 5;
            _cache.Set(cacheKey, data, TimeSpan.FromDays(cacheDurationDay));
            return data;
        }

        public IEnumerable<CollectionDTO>? GetCollectionsByWorkspace(int workspaceId)
        {
            var cacheKey = $"worksapce:{workspaceId}:collection";
            const string sql = @"
            SELECT 
              clt.Id CollectionId, 
              clt.CollectionName, 
              clt.CreatedAt, 
              clt.WorkspaceId 
            FROM 
              Collections clt 
            WHERE 
              WorkspaceId = @WorkspaceId 
            ORDER BY 
              clt.CreatedAt
            ";

            var data = _con.Query<CollectionDTO>(sql, new
            {
                WorkspaceId = workspaceId
            });
            int cacheDurationDay = 15;
            _cache.Set(cacheKey, data, TimeSpan.FromDays(cacheDurationDay));
            return data;
        }
    }
}