using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace UnitTestForTrello.Tests.Utility
{
    public static class TestDatabaseHelper
    {
        private static SqliteConnection? _connection;
        public static void CreateInMemoryDatabaseAndSchema()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            _connection?.Execute(@"
            CREATE TABLE Users(
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

            CREATE TABLE WorkspaceType(
                Id INTEGER PRIMARY KEY,
                TypeValue TEXT NOT NULL,
                DisplayValue TEXT NOT NULL
            );

            CREATE TABLE Workspace(
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

            CREATE TABLE Board(
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

            CREATE TABLE Color(
                Id INTEGER PRIMARY KEY,
                ColorName TEXT,
                ColorHex TEXT,
                Icon TEXT
            );

            CREATE TABLE Stage(
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

            CREATE TABLE CardCoverType(
                Id INTEGER PRIMARY KEY,
                TypeValue TEXT NOT NULL,
                DisplayValue TEXT NOT NULL
            );

            CREATE TABLE Cards(
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

            CREATE TABLE Labels(
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

            CREATE TABLE CardLabel(
                CardId INTEGER,
                LabelId INTEGER
            );

            CREATE TABLE AttachmentType(
                Id INTEGER PRIMARY KEY,
                TypeValue TEXT NOT NULL,
                DisplayValue TEXT NOT NULL
            );

            CREATE TABLE Attachment(
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

            CREATE TABLE CheckList(
                Id INTEGER PRIMARY KEY,
                CheckListName TEXT,
                CardId INTEGER,
                Position INTEGER,
                CreatedAt TEXT,
                CreatedBy INTEGER,
                UpdatedAt TEXT,
                UpdatedBy INTEGER
            );

            CREATE TABLE RolePermission(
                Id INTEGER PRIMARY KEY,
                PermissionName TEXT,
                PermissionCode TEXT
            );

            CREATE TABLE OwnerType(
                Id INTEGER PRIMARY KEY,
                OwnerTypeValue TEXT NOT NULL
            );

            CREATE TABLE Members(
                Id INTEGER PRIMARY KEY,
                UserId INTEGER,
                RolePermissonId INTEGER,
                OwnerTypeId INTEGER,
                OwnerId INTEGER,
                InvitedBy INTEGER,
                JoinedAt TEXT,
                MemberStatus TEXT
            );

            CREATE TABLE CheckListItem(
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

            CREATE TABLE Comment(
                Id INTEGER PRIMARY KEY,
                Content TEXT,
                CardId INTEGER,
                CreatedAt TEXT,
                CreatedBy INTEGER,
                UpdatedAt TEXT,
                UpdatedBy INTEGER
            );

            CREATE TABLE DataType(
                Id INTEGER PRIMARY KEY,
                DataTypeValue TEXT NOT NULL
            );

            CREATE TABLE CustomField(
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

            CREATE TABLE FieldItem(
                Id INTEGER PRIMARY KEY,
                ColorId INTEGER,
                FieldItemValue TEXT,
                Position INTEGER,
                CustomFieldId INTEGER
            );

            CREATE TABLE FieldValue(
                Id INTEGER PRIMARY KEY,
                CardId INTEGER,
                FieldValue TEXT,
                CustomFieldId INTEGER
            );

            CREATE TABLE Collections(
                Id INTEGER PRIMARY KEY,
                CollectionName TEXT,
                CreatedAt TEXT,
                CreatedBy INTEGER,
                UpdatedAt TEXT,
                UpdatedBy INTEGER,
                WorkspaceId INTEGER
            );

            CREATE TABLE BoardCollection(
                BoardId INTEGER,
                CollectionId INTEGER
            );

            CREATE TABLE PowerUpCategory(
                Id INTEGER PRIMARY KEY,
                CategoryValue TEXT NOT NULL,
                DisplayValue TEXT NOT NULL
            );

            CREATE TABLE PowerUp(
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

            CREATE TABLE BoardPowerUp(
                BoardId INTEGER,
                PowerUpId INTEGER,
                BoardPowerUpStatus INTEGER NOT NULL
            );

            CREATE TABLE StickerCategory(
                Id INTEGER PRIMARY KEY,
                CategoryValue TEXT NOT NULL,
                DisplayValue TEXT NOT NULL
            );

            CREATE TABLE Sticker(
                Id INTEGER PRIMARY KEY,
                CategoryId INTEGER,
                StickerName TEXT,
                StickerUrl TEXT,
                CreatedAt TEXT,
                CreatedBy INTEGER
            );

            CREATE TABLE CardSticker(
                CardId INTEGER,
                StickerId INTEGER,
                CreatedAt TEXT,
                CreatedBy INTEGER,
                PositionX REAL,
                PositionY REAL,
                IndexZ INTEGER
            );

            CREATE TABLE ReactionCategory(
                Id INTEGER PRIMARY KEY,
                CategoryValue TEXT NOT NULL,
                DisplayValue TEXT NOT NULL
            );

            CREATE TABLE Reaction(
                Id INTEGER PRIMARY KEY,
                ReactionName TEXT,
                ShortCode TEXT NOT NULL,
                CategoryId INTEGER,
                Icon TEXT
            );

            CREATE TABLE CommentReaction(
                CommentId INTEGER,
                ReactionId INTEGER,
                CreatedBy INTEGER,
                CreatedAt TEXT
            );

            CREATE TABLE Activity(
                Id INTEGER PRIMARY KEY,
                CreatedAt TEXT,
                ActivityDescription TEXT,
                UserId INTEGER,
                OwnerTypeId INTEGER,
                OwnerId INTEGER
            );

            CREATE TABLE Notification(
                Id INTEGER PRIMARY KEY,
                ActivityId INTEGER,
                IsRead INTEGER NOT NULL
            );

            CREATE TABLE ShareLink(
                Id INTEGER PRIMARY KEY,
                OwnerTypeId INTEGER,
                RolePermissonId INTEGER,
                OwnerId INTEGER,
                ShareLinkToken TEXT,
                ShareLinkStatus INTEGER NOT NULL
            );

            CREATE TABLE UserStarredBoard(
                UserId INTEGER,
                BoardId INTEGER,
                CreatedAt TEXT,
                StarredBoardsStatus INTEGER NOT NULL
            );

            CREATE TABLE UserViewHistory(
                UserId INTEGER,
                OwnerTypeId INTEGER,
                OwnerId INTEGER,
                AccessedAt TEXT
            );

            CREATE TABLE WorkspaceMembershipDomain(
                Id INTEGER PRIMARY KEY,
                WorkspaceId INTEGER,
                Domain TEXT,
                CreatedAt TEXT
            );

            CREATE TABLE TemplateCategory(
                Id INTEGER PRIMARY KEY,
                CategoryValue TEXT NOT NULL,
                DisplayValue TEXT NOT NULL,
                IconUrl TEXT
            );

            CREATE TABLE Template(
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

            CREATE TABLE SettingOption(
                Id INTEGER PRIMARY KEY,
                DisplayValue TEXT,
                SettingOptionValue TEXT
            );

            CREATE TABLE SettingKey(
                Id INTEGER PRIMARY KEY,
                KeyName TEXT,
                SettingKeyDescription TEXT,
                OwnerTypeId INTEGER,
                DefaultValue INTEGER,
                IsBoolean INTEGER NOT NULL
            );

            CREATE TABLE SettingKeySettingOption(
                SettingKeyId INTEGER,
                SettingOptionId INTEGER
            );

            CREATE TABLE SettingValue(
                Id INTEGER PRIMARY KEY,
                SettingKeyId INTEGER,
                SettingContent INTEGER,
                CreatedAt TEXT,
                CreatedBy INTEGER,
                UpdatedAt TEXT,
                UpdatedBy INTEGER,
                OwnerId INTEGER
            );

            CREATE TABLE BillingPlan(
                Id INTEGER PRIMARY KEY,
                PlanName TEXT,
                BillingPlanDescription TEXT,
                PricePerUser REAL,
                IsActive INTEGER NOT NULL
            );

            CREATE TABLE BillingContact(
                Id INTEGER PRIMARY KEY,
                UserId INTEGER,
                WorkspaceId INTEGER,
                BillingContactName TEXT,
                BillingContactEmail TEXT,
                BillingLanguage INTEGER,
                AdditionalInvoiceDetail TEXT
            );

            CREATE TABLE PaymentInformation(
                Id INTEGER PRIMARY KEY,
                BillingContactId INTEGER,
                CardNumber TEXT,
                CardBrand TEXT,
                ExpirationDate TEXT,
                Cvv TEXT,
                Country TEXT,
                PostalCode TEXT
            );

            CREATE TABLE Subscription(
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

            CREATE TABLE Export(
                Id INTEGER PRIMARY KEY,
                WorkspaceId INTEGER,
                CreatedBy INTEGER,
                CreatedAt TEXT,
                Size INTEGER
            );
            ");
        }
        public static void SeedBoards()
        {
            _connection?.Execute(@"
            INSERT INTO Board (Id, BoardName, BoardDescription, CreatedAt, CreatedBy, BackgroundUrl, BoardStatus, WorkspaceId)
            VALUES 
                (1, 'Test Board 1', 'Description', datetime('now'), 1, 'url1', 'active', 1),
                (2, 'Test Board 2', 'Description', datetime('now'), 1, 'url2', 'active', 1),
                (3, 'Inactive Board', 'Description', datetime('now'), 1, 'url3', 'archived', 1);
            ");
        }

        public static void SeedUserStarredBoards()
        {
            _connection?.Execute(@"
            INSERT INTO UserStarredBoard (UserId, BoardId, CreatedAt, StarredBoardsStatus)
            VALUES 
                (1, 1, datetime('now'), 1),
                (1, 2, datetime('now'), 1);
            ");
        }

        public static void SeedOwnerTypes()
        {
            _connection?.Execute(@"
            INSERT INTO OwnerType (Id, OwnerTypeValue)
            VALUES 
                (1, 'WORKSPACE'),
                (2, 'BOARD'),
                (3, 'USER'),
                (4, 'CARD');
            ");
        }

        public static void SeedUserViewHistories()
        {
            _connection?.Execute(@"
            INSERT INTO UserViewHistory (UserId, OwnerId, OwnerTypeId, AccessedAt)
            VALUES 
                (1, 1, 2, datetime('now', '-1 day')),
                (1, 2, 2, datetime('now', '-2 day')),
                (2, 1, 2, datetime('now', '-3 day'));
            ");
        }

        public static void SeedWorkspaces()
        {
            _connection?.Execute(@"
            INSERT INTO Workspace (Id, WorkspaceName, LogoUrl, CreatedAt, ShortName, Website, WorkspaceDescription)
            VALUES 
                (1, 'Workspace 1', 'logo1.png', datetime('now', '-5 day'), 'WS1', 'https://workspace1.com', 'Description for Workspace 1'),
                (2, 'Workspace 2', 'logo2.png', datetime('now', '-10 day'), 'WS2', 'https://workspace2.com', 'Description for Workspace 2');
            ");
        }

        public static void SeedMembersOfWorkspace()
        {
            _connection?.Execute(@"
            INSERT INTO Members (UserId, OwnerId, OwnerTypeId, JoinedAt)
            VALUES
                (1, 1, 1, datetime('now', '-6 day')), -- User 1 thuộc Workspace 1
                (1, 2, 1, datetime('now', '-7 day')), -- User 1 thuộc Workspace 2
                (2, 1, 1, datetime('now', '-8 day')), -- User 2 thuộc Workspace 1
                (3, 1, 1, datetime('now', '-8 day')); -- User 3 thuộc Workspace 1
        ");
        }

        public static void SeedMembersOfBoard()
        {
            _connection?.Execute(@"
            INSERT INTO Members (UserId, OwnerId, OwnerTypeId, JoinedAt)
            VALUES
                (1, 1, 2, datetime('now', '-1 day')),  -- User 1 là thành viên của Board 1 (BOARD)
                (1, 2, 2, datetime('now', '-2 day')),  -- User 1 là thành viên của Board 2
                (2, 3, 2, datetime('now', '-3 day')),  -- User 2 là thành viên của Board 3
                (2, 1, 2, datetime('now', '-4 day')),  -- Thêm User 2 vào Board 1
                (3, 1, 2, datetime('now', '-5 day'));  -- Thêm User 3 vào Board 1
            ");
        }

        public static void SeedUsers()
        {
            _connection?.Execute(@"
            INSERT INTO Users (Id, PictureUrl, Email, Username, Bio) VALUES
            (1, 'https://example.com/images/james85.png', 'james85@booth-daniels.net', 'james85', 'Software engineer and coffee lover.'),
            (2, 'https://example.com/images/alice99.png', 'alice99@example.com', 'alice99', 'UI/UX designer with a passion for art.'),
            (3, 'https://example.com/images/bob77.png', 'bob77@example.com', 'bob77', 'Backend developer and open-source enthusiast.');
            ");
        }

        public static void SeedWorkspaceTypes()
        {
            _connection?.Execute(@"
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

        public static void SeedCardRelatedData()
        {
            // Color
            _connection?.Execute(@"
            INSERT INTO Color (Id, ColorName, Icon) VALUES
            (1, 'Red', 'https://example.com/icons/red-icon.png'),
            (2, 'Blue', 'https://example.com/icons/blue-icon.png'),
            (3, 'Green', 'https://example.com/icons/green-icon.png');
            ");

            // Stage
            _connection?.Execute(@"
            INSERT INTO Stage (Id, Title, Position, BoardId, ColorId) VALUES
            (1, 'To Do', 1, 1, 1),
            (2, 'In Progress', 2, 1, 2);
            ");

            // Cards
            _connection?.Execute(@"
            INSERT INTO Cards (Id, Title, CardDescription, Position, StageId, CardLocation, CoverValue) VALUES
            (1, 'Card 1', 'Description for Card 1', 1, 1, 'List 1', 'Cover1'),
            (2, 'Card 2', 'Description for Card 2', 2, 1, 'List 1', 'Cover2'),
            (3, 'Card 3', 'Description for Card 3', 1, 2, 'List 2', 'Cover3');
            ");

            // Comment
            _connection?.Execute(@"
            INSERT INTO Comment (Id, CardId, Content) VALUES
            (1, 1, 'First comment'),
            (2, 1, 'Second comment'),
            (3, 2, 'Only comment');
            ");

            // CheckList + CheckListItem
            _connection?.Execute(@"
            INSERT INTO CheckList (Id, CardId, CheckListName) VALUES
            (1, 1, 'Checklist 1'),
            (2, 2, 'Checklist 2');

            INSERT INTO CheckListItem (Id, CheckListId, CheckListItemName) VALUES
            (1, 1, 'Item 1'),
            (2, 1, 'Item 2'),
            (3, 2, 'Item 3');
            ");

            // Label → Labels
            _connection?.Execute(@"
            INSERT INTO Labels (Id, Title, ColorId, IsDefault) VALUES
            (1, 'Urgent', 1, 0),
            (2, 'Low Priority', 2, 0);
            ");

            // CardLabel
            _connection?.Execute(@"
            INSERT INTO CardLabel (CardId, LabelId) VALUES
            (1, 1),
            (1, 2),
            (2, 1);
            ");
        }

        public static void SeedMembersOfCard()
        {
            _connection?.Execute(@"
            INSERT INTO Members (UserId, OwnerId, OwnerTypeId, JoinedAt)
            VALUES
                (1, 1, 4, datetime('now', '-1 day')),  -- CARD
                (2, 1, 4, datetime('now', '-2 day'));
            ");
        }
        public static void SeedCommentsWithReactionsForCard()
        {
            // Comments on Card 1
            _connection?.Execute(@"
            INSERT INTO Comment (Id, CardId, Content, CreatedAt, UpdatedAt, CreatedBy) VALUES
                (100, 1, 'First comment', datetime('now','-1 day'), datetime('now'), 1),
                (101, 1, 'Second comment', datetime('now','-2 day'), datetime('now'), 2);
            ");

            // Reactions
            _connection?.Execute(@"
            INSERT INTO Reaction (Id, ReactionName, ShortCode) VALUES
                (1, 'Like', ':like:'),
                (2, 'Love', ':love:');
            ");

            // CommentReaction (link comments to reactions)
            _connection?.Execute(@"
            INSERT INTO CommentReaction (CommentId, ReactionId, CreatedBy, CreatedAt) VALUES
                (100, 1, 2, datetime('now')),
                (100, 1, 3, datetime('now')),
                (100, 2, 2, datetime('now')),
                (101, 1, 3, datetime('now')); -- second comment has only one reaction
            ");
        }
        public static void SeedCardActivities()
        {
            _connection?.Execute(@"
            INSERT INTO Activity (Id, CreatedAt, ActivityDescription, UserId, OwnerTypeId, OwnerId) VALUES
                (1, datetime('now', '-1 day'), 'Added new checklist', 1, 4, 1),
                (2, datetime('now', '-2 day'), 'Changed card title', 2, 4, 1),
                (3, datetime('now', '-3 day'), 'Other workspace activity', 1, 1, 1); -- không match vì OwnerType != CARD
            ");
        }

        public static void SeedCardCustomFieldAndValues()
        {
            // DataType
            _connection?.Execute(@"
                INSERT INTO DataType (Id, DataTypeValue) VALUES
                (1, 'date'),
                (2, 'dropdown'),
                (3, 'number'),
                (4, 'text'),
                (5, 'boolean');
            ");

            // CustomField
            _connection?.Execute(@"
                INSERT INTO CustomField (Id, Title, DataTypeId, BoardId, Position, IsFrontCardShowed) VALUES
                (1, 'Priority', 2, 1, 1, 1),     -- dropdown
                (2, 'Estimate', 3, 1, 2, 1),     -- number
                (3, 'Description', 4, 1, 3, 1);  -- text
            ");

            // FieldItem (Dropdown for Priority)
            _connection?.Execute(@"
                INSERT INTO FieldItem (Id, FieldItemValue, CustomFieldId, Position) VALUES
                (1, 'High', 1, 1),
                (2, 'Low', 1, 2);
            ");

            // FieldValue
            _connection?.Execute(@"
                INSERT INTO FieldValue (Id, CardId, FieldValue, CustomFieldId) VALUES
                (1, 1, '1', 1),               -- Dropdown (maps to FieldItem.Id = 1 => High)
                (2, 1, '5', 2),               -- Number
                (3, 1, 'Some details', 3);    -- Text
            ");
        }

        public static void SeedAttachmentTypes()
        {
            _connection?.Execute(@"
            INSERT INTO AttachmentType (Id, TypeValue, DisplayValue) VALUES
            (1, 'card', 'Trello cards'),
            (2, 'link', 'Links'),
            (3, 'file', 'Files');
            ");
        }

        public static void SeedAttachments()
        {
            _connection?.Execute(@"
            INSERT INTO Attachment (Id, CardId, AttachmentTypeId, AttachmentPath, AttachmentName, CreatedAt, CreatedBy, Size, IsCover, Thumbnail) VALUES
            (1, 1, 3, 'uploads/docs/file1.pdf', 'Project Plan', datetime('now', '-1 day'), 1, '200KB', 0, 'thumb1.png'),
            (2, 1, 2, 'https://example.com', 'Reference Link', datetime('now', '-2 day'), 2, NULL, 0, NULL),
            (3, 1, 1, NULL, 'Trello Card Ref', datetime('now', '-3 day'), 3, NULL, 1, NULL);
            ");
        }



        public static void SeedAllData()
        {
            SeedUsers();
            SeedWorkspaceTypes();
            SeedWorkspaces();
            SeedOwnerTypes();

            SeedBoards();
            SeedUserStarredBoards();
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

        public static void ClearData()
        {
            using var cmd = _connection?.CreateCommand();

            // Delete all data in table 
            if (cmd == null) return;
            cmd.CommandText = @"
                DELETE FROM Attachment;
                DELETE FROM CheckListItem;
                DELETE FROM CheckList;
                DELETE FROM Comment;
                DELETE FROM Cards;
                DELETE FROM Stage;
                DELETE FROM Color;
                DELETE FROM UserViewHistory;
                DELETE FROM UserStarredBoard;
                DELETE FROM Members;
                DELETE FROM Board;
                DELETE FROM Workspace;
                DELETE FROM WorkspaceType;
                DELETE FROM OwnerType;
                DELETE FROM [User];
            ";
            cmd.ExecuteNonQuery();
        }

        public static SqliteConnection? GetInMemoryDatabaseConnection()
        {
            CreateInMemoryDatabaseAndSchema();
            SeedAllData();
            return _connection;
        }
    }
}
