using System.Data.SqlClient;
using UnitTestForTrello.Models;

namespace UnitTestForTrello.Repositories
{
    public class WorkspaceRepository : RepositoryBase<Workspace>
    {
        public WorkspaceRepository(SqlConnection con, SqlTransaction tran) : base(con, tran) { }

        public override int Create(Workspace entity)
        {
            using var cmd = new SqlCommand(@"
                    INSERT INTO Workspaces 
                        (WorkspaceName, WorkspaceDescription, CategoryId, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, LogoUrl)
                    VALUES 
                        (@WorkspaceName, @WorkspaceDescription, @CategoryId, @CreatedAt, @CreatedBy, @UpdatedAt, @UpdatedBy, @LogoUrl);
                    SELECT SCOPE_IDENTITY();", _con, _tran);

            cmd.Parameters.AddWithValue("@WorkspaceName", (object?)entity.WorkspaceName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@WorkspaceDescription", (object?)entity.WorkspaceDescription ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CategoryId", (object?)entity.CategoryId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedAt", (object?)entity.CreatedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedBy", (object?)entity.CreatedBy ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdatedAt", (object?)entity.UpdatedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdatedBy", (object?)entity.UpdatedBy ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@LogoUrl", (object?)entity.LogoUrl ?? DBNull.Value);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public override Workspace? GetById(int id)
        {
            using var cmd = new SqlCommand("SELECT * FROM Workspaces WHERE Id = @Id", _con, _tran);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapReaderToEntity(reader);
            }
            return null;
        }

        public override List<Workspace> GetAll()
        {
            var workspaces = new List<Workspace>();
            using var cmd = new SqlCommand("SELECT * FROM Workspaces", _con, _tran);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                workspaces.Add(MapReaderToEntity(reader));
            }
            return workspaces;
        }

        public override bool Update(Workspace entity)
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
                    WHERE Id = @Id", _con, _tran);

            cmd.Parameters.AddWithValue("@Id", entity.Id);
            cmd.Parameters.AddWithValue("@WorkspaceName", (object?)entity.WorkspaceName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@WorkspaceDescription", (object?)entity.WorkspaceDescription ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CategoryId", (object?)entity.CategoryId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedAt", (object?)entity.CreatedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedBy", (object?)entity.CreatedBy ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdatedAt", (object?)entity.UpdatedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdatedBy", (object?)entity.UpdatedBy ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@LogoUrl", (object?)entity.LogoUrl ?? DBNull.Value);

            return cmd.ExecuteNonQuery() > 0;
        }

        public override bool Delete(int id)
        {
            using var cmd = new SqlCommand("DELETE FROM Workspaces WHERE Id = @Id", _con, _tran);
            cmd.Parameters.AddWithValue("@Id", id);

            return cmd.ExecuteNonQuery() > 0;
        }

        protected override Workspace MapReaderToEntity(SqlDataReader reader)
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
