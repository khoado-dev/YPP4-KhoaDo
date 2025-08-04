using System.Data.SqlClient;
using UnitTestForTrello.Models;

namespace UnitTestForTrello.Repositories
{
    public class WorkspaceRepository
    {
        private readonly SqlConnection _con;
        private readonly SqlTransaction _tran;

        public WorkspaceRepository(SqlConnection con, SqlTransaction tran)
        {
            _con = con;
            _tran = tran;
        }

        public int CreateWorkspace(Workspace workspace)
        {
            using var cmd = new SqlCommand(@"
                    INSERT INTO Workspaces 
                        (WorkspaceName, WorkspaceDescription, CategoryId, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, LogoUrl)
                    VALUES 
                        (@WorkspaceName, @WorkspaceDescription, @CategoryId, @CreatedAt, @CreatedBy, @UpdatedAt, @UpdatedBy, @LogoUrl);
                    SELECT SCOPE_IDENTITY();",_con, _tran);

            cmd.Parameters.AddWithValue("@WorkspaceName", (object?)workspace.WorkspaceName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@WorkspaceDescription", (object?)workspace.WorkspaceDescription ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CategoryId", (object?)workspace.CategoryId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedAt", (object?)workspace.CreatedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedBy", (object?)workspace.CreatedBy ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdatedAt", (object?)workspace.UpdatedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdatedBy", (object?)workspace.UpdatedBy ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@LogoUrl", (object?)workspace.LogoUrl ?? DBNull.Value);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public Workspace? GetWorkspaceById(int id)
        {
            using var cmd = new SqlCommand("SELECT * FROM Workspaces WHERE Id = @Id",_con, _tran);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapReaderToWorkspace(reader);
            }
            return null;
        }

        public List<Workspace> GetAllWorkspaces()
        {
            var workspaces = new List<Workspace>();
            using var cmd = new SqlCommand("SELECT * FROM Workspaces",_con, _tran);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                workspaces.Add(MapReaderToWorkspace(reader));
            }
            return workspaces;
        }

        public bool UpdateWorkspace(Workspace workspace)
        {
            using var cmd = new SqlCommand(@"
                    UPDATE Workspaces SET
                        WorkspaceName = @WorkspaceName,
                        WorkspaceDescription = @WorkspaceDescription,
                        CategoryId = @CategoryId,
                        CreatedAt = @CreatedAt,
                        CreatedBy = @CreatedBy,
                        UpdatedAt = @UpdatedAt,
                        UpdatedBy = @UpdatedBy,
                        LogoUrl = @LogoUrl
                    WHERE Id = @Id",_con, _tran);

            cmd.Parameters.AddWithValue("@Id", workspace.Id);
            cmd.Parameters.AddWithValue("@WorkspaceName", (object?)workspace.WorkspaceName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@WorkspaceDescription", (object?)workspace.WorkspaceDescription ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CategoryId", (object?)workspace.CategoryId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedAt", (object?)workspace.CreatedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedBy", (object?)workspace.CreatedBy ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdatedAt", (object?)workspace.UpdatedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdatedBy", (object?)workspace.UpdatedBy ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@LogoUrl", (object?)workspace.LogoUrl ?? DBNull.Value);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool DeleteWorkspace(int id)
        {
            using var cmd = new SqlCommand("DELETE FROM Workspaces WHERE Id = @Id",_con, _tran);
            cmd.Parameters.AddWithValue("@Id", id);

            return cmd.ExecuteNonQuery() > 0;
        }

        private Workspace MapReaderToWorkspace(SqlDataReader reader)
        {
            return new Workspace
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                WorkspaceName = reader["WorkspaceName"] as string,
                WorkspaceDescription = reader["WorkspaceDescription"] as string,
                CategoryId = reader["CategoryId"] as int?,
                CreatedAt = reader["CreatedAt"] as DateTime?,
                CreatedBy = reader["CreatedBy"] as int?,
                UpdatedAt = reader["UpdatedAt"] as DateTime?,
                UpdatedBy = reader["UpdatedBy"] as int?,
                LogoUrl = reader["LogoUrl"] as string
            };
        }
    }
}
