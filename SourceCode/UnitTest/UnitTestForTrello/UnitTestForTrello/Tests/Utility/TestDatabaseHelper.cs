using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace UnitTestForTrello.Tests.Utility
{
    public static class TestDatabaseHelper
    {
        public static (SqliteConnection, IDbTransaction) CreateInMemoryDatabaseAndSchema()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();
            var transaction = connection.BeginTransaction();

            connection.Execute(@"
            CREATE TABLE [User] (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                PictureUrl TEXT,
                Email TEXT NOT NULL UNIQUE,
                Username TEXT NOT NULL,
                Bio TEXT
            );");

            // Board table
            connection.Execute(@"
                CREATE TABLE Board (
                    Id INTEGER PRIMARY KEY,
                    BoardName TEXT,
                    BoardDescription TEXT,
                    CreatedAt TEXT,
                    CreatedBy INTEGER,
                    BackgroundUrl TEXT,
                    BoardStatus TEXT,
                    WorkspaceId INTEGER
                );
            ", transaction: transaction);

            // UserStarredBoard table
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

            connection.Execute(@"
                CREATE TABLE Workspace (
                    Id INTEGER PRIMARY KEY,
                    WorkspaceName TEXT,
                    LogoUrl TEXT,
                    CreatedAt TEXT,
                    WorkspaceTypeId INTEGER
                );
            ", transaction: transaction);

            connection.Execute(@"
                CREATE TABLE Members (
                    UserId INTEGER,
                    OwnerId INTEGER,
                    OwnerTypeId INTEGER
                );
            ", transaction: transaction);

            connection.Execute(@"
            CREATE TABLE WorkspaceType (
                Id INTEGER PRIMARY KEY,
                TypeValue TEXT NOT NULL,
                DisplayValue TEXT NOT NULL
            );
            ", transaction: transaction);


            return (connection, transaction);
        }

        public static void SeedBoards(IDbConnection connection, IDbTransaction transaction)
        {
            connection.Execute(@"

           INSERT INTO Board (Id, BoardName, BoardDescription, CreatedAt, CreatedBy, BackgroundUrl, BoardStatus, WorkspaceId)
            VALUES 
                (1, 'Test Board 1', 'Description', datetime('now'), 1, 'url1', 'active', 1),
                (2, 'Test Board 2', 'Description', datetime('now'), 1, 'url2', 'active', 1),
                (3, 'Inactive Board', 'Description', datetime('now'), 1, 'url3', 'archived', 1);
        ", transaction: transaction);
        }

        public static void SeedUserStarredBoards(IDbConnection connection, IDbTransaction transaction)
        {
            connection.Execute(@"
            INSERT INTO UserStarredBoard (UserId, BoardId, CreatedAt, StarredBoardsStatus)
            VALUES 
                (1, 1, datetime('now'), 1),
                (1, 2, datetime('now'), 1);
        ", transaction: transaction);
        }

        public static void SeedOwnerTypes(IDbConnection connection, IDbTransaction transaction)
        {
            connection.Execute(@"
            INSERT INTO OwnerType (Id, OwnerTypeValue)
            VALUES 
                (1, 'WORKSPACE'),
                (2, 'BOARD'),
                (3, 'USER'),
                (4, 'CARD');
            ", transaction: transaction);
        }

        public static void SeedUserViewHistories(IDbConnection connection, IDbTransaction transaction)
        {
            connection.Execute(@"
            INSERT INTO UserViewHistory (Id, UserId, OwnerId, OwnerTypeId, AccessedAt)
            VALUES 
                (1, 1, 1, 2, datetime('now', '-1 day')),
                (2, 1, 2, 2, datetime('now', '-2 day')),
                (3, 2, 1, 2, datetime('now', '-3 day'));
            ", transaction: transaction);
        }
        public static void SeedWorkspaces(IDbConnection connection, IDbTransaction transaction)
        {
            connection.Execute(@"
            INSERT INTO Workspace (Id, WorkspaceName, LogoUrl, CreatedAt, WorkspaceTypeId)
            VALUES 
                (1, 'Workspace 1', 'logo1.png', datetime('now', '-5 day'), 1),
                (2, 'Workspace 2', 'logo2.png', datetime('now', '-10 day'), 3);
            ", transaction: transaction);
        }

        public static void SeedMembersOfWorkspace(IDbConnection connection, IDbTransaction transaction)
        {
            connection.Execute(@"
                INSERT INTO Members (UserId, OwnerId, OwnerTypeId)
                VALUES
                    (1, 1, 1), -- User 1 thuộc Workspace 1
                    (1, 2, 1), -- User 1 thuộc Workspace 2
                    (2, 1, 1); -- User 2 thuộc Workspace 1
            ", transaction: transaction);
        }

        public static void SeedMembersOfBoard(IDbConnection connection, IDbTransaction transaction)
        {
            connection.Execute(@"
                INSERT INTO Members (UserId, OwnerId, OwnerTypeId)
                VALUES
                    (1, 1, 2),  -- User 1 là thành viên của Board 1 (OwnerTypeId = 2 => BOARD)
                    (1, 2, 2),  -- User 1 là thành viên của Board 2
                    (2, 3, 2);  -- User 2 là thành viên của Board 3
            ", transaction: transaction);
        }

        public static void SeedUsers(IDbConnection connection, IDbTransaction transaction)
        {
            connection.Execute(@"
                INSERT INTO [User] (Id, PictureUrl, Email, Username, Bio) VALUES
                (1, 'https://example.com/images/james85.png', 'james85@booth-daniels.net', 'james85', 'Software engineer and coffee lover.'),
                (2, 'https://example.com/images/alice99.png', 'alice99@example.com', 'alice99', 'UI/UX designer with a passion for art.'),
                (3, 'https://example.com/images/bob77.png', 'bob77@example.com', 'bob77', 'Backend developer and open-source enthusiast.');
            ", transaction: transaction);
        }
        public static void SeedWorkspaceTypes(IDbConnection connection, IDbTransaction transaction)
        {
            connection.Execute(@"
                INSERT INTO WorkspaceType (Id, TypeValue, DisplayValue) VALUES
                    (1, 'business', 'Business'),
                    (2, 'sales_crm', 'Sales CRM'),
                    (3, 'engineering_it', 'Engineering-IT'),
                    (4, 'small_business', 'Small Business'),
                    (5, 'education', 'Education'),
                    (6, 'human_resources', 'Human Resources'),
                    (7, 'operations', 'Operations'),
                    (8, 'marketing', 'Marketing'),
                    (9, 'other', 'Other');
            ", transaction: transaction);
        }

        public static void SeedAllData(IDbConnection connection, IDbTransaction transaction)
        {
            SeedBoards(connection, transaction);
            SeedUserStarredBoards(connection, transaction);
            SeedOwnerTypes(connection, transaction);
            SeedUserViewHistories(connection, transaction);
            SeedWorkspaces(connection, transaction);
            SeedMembersOfWorkspace(connection, transaction);
            SeedMembersOfBoard(connection, transaction);
            SeedUsers(connection, transaction);
        }
    }
}
