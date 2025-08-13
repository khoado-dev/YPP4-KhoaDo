using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories.IRepositories;

namespace UnitTestForTrello.Tests
{
    public class WorkspaceRepository : IWorkspaceRepository
    {
        private readonly IDbConnection _con;
        private readonly IDbTransaction _tran;
        
        public WorkspaceRepository(IDbConnection con, IDbTransaction tran)
        {
            _con = con;
            _tran = tran;
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

            return _con.Query<WorkspaceMemberDTO>(sql, new { UserId = userId }, _tran);
        }

        public IEnumerable<WorkspaceTypeDTO> GetWorkspaceTypes()
        {
            const string sql = @"
            SELECT
                Id WorkspaceTypeId,
                TypeValue,
                DisplayValue
            FROM 
                WorkspaceType;";

            return _con.Query<WorkspaceTypeDTO>(sql, null, _tran);
        }
    }
}