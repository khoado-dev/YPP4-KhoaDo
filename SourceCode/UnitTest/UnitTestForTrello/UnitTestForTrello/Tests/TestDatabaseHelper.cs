using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestForTrello.Tests
{
    public static class TestDatabaseHelper
    {
        public static (SqliteConnection, IDbTransaction) CreateInMemoryDatabaseAndSchema()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();
            var transaction = connection.BeginTransaction();

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

            connection.Execute(@"
            CREATE TABLE UserStarredBoard (
                UserId INTEGER,
                BoardId INTEGER,
                CreatedAt TEXT,
                StarredBoardsStatus INTEGER
            );
        ", transaction: transaction);

            return (connection, transaction);
        }

        public static void AddSeedTestData(IDbConnection connection, IDbTransaction transaction)
        {
            connection.Execute(@"
            INSERT INTO Board (Id, BoardName, BoardDescription, CreatedAt, BackgroundUrl, BoardStatus, WorkspaceId)
            VALUES (1, 'Test Board 1', 'Description', datetime('now'), 'url1', 'active', 1),
                   (2, 'Inactive Board', 'Description', datetime('now'), 'url2', 'archived', 1)
        ", transaction: transaction);

            connection.Execute(@"
            INSERT INTO UserStarredBoard (UserId, BoardId, CreatedAt, StarredBoardsStatus)
            VALUES (1, 1, datetime('now'), 1),
                   (1, 2, datetime('now'), 1)
        ", transaction: transaction);
        }
    }

}
