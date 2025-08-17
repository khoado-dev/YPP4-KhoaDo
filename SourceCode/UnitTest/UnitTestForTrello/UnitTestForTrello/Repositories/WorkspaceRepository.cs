using Dapper;
using System.Data;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories.IRepositories;
using UnitTestForTrello.Tests.Utility;

namespace UnitTestForTrello.Tests
{
    public class WorkspaceRepository : IWorkspaceRepository
    {
        private readonly IDbConnection _con;
        private readonly ICustomCache _cache;
        public WorkspaceRepository(IDbConnection con, ICustomCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public WorkspaceDetailDTO? GetWorkspaceDetailById(int workspaceId)
        {
            const string sql = @"
            SELECT 
                Id WorkspaceId,
                LogoUrl,
                WorkspaceName,
                ShortName,
                Website,
                WorkspaceDescription
            FROM Workspace
            WHERE Id = @WorkspaceId";

            return _con.QueryFirstOrDefault<WorkspaceDetailDTO>(sql, new { WorkspaceId = workspaceId });
        }

        public IEnumerable<WorkspaceMemberDTO> GetWorkspacesByUserId(int userId)
        {
            const string sql = @"
            SELECT 
                wsp.Id WorkspaceId,
                wsp.WorkspaceName, 
                wsp.LogoUrl,
                me.UserId,
                wsp.CreatedAt
            FROM Workspace wsp
            JOIN Members me ON me.OwnerId = wsp.Id
            JOIN OwnerType owt ON owt.Id = me.OwnerTypeId
            WHERE owt.OwnerTypeValue = 'WORKSPACE' AND me.UserId = @UserId
            ORDER BY wsp.CreatedAt;";

            return _con.Query<WorkspaceMemberDTO>(sql, new { UserId = userId });
        }

        public IEnumerable<WorkspaceTypeDTO> GetWorkspaceTypes()
        {
            var cacheKey = $"workspacetypes";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<WorkspaceTypeDTO>? cached))
                return cached!;
            const string sql = @"
            SELECT
                Id WorkspaceTypeId,
                TypeValue,
                DisplayValue
            FROM 
                WorkspaceType;";

            var data = _con.Query<WorkspaceTypeDTO>(sql, null);
            _cache.Set(cacheKey, data, TimeSpan.FromMinutes(5));
            return data;
        }
    }
}