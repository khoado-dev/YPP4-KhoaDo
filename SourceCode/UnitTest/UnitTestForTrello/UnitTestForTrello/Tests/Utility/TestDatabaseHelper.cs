using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace UnitTestForTrello.Tests.Utility
{
    public static class TestDatabaseHelper
    {
        public static SqliteConnection CreateInMemoryDatabaseAndSchema()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

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
            ");

            // UserStarredBoard table
            connection.Execute(@"
                CREATE TABLE UserStarredBoard (
                    UserId INTEGER,
                    BoardId INTEGER,
                    CreatedAt TEXT,
                    StarredBoardsStatus INTEGER
                );
            ");

            // OwnerType table
            connection.Execute(@"
                CREATE TABLE OwnerType (
                    Id INTEGER PRIMARY KEY,
                    OwnerTypeValue TEXT
                );
            ");

            // UserViewHistory table
            connection.Execute(@"
                CREATE TABLE UserViewHistory (
                    Id INTEGER PRIMARY KEY,
                    UserId INTEGER,
                    OwnerId INTEGER,
                    OwnerTypeId INTEGER,
                    AccessedAt TEXT
                );
            ");

            connection.Execute(@"
                CREATE TABLE Workspace (
                    Id INTEGER PRIMARY KEY,
                    WorkspaceName TEXT,
                    LogoUrl TEXT,
                    CreatedAt TEXT,
                    ShortName TEXT,
                    Website TEXT,
                    WorkspaceDescription TEXT
                );
            ");


            connection.Execute(@"
                CREATE TABLE Members (
                    UserId INTEGER,
                    OwnerId INTEGER,
                    OwnerTypeId INTEGER
                );
            ");

            connection.Execute(@"
            CREATE TABLE WorkspaceType (
                Id INTEGER PRIMARY KEY,
                TypeValue TEXT NOT NULL,
                DisplayValue TEXT NOT NULL
            );
            ");

            connection.Execute(@"
            CREATE TABLE Color (
                Id INTEGER PRIMARY KEY,
                ColorName TEXT
            );
            ");

            // Stage
            connection.Execute(@"
            CREATE TABLE Stage (
                Id INTEGER PRIMARY KEY,
                Title TEXT,
                Position INTEGER,
                BoardId INTEGER,
                ColorId INTEGER
            );
            ");

            // Cards
            connection.Execute(@"
            CREATE TABLE Cards (
                Id INTEGER PRIMARY KEY,
                Title TEXT,
                Position INTEGER,
                StageId INTEGER,
                CardLocation TEXT,
                CoverValue TEXT
            );
            ");

            // Comment
            connection.Execute(@"
            CREATE TABLE Comment (
                Id INTEGER PRIMARY KEY,
                CardId INTEGER,
                Content TEXT
            );
            ");

            // CheckList
            connection.Execute(@"
            CREATE TABLE CheckList (
                Id INTEGER PRIMARY KEY,
                CardId INTEGER,
                Title TEXT
            );
            ");

            // CheckListItem
            connection.Execute(@"
            CREATE TABLE CheckListItem (
                Id INTEGER PRIMARY KEY,
                CheckListId INTEGER,
                Title TEXT
            );
            ");

            // Attachment
            connection.Execute(@"
            CREATE TABLE Attachment (
                Id INTEGER PRIMARY KEY,
                CardId INTEGER,
                FileUrl TEXT
            );
            ");

            return connection;
        }

        public static void SeedBoards(IDbConnection connection)
        {
            connection.Execute(@"
            INSERT INTO Board (Id, BoardName, BoardDescription, CreatedAt, CreatedBy, BackgroundUrl, BoardStatus, WorkspaceId)
            VALUES 
                (1, 'Test Board 1', 'Description', datetime('now'), 1, 'url1', 'active', 1),
                (2, 'Test Board 2', 'Description', datetime('now'), 1, 'url2', 'active', 1),
                (3, 'Inactive Board', 'Description', datetime('now'), 1, 'url3', 'archived', 1);
            ");
        }

        public static void SeedUserStarredBoards(IDbConnection connection)
        {
            connection.Execute(@"
            INSERT INTO UserStarredBoard (UserId, BoardId, CreatedAt, StarredBoardsStatus)
            VALUES 
                (1, 1, datetime('now'), 1),
                (1, 2, datetime('now'), 1);
            ");
        }

        public static void SeedOwnerTypes(IDbConnection connection)
        {
            connection.Execute(@"
            INSERT INTO OwnerType (Id, OwnerTypeValue)
            VALUES 
                (1, 'WORKSPACE'),
                (2, 'BOARD'),
                (3, 'USER'),
                (4, 'CARD');
            ");
        }

        public static void SeedUserViewHistories(IDbConnection connection)
        {
            connection.Execute(@"
            INSERT INTO UserViewHistory (Id, UserId, OwnerId, OwnerTypeId, AccessedAt)
            VALUES 
                (1, 1, 1, 2, datetime('now', '-1 day')),
                (2, 1, 2, 2, datetime('now', '-2 day')),
                (3, 2, 1, 2, datetime('now', '-3 day'));
            ");
        }
        public static void SeedWorkspaces(IDbConnection connection)
        {
            connection.Execute(@"
            INSERT INTO Workspace (Id, WorkspaceName, LogoUrl, CreatedAt, ShortName, Website, WorkspaceDescription)
            VALUES 
                (1, 'Workspace 1', 'logo1.png', datetime('now', '-5 day'), 'WS1', 'https://workspace1.com', 'Description for Workspace 1'),
                (2, 'Workspace 2', 'logo2.png', datetime('now', '-10 day'), 'WS2', 'https://workspace2.com', 'Description for Workspace 2');
            ");
        }

        public static void SeedMembersOfWorkspace(IDbConnection connection)
        {
            connection.Execute(@"
                INSERT INTO Members (UserId, OwnerId, OwnerTypeId)
                VALUES
                    (1, 1, 1), -- User 1 thuộc Workspace 1
                    (1, 2, 1), -- User 1 thuộc Workspace 2
                    (2, 1, 1); -- User 2 thuộc Workspace 1
            ");
        }

        public static void SeedMembersOfBoard(IDbConnection connection)
        {
            connection.Execute(@"
            INSERT INTO Members (UserId, OwnerId, OwnerTypeId)
            VALUES
                (1, 1, 2),  -- User 1 là thành viên của Board 1 (OwnerTypeId = 2 => BOARD)
                (1, 2, 2),  -- User 1 là thành viên của Board 2
                (2, 3, 2),  -- User 2 là thành viên của Board 3
                (2, 1, 2),  -- thêm User 2 vào Board 1
                (3, 1, 2);  -- thêm User 3 vào Board 1
            ");
        }

        public static void SeedUsers(IDbConnection connection)
        {
            connection.Execute(@"
            INSERT INTO [User] (Id, PictureUrl, Email, Username, Bio) VALUES
            (1, 'https://example.com/images/james85.png', 'james85@booth-daniels.net', 'james85', 'Software engineer and coffee lover.'),
            (2, 'https://example.com/images/alice99.png', 'alice99@example.com', 'alice99', 'UI/UX designer with a passion for art.'),
            (3, 'https://example.com/images/bob77.png', 'bob77@example.com', 'bob77', 'Backend developer and open-source enthusiast.');
            ");
        }
        public static void SeedWorkspaceTypes(IDbConnection connection)
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
            ");
        }

        public static void SeedCardRelatedData(IDbConnection connection)
        {
            // Color
            connection.Execute(@"
            INSERT INTO Color (Id, ColorName) VALUES
            (1, 'Red'),
            (2, 'Blue');
            ");

            // Stage
            connection.Execute(@"
            INSERT INTO Stage (Id, Title, Position, BoardId, ColorId) VALUES
            (1, 'To Do', 1, 1, 1),
            (2, 'In Progress', 2, 1, 2);
            ");

            // Cards
            connection.Execute(@"
            INSERT INTO Cards (Id, Title, Position, StageId, CardLocation, CoverValue) VALUES
            (1, 'Card 1', 1, 1, 'List 1', 'Cover1'),
            (2, 'Card 2', 2, 1, 'List 1', 'Cover2'),
            (3, 'Card 3', 1, 2, 'List 2', 'Cover3');
            ");

            // Comment
            connection.Execute(@"
            INSERT INTO Comment (Id, CardId, Content) VALUES
            (1, 1, 'First comment'),
            (2, 1, 'Second comment'),
            (3, 2, 'Only comment');
            ");

            // CheckList + CheckListItem
            connection.Execute(@"
            INSERT INTO CheckList (Id, CardId, Title) VALUES
            (1, 1, 'Checklist 1'),
            (2, 2, 'Checklist 2');

            INSERT INTO CheckListItem (Id, CheckListId, Title) VALUES
            (1, 1, 'Item 1'),
            (2, 1, 'Item 2'),
            (3, 2, 'Item 3');
            ");

            // Attachment
            connection.Execute(@"
            INSERT INTO Attachment (Id, CardId, FileUrl) VALUES
            (1, 1, 'file1.jpg'),
            (2, 1, 'file2.png'),
            (3, 3, 'file3.docx');
            ");
        }
        public static void SeedAllData(IDbConnection connection)
        {
            SeedUsers(connection);
            SeedWorkspaceTypes(connection);
            SeedWorkspaces(connection);
            SeedOwnerTypes(connection);

            SeedBoards(connection);
            SeedUserStarredBoards(connection);
            SeedMembersOfWorkspace(connection);
            SeedMembersOfBoard(connection);

            SeedUserViewHistories(connection);

            SeedCardRelatedData(connection);
        }
    }
}
