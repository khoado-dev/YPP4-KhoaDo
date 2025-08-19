using System;
using Dapper;
using Microsoft.Data.Sqlite;

namespace UnitTestForTrello.Tests.Utility
{
    public static class TestDatabase
    {
        private static SqliteConnection? _conn;

        public static SqliteConnection OpenAndInit()
        {
            int defaultTimeout = 5000; // 5 seconds
            if (_conn is not null && _conn.State == System.Data.ConnectionState.Open)
                return _conn;

            _conn = new SqliteConnection("Data Source=:memory:");
            _conn.Open();

            // PRAGMAs cho in-memory
            _conn.Execute("PRAGMA journal_mode=WAL;");
            _conn.Execute("PRAGMA synchronous=NORMAL;");
            _conn.Execute("PRAGMA read_uncommitted = ON;");
            _conn.Execute($"PRAGMA busy_timeout={defaultTimeout};");

            CreateSchema();
            SeedAllData();

            return _conn;
        }

        public static SqliteConnection GetConnection()
        {
            if (_conn is null || _conn.State != System.Data.ConnectionState.Open)
                throw new InvalidOperationException("Database is not initialized. Call OpenAndInit() first.");
            return _conn;
        }

        public static void Reset() // Drop -> Create -> Seed
        {
            if (_conn is null) return;

            var dropSql = @"
                DROP TABLE IF EXISTS SettingValue;
                DROP TABLE IF EXISTS SettingKeySettingOption;
                DROP TABLE IF EXISTS SettingKey;
                DROP TABLE IF EXISTS SettingOption;

                DROP TABLE IF EXISTS Export;
                DROP TABLE IF EXISTS Subscription;
                DROP TABLE IF EXISTS PaymentInformation;
                DROP TABLE IF EXISTS BillingContact;
                DROP TABLE IF EXISTS BillingPlan;

                DROP TABLE IF EXISTS Template;
                DROP TABLE IF EXISTS TemplateCategory;

                DROP TABLE IF EXISTS WorkspaceMembershipDomain;
                DROP TABLE IF EXISTS UserViewHistory;
                DROP TABLE IF EXISTS UserStarredBoard;
                DROP TABLE IF EXISTS ShareLink;
                DROP TABLE IF EXISTS Notification;
                DROP TABLE IF EXISTS Activity;

                DROP TABLE IF EXISTS CommentReaction;
                DROP TABLE IF EXISTS Reaction;
                DROP TABLE IF EXISTS ReactionCategory;

                DROP TABLE IF EXISTS CardSticker;
                DROP TABLE IF EXISTS Sticker;
                DROP TABLE IF EXISTS StickerCategory;

                DROP TABLE IF EXISTS BoardPowerUp;
                DROP TABLE IF EXISTS PowerUp;
                DROP TABLE IF EXISTS PowerUpCategory;

                DROP TABLE IF EXISTS BoardCollection;
                DROP TABLE IF EXISTS Collections;

                DROP TABLE IF EXISTS FieldValue;
                DROP TABLE IF EXISTS FieldItem;
                DROP TABLE IF EXISTS CustomField;
                DROP TABLE IF EXISTS DataType;

                DROP TABLE IF EXISTS Attachment;
                DROP TABLE IF EXISTS AttachmentType;

                DROP TABLE IF EXISTS CardLabel;
                DROP TABLE IF EXISTS Labels;

                DROP TABLE IF EXISTS CheckListItem;
                DROP TABLE IF EXISTS CheckList;

                DROP TABLE IF EXISTS Comment;
                DROP TABLE IF EXISTS Cards;

                DROP TABLE IF EXISTS Stage;
                DROP TABLE IF EXISTS Color;

                DROP TABLE IF EXISTS Members;
                DROP TABLE IF EXISTS OwnerType;
                DROP TABLE IF EXISTS RolePermission;

                DROP TABLE IF EXISTS Board;
                DROP TABLE IF EXISTS Workspace;
                DROP TABLE IF EXISTS WorkspaceType;

                DROP TABLE IF EXISTS Users;
            ";
            _conn.Execute(dropSql);

            CreateSchema();
            SeedAllData();
        }

        public static void Dispose()
        {
            try { _conn?.Dispose(); }
            finally { _conn = null; }
        }

        // ================== SCHEMA ==================
        private static void CreateSchema()
        {
            GetConnection().Execute(@"
            CREATE TABLE IF NOT EXISTS Users(
                Id INTEGER PRIMARY KEY,
                Username TEXT,
                Bio TEXT,
                Email TEXT,
                LastActive TEXT,
                CreatedAt TEXT,
                UpdatedAt TEXT,
                PictureUrl TEXT,
                FullName TEXT
            );

            CREATE TABLE IF NOT EXISTS WorkspaceType(
                Id INTEGER PRIMARY KEY,
                TypeValue TEXT NOT NULL,
                DisplayValue TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Workspace(
                Id INTEGER PRIMARY KEY,
                WorkspaceName TEXT,
                WorkspaceDescription TEXT,
                ShortName TEXT,
                Website TEXT,
                TypeId INTEGER,
                CreatedAt TEXT,
                CreatedBy INTEGER,
                UpdatedAt TEXT,
                UpdatedBy INTEGER,
                LogoUrl TEXT
            );

            CREATE TABLE IF NOT EXISTS Board(
                Id INTEGER PRIMARY KEY,
                BoardName TEXT,
                BoardDescription TEXT,
                CreatedAt TEXT,
                CreatedBy INTEGER,
                BackgroundUrl TEXT,
                WorkspaceId INTEGER,
                BoardStatus TEXT,
                UpdatedAt TEXT,
                UpdatedBy INTEGER,
                IsTemplate INTEGER
            );

            CREATE TABLE IF NOT EXISTS Color(
                Id INTEGER PRIMARY KEY,
                ColorName TEXT,
                ColorHex TEXT,
                Icon TEXT
            );

            CREATE TABLE IF NOT EXISTS Stage(
                Id INTEGER PRIMARY KEY,
                Title TEXT,
                CreatedAt TEXT,
                CreatedBy INTEGER,
                BoardId INTEGER,
                StageStatus TEXT,
                ColorId INTEGER,
                Position INTEGER,
                UpdatedAt TEXT,
                UpdatedBy INTEGER
            );

            CREATE TABLE IF NOT EXISTS CardCoverType(
                Id INTEGER PRIMARY KEY,
                TypeValue TEXT NOT NULL,
                DisplayValue TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Cards(
                Id INTEGER PRIMARY KEY,
                StageId INTEGER,
                Title TEXT,
                CardDescription TEXT,
                CreatedAt TEXT,
                CreatedBy INTEGER,
                CardStatus TEXT,
                CardLocation TEXT,
                StartDate TEXT,
                DueDate TEXT,
                CardCoverTypeId INTEGER,
                CoverValue TEXT,
                Position INTEGER,
                UpdatedAt TEXT,
                UpdatedBy INTEGER,
                IsTemplate INTEGER,
                IsCompleted INTEGER
            );

            CREATE TABLE IF NOT EXISTS Labels(
                Id INTEGER PRIMARY KEY,
                Title TEXT,
                CreatedAt TEXT,
                CreatedBy INTEGER,
                UpdatedAt TEXT,
                UpdatedBy INTEGER,
                ColorId INTEGER,
                IsDefault INTEGER NOT NULL,
                BoardId INTEGER
            );

            CREATE TABLE IF NOT EXISTS CardLabel(
                CardId INTEGER,
                LabelId INTEGER
            );

            CREATE TABLE IF NOT EXISTS AttachmentType(
                Id INTEGER PRIMARY KEY,
                TypeValue TEXT NOT NULL,
                DisplayValue TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Attachment(
                Id INTEGER PRIMARY KEY,
                CardId INTEGER,
                AttachmentTypeId INTEGER,
                AttachmentPath TEXT,
                AttachmentName TEXT,
                CreatedAt TEXT,
                CreatedBy INTEGER,
                Size TEXT,
                IsCover INTEGER,
                Thumbnail TEXT
            );

            CREATE TABLE IF NOT EXISTS CheckList(
                Id INTEGER PRIMARY KEY,
                CheckListName TEXT,
                CardId INTEGER,
                Position INTEGER,
                CreatedAt TEXT,
                CreatedBy INTEGER,
                UpdatedAt TEXT,
                UpdatedBy INTEGER
            );

            CREATE TABLE IF NOT EXISTS RolePermission(
                Id INTEGER PRIMARY KEY,
                PermissionName TEXT,
                PermissionCode TEXT
            );

            CREATE TABLE IF NOT EXISTS OwnerType(
                Id INTEGER PRIMARY KEY,
                OwnerTypeValue TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Members(
                Id INTEGER PRIMARY KEY,
                UserId INTEGER,
                RolePermissonId INTEGER,
                OwnerTypeId INTEGER,
                OwnerId INTEGER,
                InvitedBy INTEGER,
                JoinedAt TEXT,
                MemberStatus TEXT
            );

            CREATE TABLE IF NOT EXISTS CheckListItem(
                Id INTEGER PRIMARY KEY,
                CheckListItemName TEXT,
                MemberId INTEGER,
                CheckListId INTEGER,
                DueDate TEXT,
                CheckListItemStatus INTEGER,
                CreatedAt TEXT,
                CreatedBy INTEGER,
                UpdatedAt TEXT,
                UpdatedBy INTEGER,
                Position INTEGER
            );

            CREATE TABLE IF NOT EXISTS Comment(
                Id INTEGER PRIMARY KEY,
                Content TEXT,
                CardId INTEGER,
                CreatedAt TEXT,
                CreatedBy INTEGER,
                UpdatedAt TEXT,
                UpdatedBy INTEGER
            );

            CREATE TABLE IF NOT EXISTS DataType(
                Id INTEGER PRIMARY KEY,
                DataTypeValue TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS CustomField(
                Id INTEGER PRIMARY KEY,
                Title TEXT,
                DataTypeId INTEGER,
                BoardId INTEGER,
                CreatedAt TEXT,
                CreatedBy INTEGER,
                UpdatedAt TEXT,
                UpdatedBy INTEGER,
                Position INTEGER,
                IsFrontCardShowed INTEGER NOT NULL
            );

            CREATE TABLE IF NOT EXISTS FieldItem(
                Id INTEGER PRIMARY KEY,
                ColorId INTEGER,
                FieldItemValue TEXT,
                Position INTEGER,
                CustomFieldId INTEGER
            );

            CREATE TABLE IF NOT EXISTS FieldValue(
                Id INTEGER PRIMARY KEY,
                CardId INTEGER,
                FieldValue TEXT,
                CustomFieldId INTEGER
            );

            CREATE TABLE IF NOT EXISTS Collections(
                Id INTEGER PRIMARY KEY,
                CollectionName TEXT,
                CreatedAt TEXT,
                CreatedBy INTEGER,
                UpdatedAt TEXT,
                UpdatedBy INTEGER,
                WorkspaceId INTEGER
            );

            CREATE TABLE IF NOT EXISTS BoardCollection(
                BoardId INTEGER,
                CollectionId INTEGER
            );

            CREATE TABLE IF NOT EXISTS PowerUpCategory(
                Id INTEGER PRIMARY KEY,
                CategoryValue TEXT NOT NULL,
                DisplayValue TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS PowerUp(
                Id INTEGER PRIMARY KEY,
                PowerUpName TEXT,
                IconUrl TEXT,
                BackgroundUrl TEXT,
                AuthorName TEXT,
                PowerUpDescription TEXT,
                EmailContact TEXT,
                PolicyUrl TEXT,
                IsStaffPick INTEGER,
                IsIntegration INTEGER,
                CategoryId INTEGER
            );

            CREATE TABLE IF NOT EXISTS BoardPowerUp(
                BoardId INTEGER,
                PowerUpId INTEGER,
                BoardPowerUpStatus INTEGER NOT NULL
            );

            CREATE TABLE IF NOT EXISTS StickerCategory(
                Id INTEGER PRIMARY KEY,
                CategoryValue TEXT NOT NULL,
                DisplayValue TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Sticker(
                Id INTEGER PRIMARY KEY,
                CategoryId INTEGER,
                StickerName TEXT,
                StickerUrl TEXT,
                CreatedAt TEXT,
                CreatedBy INTEGER
            );

            CREATE TABLE IF NOT EXISTS CardSticker(
                CardId INTEGER,
                StickerId INTEGER,
                CreatedAt TEXT,
                CreatedBy INTEGER,
                PositionX REAL,
                PositionY REAL,
                IndexZ INTEGER
            );

            CREATE TABLE IF NOT EXISTS ReactionCategory(
                Id INTEGER PRIMARY KEY,
                CategoryValue TEXT NOT NULL,
                DisplayValue TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Reaction(
                Id INTEGER PRIMARY KEY,
                ReactionName TEXT,
                ShortCode TEXT NOT NULL,
                CategoryId INTEGER,
                Icon TEXT
            );

            CREATE TABLE IF NOT EXISTS CommentReaction(
                CommentId INTEGER,
                ReactionId INTEGER,
                CreatedBy INTEGER,
                CreatedAt TEXT
            );

            CREATE TABLE IF NOT EXISTS Activity(
                Id INTEGER PRIMARY KEY,
                CreatedAt TEXT,
                ActivityDescription TEXT,
                UserId INTEGER,
                OwnerTypeId INTEGER,
                OwnerId INTEGER
            );

            CREATE TABLE IF NOT EXISTS Notification(
                Id INTEGER PRIMARY KEY,
                ActivityId INTEGER,
                IsRead INTEGER NOT NULL
            );

            CREATE TABLE IF NOT EXISTS ShareLink(
                Id INTEGER PRIMARY KEY,
                OwnerTypeId INTEGER,
                RolePermissonId INTEGER,
                OwnerId INTEGER,
                ShareLinkToken TEXT,
                ShareLinkStatus INTEGER NOT NULL
            );

            CREATE TABLE IF NOT EXISTS UserStarredBoard(
                UserId INTEGER,
                BoardId INTEGER,
                CreatedAt TEXT,
                StarredBoardsStatus INTEGER NOT NULL
            );

            CREATE TABLE IF NOT EXISTS UserViewHistory(
                UserId INTEGER,
                OwnerTypeId INTEGER,
                OwnerId INTEGER,
                AccessedAt TEXT
            );

            CREATE TABLE IF NOT EXISTS WorkspaceMembershipDomain(
                Id INTEGER PRIMARY KEY,
                WorkspaceId INTEGER,
                Domain TEXT,
                CreatedAt TEXT
            );

            CREATE TABLE IF NOT EXISTS TemplateCategory(
                Id INTEGER PRIMARY KEY,
                CategoryValue TEXT NOT NULL,
                DisplayValue TEXT NOT NULL,
                IconUrl TEXT
            );

            CREATE TABLE IF NOT EXISTS Template(
                Id INTEGER PRIMARY KEY,
                Title TEXT,
                TemplateDescription TEXT,
                CategoryId INTEGER,
                Viewed INTEGER,
                Copied INTEGER,
                CreatedBy INTEGER,
                CreatedAt TEXT,
                UpdatedAt TEXT,
                UpdatedBy INTEGER,
                BoardId INTEGER,
                BackgroundUrl TEXT
            );

            CREATE TABLE IF NOT EXISTS SettingOption(
                Id INTEGER PRIMARY KEY,
                DisplayValue TEXT,
                SettingOptionValue TEXT
            );

            CREATE TABLE IF NOT EXISTS SettingKey(
                Id INTEGER PRIMARY KEY,
                KeyName TEXT,
                SettingKeyDescription TEXT,
                OwnerTypeId INTEGER,
                DefaultValue INTEGER,
                IsBoolean INTEGER NOT NULL
            );

            CREATE TABLE IF NOT EXISTS SettingKeySettingOption(
                SettingKeyId INTEGER,
                SettingOptionId INTEGER
            );

            CREATE TABLE IF NOT EXISTS SettingValue(
                Id INTEGER PRIMARY KEY,
                SettingKeyId INTEGER,
                SettingContent INTEGER,
                CreatedAt TEXT,
                CreatedBy INTEGER,
                UpdatedAt TEXT,
                UpdatedBy INTEGER,
                OwnerId INTEGER
            );

            CREATE TABLE IF NOT EXISTS BillingPlan(
                Id INTEGER PRIMARY KEY,
                PlanName TEXT,
                BillingPlanDescription TEXT,
                PricePerUser REAL,
                IsActive INTEGER NOT NULL
            );

            CREATE TABLE IF NOT EXISTS BillingContact(
                Id INTEGER PRIMARY KEY,
                UserId INTEGER,
                WorkspaceId INTEGER,
                BillingContactName TEXT,
                BillingContactEmail TEXT,
                BillingLanguage INTEGER,
                AdditionalInvoiceDetail TEXT
            );

            CREATE TABLE IF NOT EXISTS PaymentInformation(
                Id INTEGER PRIMARY KEY,
                BillingContactId INTEGER,
                CardNumber TEXT,
                CardBrand TEXT,
                ExpirationDate TEXT,
                Cvv TEXT,
                Country TEXT,
                PostalCode TEXT
            );

            CREATE TABLE IF NOT EXISTS Subscription(
                Id INTEGER PRIMARY KEY,
                BillingContactId INTEGER,
                BillingPlanId INTEGER,
                StartDate TEXT,
                EndDate TEXT,
                IsMonthly INTEGER NOT NULL,
                SubscriptionStatus INTEGER NOT NULL,
                AutoRenew INTEGER,
                MemberCountBilled INTEGER
            );

            CREATE TABLE IF NOT EXISTS Export(
                Id INTEGER PRIMARY KEY,
                WorkspaceId INTEGER,
                CreatedBy INTEGER,
                CreatedAt TEXT,
                Size INTEGER
            );
            ");
        }

        // ================== SEED ==================
        private static void SeedAllData()
        {
            SeedUsers();
            SeedWorkspaceTypes();
            SeedWorkspaces();
            SeedOwnerTypes();

            SeedBoards();
            SeedUserStarredBoards();
            SeedRolePermissions();
            SeedMembersOfWorkspace();
            SeedMembersOfBoard();

            SeedUserViewHistories();

            SeedCardRelatedData();
            SeedMembersOfCard();
            SeedCommentsWithReactionsForCard();
            SeedCardActivities();
            SeedCardCustomFieldAndValues();

            SeedAttachmentTypes();
            SeedAttachments();
        }

        private static void SeedBoards()
        {
            GetConnection().Execute(@"
            INSERT INTO Board (Id, BoardName, BoardDescription, CreatedAt, CreatedBy, BackgroundUrl, BoardStatus, WorkspaceId)
            VALUES 
                (1, 'Test Board 1', 'Description', datetime('now'), 1, 'url1', 'ACTIVE', 1),
                (2, 'Test Board 2', 'Description', datetime('now'), 1, 'url2', 'ACTIVE', 1),
                (3, 'Inactive Board', 'Description', datetime('now'), 1, 'url3', 'ARCHIVED', 1);
            ");
        }

        private static void SeedUserStarredBoards()
        {
            GetConnection().Execute(@"
            INSERT INTO UserStarredBoard (UserId, BoardId, CreatedAt, StarredBoardsStatus)
            VALUES 
                (1, 1, datetime('now'), 1),
                (1, 2, datetime('now'), 1);
            ");
        }

        private static void SeedOwnerTypes()
        {
            GetConnection().Execute(@"
            INSERT INTO OwnerType (Id, OwnerTypeValue)
            VALUES 
                (1, 'WORKSPACE'),
                (2, 'BOARD'),
                (3, 'USER'),
                (4, 'CARD');
            ");
        }

        private static void SeedUserViewHistories()
        {
            GetConnection().Execute(@"
            INSERT INTO UserViewHistory (UserId, OwnerId, OwnerTypeId, AccessedAt)
            VALUES 
                (1, 1, 2, datetime('now', '-1 day')),
                (1, 2, 2, datetime('now', '-2 day')),
                (2, 1, 2, datetime('now', '-3 day'));
            ");
        }

        private static void SeedWorkspaces()
        {
            GetConnection().Execute(@"
            INSERT INTO Workspace (Id, WorkspaceName, LogoUrl, CreatedAt, ShortName, Website, WorkspaceDescription)
            VALUES 
                (1, 'Workspace 1', 'logo1.png', datetime('now', '-5 day'), 'WS1', 'https://workspace1.com', 'Description for Workspace 1'),
                (2, 'Workspace 2', 'logo2.png', datetime('now', '-10 day'), 'WS2', 'https://workspace2.com', 'Description for Workspace 2');
            ");
        }

        private static void SeedMembersOfWorkspace()
        {
            GetConnection().Execute(@"
            INSERT INTO Members (UserId, OwnerId, OwnerTypeId, RolePermissonId, JoinedAt)
            VALUES
                (1, 1, 1, 1, datetime('now', '-6 day')),
                (1, 2, 1, 2, datetime('now', '-7 day')),
                (2, 1, 1, 2, datetime('now', '-8 day')),
                (3, 1, 1, 3, datetime('now', '-8 day'));
            ");
        }

        private static void SeedMembersOfBoard()
        {
            GetConnection().Execute(@"
            INSERT INTO Members (UserId, OwnerId, OwnerTypeId, RolePermissonId, JoinedAt)
            VALUES
                (1, 1, 2, 1, datetime('now', '-1 day')),
                (1, 2, 2, 2, datetime('now', '-2 day')),
                (2, 3, 2, 2, datetime('now', '-3 day')),
                (2, 1, 2, 2, datetime('now', '-4 day')),
                (3, 1, 2, 3, datetime('now', '-5 day'));
            ");
        }

        private static void SeedUsers()
        {
            GetConnection().Execute(@"
            INSERT INTO Users (Id, PictureUrl, Email, Username, Bio) VALUES
            (1, 'https://example.com/images/james85.png', 'james85@booth-daniels.net', 'james85', 'Software engineer and coffee lover.'),
            (2, 'https://example.com/images/alice99.png', 'alice99@example.com', 'alice99', 'UI/UX designer with a passion for art.'),
            (3, 'https://example.com/images/bob77.png', 'bob77@example.com', 'bob77', 'Backend developer and open-source enthusiast.');
            ");
        }

        private static void SeedWorkspaceTypes()
        {
            GetConnection().Execute(@"
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

        private static void SeedCardRelatedData()
        {
            GetConnection().Execute(@"
            INSERT INTO Color (Id, ColorName, Icon) VALUES
            (1, 'Red', 'https://example.com/icons/red-icon.png'),
            (2, 'Blue', 'https://example.com/icons/blue-icon.png'),
            (3, 'Green', 'https://example.com/icons/green-icon.png');

            INSERT INTO Stage (Id, Title, Position, BoardId, ColorId) VALUES
            (1, 'To Do', 1, 1, 1),
            (2, 'In Progress', 2, 1, 2);

            INSERT INTO Cards (Id, Title, CardDescription, Position, StageId, CardLocation, CoverValue) VALUES
            (1, 'Card 1', 'Description for Card 1', 1, 1, 'List 1', 'Cover1'),
            (2, 'Card 2', 'Description for Card 2', 2, 1, 'List 1', 'Cover2'),
            (3, 'Card 3', 'Description for Card 3', 1, 2, 'List 2', 'Cover3');

            INSERT INTO Comment (Id, CardId, Content) VALUES
            (1, 1, 'First comment'),
            (2, 1, 'Second comment'),
            (3, 2, 'Only comment');

            INSERT INTO CheckList (Id, CardId, CheckListName) VALUES
            (1, 1, 'Checklist 1'),
            (2, 2, 'Checklist 2');

            INSERT INTO CheckListItem (Id, CheckListId, CheckListItemName) VALUES
            (1, 1, 'Item 1'),
            (2, 1, 'Item 2'),
            (3, 2, 'Item 3');

            INSERT INTO Labels (Id, Title, ColorId, IsDefault) VALUES
            (1, 'Urgent', 1, 0),
            (2, 'Low Priority', 2, 0);

            INSERT INTO CardLabel (CardId, LabelId) VALUES
            (1, 1),
            (1, 2),
            (2, 1);
            ");
        }

        private static void SeedMembersOfCard()
        {
            GetConnection().Execute(@"
            INSERT INTO Members (UserId, OwnerId, OwnerTypeId, JoinedAt)
            VALUES
                (1, 1, 4, datetime('now', '-1 day')),
                (2, 1, 4, datetime('now', '-2 day'));
            ");
        }

        private static void SeedCommentsWithReactionsForCard()
        {
            GetConnection().Execute(@"
            INSERT INTO Comment (Id, CardId, Content, CreatedAt, UpdatedAt, CreatedBy) VALUES
                (100, 1, 'First comment', datetime('now','-1 day'), datetime('now'), 1),
                (101, 1, 'Second comment', datetime('now','-2 day'), datetime('now'), 2);

            INSERT INTO Reaction (Id, ReactionName, ShortCode) VALUES
                (1, 'Like', ':like:'),
                (2, 'Love', ':love:');

            INSERT INTO CommentReaction (CommentId, ReactionId, CreatedBy, CreatedAt) VALUES
                (100, 1, 2, datetime('now')),
                (100, 1, 3, datetime('now')),
                (100, 2, 2, datetime('now')),
                (101, 1, 3, datetime('now'));
            ");
        }

        private static void SeedCardActivities()
        {
            GetConnection().Execute(@"
            INSERT INTO Activity (Id, CreatedAt, ActivityDescription, UserId, OwnerTypeId, OwnerId) VALUES
                (1, datetime('now', '-1 day'), 'Added new checklist', 1, 4, 1),
                (2, datetime('now', '-2 day'), 'Changed card title', 2, 4, 1),
                (3, datetime('now', '-3 day'), 'Other workspace activity', 1, 1, 1);
            ");
        }

        private static void SeedCardCustomFieldAndValues()
        {
            GetConnection().Execute(@"
                INSERT INTO DataType (Id, DataTypeValue) VALUES
                (1, 'date'),
                (2, 'dropdown'),
                (3, 'number'),
                (4, 'text'),
                (5, 'boolean');

                INSERT INTO CustomField (Id, Title, DataTypeId, BoardId, Position, IsFrontCardShowed) VALUES
                (1, 'Priority', 2, 1, 1, 1),
                (2, 'Estimate', 3, 1, 2, 1),
                (3, 'Description', 4, 1, 3, 1);

                INSERT INTO FieldItem (Id, FieldItemValue, CustomFieldId, Position) VALUES
                (1, 'High', 1, 1),
                (2, 'Low', 1, 2);

                INSERT INTO FieldValue (Id, CardId, FieldValue, CustomFieldId) VALUES
                (1, 1, '1', 1),
                (2, 1, '5', 2),
                (3, 1, 'Some details', 3);
            ");
        }

        private static void SeedAttachmentTypes()
        {
            GetConnection().Execute(@"
            INSERT INTO AttachmentType (Id, TypeValue, DisplayValue) VALUES
            (1, 'card', 'Trello cards'),
            (2, 'link', 'Links'),
            (3, 'file', 'Files');
            ");
        }

        private static void SeedAttachments()
        {
            GetConnection().Execute(@"
            INSERT INTO Attachment (Id, CardId, AttachmentTypeId, AttachmentPath, AttachmentName, CreatedAt, CreatedBy, Size, IsCover, Thumbnail) VALUES
            (1, 1, 3, 'uploads/docs/file1.pdf', 'Project Plan', datetime('now', '-1 day'), 1, '200KB', 0, 'thumb1.png'),
            (2, 1, 2, 'https://example.com', 'Reference Link', datetime('now', '-2 day'), 2, NULL, 0, NULL),
            (3, 1, 1, NULL, 'Trello Card Ref', datetime('now', '-3 day'), 3, NULL, 1, NULL);
            ");
        }

        private static void SeedRolePermissions()
        {
            GetConnection().Execute(@"
            INSERT INTO RolePermission (Id, PermissionName, PermissionCode) VALUES
            (1, 'Admin', 'ADMIN'),
            (2, 'Member', 'MEMBER'),
            (3, 'Viewer', 'VIEWER');
            ");
        }
    }
}
