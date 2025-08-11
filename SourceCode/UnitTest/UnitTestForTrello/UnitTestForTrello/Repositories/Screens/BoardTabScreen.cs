using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestForTrello.Models;

namespace UnitTestForTrello.Repositories.Screens
{
    public class BoardTabScreen : RepositoryBase
    {
        public BoardTabScreen(SqlConnection con, SqlTransaction tran) : base(con, tran)
        {
        }

        public List<Board> GetStarredBoardsByUser(int userId)
        {
            var boards = new List<Board>();

            using var cmd = new SqlCommand(@"
                SELECT b.Id, b.BoardName, b.BoardDescription, b.BackgroundUrl, b.WorkspaceId, 
                       b.CreatedAt, b.CreatedBy, b.UpdatedAt, b.UpdatedBy,
                       b.BoardStatus,
                       w.Id AS W_Id, w.WorkspaceName, w.WorkspaceDescription, w.LogoUrl, w.CreatedAt AS W_CreatedAt
                FROM UserStarredBoard usb
                JOIN Board b ON usb.BoardId = b.Id
                JOIN Workspace w ON b.WorkspaceId = w.Id
                WHERE usb.UserId = @UserId
                  AND usb.StarredBoardsStatus = 1
                ORDER BY usb.CreatedAt DESC;", _con, _tran);

            cmd.Parameters.AddWithValue("@UserId", userId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                boards.Add(new Board
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    BoardName = reader["BoardName"] as string,
                    BoardDescription = reader["BoardDescription"] as string,
                    BackgroundUrl = reader["BackgroundUrl"] as string,
                    WorkspaceId = reader["WorkspaceId"] as int?,
                    CreatedAt = reader["CreatedAt"] as DateTime?,
                    CreatedBy = reader["CreatedBy"] as int?,
                    UpdatedAt = reader["UpdatedAt"] as DateTime?,
                    UpdatedBy = reader["UpdatedBy"] as int?,
                    BoardStatus = reader["BoardStatus"] as string,
                    Workspace = new Workspace
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("W_Id")),
                        WorkspaceName = reader["WorkspaceName"] as string,
                        WorkspaceDescription = reader["WorkspaceDescription"] as string,
                        LogoUrl = reader["LogoUrl"] as string,
                        CreatedAt = reader["W_CreatedAt"] as DateTime?
                    }
                });
            }

            return boards;
        }
    }
}
