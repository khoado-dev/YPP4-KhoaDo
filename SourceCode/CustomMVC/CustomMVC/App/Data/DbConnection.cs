using Microsoft.Data.Sqlite;

namespace CustomMVC.App.Data
{
    public class DbConnection : IDbConnection
    {
        private readonly string _connectionString;
        private static readonly string DataDir = Path.Combine(AppContext.BaseDirectory, "Data");
        private static readonly string DbFile = Path.Combine(DataDir, "mvc.db");
        public DbConnection()
        {
            Directory.CreateDirectory(DataDir);

            _connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = DbFile,
                Mode = SqliteOpenMode.ReadWriteCreate,
                Cache = SqliteCacheMode.Shared
            }.ToString();

            SQLitePCL.Batteries_V2.Init();
        }

        public SqliteConnection Open()
        {
            var conn = new SqliteConnection(this._connectionString);

            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "PRAGMA foreign_keys=ON;";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "PRAGMA journal_mode=WAL;";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "PRAGMA synchronous=NORMAL;";
                cmd.ExecuteNonQuery();
            }

            return conn;
        }

        public void EnsureCreatedAndSeed()
        {
            using var conn = Open();
            using var cmd = conn.CreateCommand();

            cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Users (
                Id     INTEGER PRIMARY KEY AUTOINCREMENT,
                Name   TEXT    NOT NULL,
                Email  TEXT    NOT NULL UNIQUE
            );";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"
            INSERT OR IGNORE INTO Users(Name, Email) VALUES
             ('Alice','alice@example.com'),
             ('Bob','bob@example.com');";
            cmd.ExecuteNonQuery();
        }
    }
}
