using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace UnitTestForTrello.Tests
{
    public static class TestDatabaseHelper
    {
        public static (SqliteConnection, IDbTransaction) CreateInMemoryDatabaseAndSchema()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();
            var transaction = connection.BeginTransaction();

            // Board table
            connection.Execute(@"
                CREATE TABLE Board (
                    Id INTEGER PRIMARY KEY,
                    BoardName TEXT,
                    BoardDescription TEXT,
                    CreatedAt TEXT,
                    BackgroundUrl TEXT,
                    BoardStatus TEXT,
                    WorkspaceId INTEGER
                );
            ", transaction: transaction);

            // UserStarredBoard table (giữ nguyên)
            connection.Execute(@"
                CREATE TABLE UserStarredBoard (
                    UserId INTEGER,
                    BoardId INTEGER,
                    CreatedAt TEXT,
                    StarredBoardsStatus INTEGER
                );
            ", transaction: transaction);

            // OwnerType table
            connection.Execute(@"
                CREATE TABLE OwnerType (
                    Id INTEGER PRIMARY KEY,
                    OwnerTypeValue TEXT
                );
            ", transaction: transaction);

            // UserViewHistory table
            connection.Execute(@"
                CREATE TABLE UserViewHistory (
                    Id INTEGER PRIMARY KEY,
                    UserId INTEGER,
                    OwnerId INTEGER,
                    OwnerTypeId INTEGER,
                    AccessedAt TEXT
                );
            ", transaction: transaction);

            return (connection, transaction);
        }

        public static void AddSeedTestData(IDbConnection connection, IDbTransaction transaction)
        {
            // Seed Board
            connection.Execute(@"
                INSERT INTO Board (Id, BoardName, BoardDescription, CreatedAt, BackgroundUrl, BoardStatus, WorkspaceId)
                VALUES (1, 'Test Board 1', 'Description', datetime('now'), 'url1', 'active', 1),
                       (2, 'Inactive Board', 'Description', datetime('now'), 'url2', 'archived', 1);
            ", transaction: transaction);

            // Seed UserStarredBoard (giữ nguyên)
            connection.Execute(@"
                INSERT INTO UserStarredBoard (UserId, BoardId, CreatedAt, StarredBoardsStatus)
                VALUES (1, 1, datetime('now'), 1),
                       (1, 2, datetime('now'), 1);
            ", transaction: transaction);

            // Seed OwnerType
            connection.Execute(@"
                INSERT INTO OwnerType (Id, OwnerTypeValue)
                VALUES (1, 'BOARD'),
                       (2, 'WORKSPACE');
            ", transaction: transaction);

            // Seed UserViewHistory
            connection.Execute(@"
                INSERT INTO UserViewHistory (Id, UserId, OwnerId, OwnerTypeId, AccessedAt)
                VALUES 
                    (1, 1, 1, 1, datetime('now', '-1 day')),
                    (2, 1, 2, 1, datetime('now', '-2 day')),
                    (3, 2, 1, 1, datetime('now', '-3 day')); -- user khác để test lọc
            ", transaction: transaction);
        }
    }
}
