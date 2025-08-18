using Dapper;
using System.Data;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories.IRepositories;
using UnitTestForTrello.Tests.Utility;

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
            FROM 
              Members mmb 
              JOIN OwnerType owt ON owt.Id = mmb.OwnerTypeId 
              JOIN Users usr ON usr.Id = mmb.UserId 
            WHERE 
              owt.OwnerTypeValue = @OwnerType 
              AND mmb.OwnerId = @BoardId;

            ";
            return _con.Query<BoardMemberDTO>(sql, new 
            { 
                BoardId = boardId,
                OwnerType = OwnerType.BOARD.ToString()
            });
        }

        public IEnumerable<CardMemberDTO> GetMembersByCardId(int cardId)
        {
            const string sql = @"
            SELECT 
              usr.Id UserId, 
              usr.PictureUrl UserPicture, 
              crd.Id CardId 
            FROM 
              Cards crd 
              JOIN Members mmb ON mmb.OwnerId = crd.Id 
              JOIN OwnerType owt ON owt.Id = mmb.OwnerTypeId 
              JOIN Users usr ON usr.Id = mmb.UserId 
            WHERE 
              owt.OwnerTypeValue = @OwnerType
              AND crd.Id = @CardId;

            ";
            return _con.Query<CardMemberDTO>(sql, new 
            { 
                CardId = cardId,
                OwnerType = OwnerType.CARD.ToString()
            });
        }

        public IEnumerable<WorkspaceMemberDTO> GetMembersByWorkspaceId(int workspaceId)
        {
            const string sql = @"
            WITH BoardCountByEachUser AS (
                SELECT 
                mmb.UserId, 
                COUNT(*) AS BoardCount 
                FROM 
                Members mmb 
                JOIN OwnerType owt ON owt.Id = mmb.OwnerTypeId 
                JOIN Board brd ON brd.Id = mmb.OwnerId 
                WHERE 
                owt.OwnerTypeValue = @BoardType 
                AND brd.WorkspaceId = @WorkspaceId 
                GROUP BY 
                mmb.UserId
            ) 
            SELECT 
                us.PictureUrl AS UserPicture, 
                us.Username, 
                us.Email AS UserEmail, 
                us.LastActive AS UserLastActive, 
                pe.PermissionName, 
                COALESCE(bcb.BoardCount, 0) AS BoardCount 
            FROM 
                Members mmb 
                JOIN OwnerType owt ON owt.Id = mmb.OwnerTypeId 
                JOIN RolePermission pe ON pe.Id = mmb.RolePermissonId 
                JOIN [Users] us ON us.Id = mmb.UserId 
                LEFT JOIN BoardCountByEachUser bcb ON bcb.UserId = mmb.UserId 
            WHERE 
                owt.OwnerTypeValue = @WorkspaceType 
                AND mmb.OwnerId = @WorkspaceId 
            ORDER BY 
                mmb.JoinedAt;
            ";
            return _con.Query<WorkspaceMemberDTO>(sql, new 
            { 
                WorkspaceId = workspaceId,
                BoardType = OwnerType.BOARD.ToString(),
                WorkspaceType = OwnerType.WORKSPACE.ToString(),
            });
        }

        public IEnumerable<RolePermissionDTO> GetRolePermissions()
        {
            const string sql = @"
            SELECT 
              Id AS RolePermissionId, 
              PermissionName, 
              PermissionCode 
            FROM 
              RolePermission;

            ";

            return _con.Query<RolePermissionDTO>(sql);
        }

        public IEnumerable<CardSelectableMemberDTO> GetSelectableMembersByCardId(int cardId)
        {
            const string sql = @"
            WITH CardWithBoardWorkspace AS (
              SELECT 
                crd.Id CardId, 
                crd.Title CardTitle, 
                brd.Id BoardId, 
                brd.BoardName, 
                wsp.Id WorkspaceId, 
                wsp.WorkspaceName 
              FROM 
                Cards crd 
                JOIN Stage stg ON stg.Id = crd.StageId 
                JOIN Board brd ON brd.Id = stg.BoardId 
                JOIN Workspace wsp ON wsp.Id = brd.WorkspaceId 
              WHERE 
                crd.Id = @CardId
            ) 
            SELECT 
              usr.Id UserId, 
              usr.PictureUrl UserPicture, 
              usr.Username, 
              OwnerTypeValue, 
              JoinedAt 
            FROM 
              Members mmb 
              JOIN OwnerType owt ON owt.Id = mmb.OwnerTypeId 
              JOIN CardWithBoardWorkspace cwbw ON (
                OwnerTypeValue = @CardType 
                AND cwbw.CardId = mmb.OwnerId
              ) 
              OR (
                OwnerTypeValue = @BoardType 
                AND cwbw.BoardId = mmb.OwnerId
              ) 
              OR (
                OwnerTypeValue = @WorkspaceType
                AND cwbw.WorkspaceId = mmb.OwnerId
              ) 
              JOIN Users usr ON usr.Id = mmb.UserId 
            ORDER BY 
              owt.Id DESC, 
              JoinedAt DESC;

            ";
            return _con.Query<CardSelectableMemberDTO>(sql, new 
            { 
                CardId = cardId,
                CardType = OwnerType.CARD.ToString(),
                BoardType = OwnerType.BOARD.ToString(),
                WorkspaceType = OwnerType.WORKSPACE.ToString(),
            });
        }
    }
}
