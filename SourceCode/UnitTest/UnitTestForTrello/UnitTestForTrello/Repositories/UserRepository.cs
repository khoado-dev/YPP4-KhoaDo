using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestForTrello.Models;

namespace UnitTestForTrello.Repositories
{
    public class UserRepository : RepositoryBase<User>
    {
        public UserRepository(SqlConnection con, SqlTransaction tran) : base(con, tran) { }

        public override int Create(User entity)
        {
            using var cmd = new SqlCommand(@"
                    INSERT INTO Users 
                        (Username, Bio, Email, LastActive, CreatedAt, UpdatedAt, PictureUrl)
                    VALUES 
                        (@Username, @Bio, @Email, @LastActive, @CreatedAt, @UpdatedAt, @PictureUrl);
                    SELECT SCOPE_IDENTITY();", _con, _tran);

            cmd.Parameters.AddWithValue("@Username", (object?)entity.Username ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Bio", (object?)entity.Bio ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object?)entity.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@LastActive", (object?)entity.LastActive ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedAt", (object?)entity.CreatedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdatedAt", (object?)entity.UpdatedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PictureUrl", (object?)entity.PictureUrl ?? DBNull.Value);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public override User? GetById(int id)
        {
            using var cmd = new SqlCommand("SELECT * FROM Users WHERE Id = @Id", _con, _tran);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapReaderToEntity(reader);
            }
            return null;
        }

        public override List<User> GetAll()
        {
            var users = new List<User>();
            using var cmd = new SqlCommand("SELECT * FROM Users", _con, _tran);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                users.Add(MapReaderToEntity(reader));
            }
            return users;
        }

        public override bool Update(User entity)
        {
            using var cmd = new SqlCommand(@"
                    UPDATE Users SET
                        Username = @Username,
                        Bio = @Bio,
                        Email = @Email,
                        LastActive = @LastActive,
                        CreatedAt = @CreatedAt,
                        UpdatedAt = @UpdatedAt, 
                        PictureUrl = @PictureUrl
                    WHERE Id = @Id", _con, _tran);

            cmd.Parameters.AddWithValue("@Id", entity.Id);
            cmd.Parameters.AddWithValue("@Username", (object?)entity.Username ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Bio", (object?)entity.Bio ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object?)entity.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@LastActive", (object?)entity.LastActive ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedAt", (object?)entity.CreatedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdatedAt", (object?)entity.UpdatedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PictureUrl", (object?)entity.PictureUrl ?? DBNull.Value);

            return cmd.ExecuteNonQuery() > 0;
        }

        public override bool Delete(int id)
        {
            using var cmd = new SqlCommand("DELETE FROM Users WHERE Id = @Id", _con, _tran);
            cmd.Parameters.AddWithValue("@Id", id);

            return cmd.ExecuteNonQuery() > 0;
        }

        protected override User MapReaderToEntity(SqlDataReader reader)
        {
            return new User
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Username = reader["Username"] as string,
                Bio = reader["Bio"] as string,
                Email = reader["Email"] as string,
                LastActive = reader["LastActive"] as DateTime?,
                CreatedAt = reader["CreatedAt"] as DateTime?,
                UpdatedAt = reader["UpdatedAt"] as DateTime?,
                PictureUrl = reader["PictureUrl"] as string
            };
        }
    }
}
