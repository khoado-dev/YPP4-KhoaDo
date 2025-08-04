using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestForTrello.Models;

namespace UnitTestForTrello.Repositories
{
    public class UserRepository
    {
        private readonly SqlConnection _con;
        private readonly SqlTransaction _tran;

        public UserRepository(SqlConnection con, SqlTransaction tran)
        {
            _con = con;
            _tran = tran;
        }

        public int CreateUser(User user)
        {
            using var cmd = new SqlCommand(@"
                    INSERT INTO Users 
                        (Username, Bio, Email, LastActive, CreatedAt, UpdatedAt, PictureUrl)
                    VALUES 
                        (@Username, @Bio, @Email, @LastActive, @CreatedAt, @UpdatedAt, @PictureUrl);
                    SELECT SCOPE_IDENTITY();", _con, _tran);

            cmd.Parameters.AddWithValue("@Username", (object?)user.Username ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Bio", (object?)user.Bio ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object?)user.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@LastActive", (object?)user.LastActive ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedAt", (object?)user.CreatedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdatedAt", (object?)user.UpdatedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PictureUrl", (object?)user.PictureUrl ?? DBNull.Value);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public User? GetUserById(int id)
        {
            using var cmd = new SqlCommand("SELECT * FROM Users WHERE Id = @Id", _con, _tran);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapReaderToUser(reader);
            }
            return null;
        }

        public List<User> GetAllUsers()
        {
            var users = new List<User>();
            using var cmd = new SqlCommand("SELECT * FROM Users", _con, _tran);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                users.Add(MapReaderToUser(reader));
            }
            return users;
        }

        public bool UpdateUser(User user)
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

            cmd.Parameters.AddWithValue("@Id", user.Id);
            cmd.Parameters.AddWithValue("@Username", (object?)user.Username ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Bio", (object?)user.Bio ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object?)user.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@LastActive", (object?)user.LastActive ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedAt", (object?)user.CreatedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdatedAt", (object?)user.UpdatedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PictureUrl", (object?)user.PictureUrl ?? DBNull.Value);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool DeleteUser(int id)
        {
            using var cmd = new SqlCommand("DELETE FROM Users WHERE Id = @Id", _con, _tran);
            cmd.Parameters.AddWithValue("@Id", id);

            return cmd.ExecuteNonQuery() > 0;
        }

        private User MapReaderToUser(SqlDataReader reader)
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
