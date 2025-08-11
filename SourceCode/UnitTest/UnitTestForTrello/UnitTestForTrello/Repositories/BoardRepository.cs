using System.Data.SqlClient;
using UnitTestForTrello.Models;

namespace UnitTestForTrello.Repositories
{
    public class BoardRepository : RepositoryBase
    {
        public BoardRepository(SqlConnection con, SqlTransaction tran) : base(con, tran) { }

        public int Create(Board entity)
        {
            using var cmd = new SqlCommand(@"
                    INSERT INTO Boards 
                        (BoardName, BoardDescription, WorkspaceId, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, BackgroundUrl)
                    VALUES 
                        (@BoardName, @BoardDescription, @WorkspaceId, @CreatedAt, @CreatedBy, @UpdatedAt, @UpdatedBy, @BackgroundUrl);
                    SELECT SCOPE_IDENTITY();", _con, _tran);

            cmd.Parameters.AddWithValue("@BoardName", (object?)entity.BoardName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@BoardDescription", (object?)entity.BoardDescription ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@WorkspaceId", (object?)entity.WorkspaceId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedAt", (object?)entity.CreatedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedBy", (object?)entity.CreatedBy ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdatedAt", (object?)entity.UpdatedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdatedBy", (object?)entity.UpdatedBy ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@BackgroundUrl", (object?)entity.BackgroundUrl ?? DBNull.Value);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public Board? GetById(int id)
        {
            using var cmd = new SqlCommand("SELECT * FROM Boards WHERE Id = @Id", _con, _tran);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapReaderToEntity(reader);
            }
            return null;
        }

        public List<Board> GetAll()
        {
            var boards = new List<Board>();
            using var cmd = new SqlCommand("SELECT * FROM Boards", _con, _tran);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                boards.Add(MapReaderToEntity(reader));
            }
            return boards;
        }

        public bool Update(Board entity)
        {
            using var cmd = new SqlCommand(@"
                    UPDATE Boards SET
                        BoardName = @BoardName,
                        BoardDescription = @BoardDescription,
                        WorkspaceId = @WorkspaceId,
                        CreatedAt = @CreatedAt,
                        CreatedBy = @CreatedBy,
                        UpdatedAt = @UpdatedAt,
                        UpdatedBy = @UpdatedBy,
                        BackgroundUrl = @BackgroundUrl
                    WHERE Id = @Id", _con, _tran);

            cmd.Parameters.AddWithValue("@Id", entity.Id);
            cmd.Parameters.AddWithValue("@BoardName", (object?)entity.BoardName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@BoardDescription", (object?)entity.BoardDescription ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@WorkspaceId", (object?)entity.WorkspaceId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedAt", (object?)entity.CreatedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedBy", (object?)entity.CreatedBy ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdatedAt", (object?)entity.UpdatedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdatedBy", (object?)entity.UpdatedBy ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@BackgroundUrl", (object?)entity.BackgroundUrl ?? DBNull.Value);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            using var cmd = new SqlCommand("DELETE FROM Boards WHERE Id = @Id", _con, _tran);
            cmd.Parameters.AddWithValue("@Id", id);

            return cmd.ExecuteNonQuery() > 0;
        }

        protected Board MapReaderToEntity(SqlDataReader reader)
        {
            return new Board
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                BoardName = reader["BoardName"] as string,
                BoardDescription = reader["BoardDescription"] as string,
                WorkspaceId = reader["WorkspaceId"] as int?,
                CreatedAt = reader["CreatedAt"] as DateTime?,
                CreatedBy = reader["CreatedBy"] as int?,
                UpdatedAt = reader["UpdatedAt"] as DateTime?,
                UpdatedBy = reader["UpdatedBy"] as int?,
                BackgroundUrl = reader["BackgroundUrl"] as string
            };
        }
    }
}
