CREATE DATABASE Trello;
GO

USE [Trello];
GO


CREATE TABLE Users (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Username [varchar](255) NULL,
    Bio [nvarchar](1000) NULL,
    Email [varchar](255) NULL,
    LastActive [datetime] NULL,
    CreatedAt [datetime] NULL,
    UpdatedAt [datetime] NULL,
    PictureUrl [varchar](255) NULL    
);
GO

CREATE TABLE [dbo].[DataTypes](
    [Id] [int] IDENTITY(1,1) PRIMARY KEY,
    [TypeValue] [varchar](20) NULL,
    Icon [varchar](255) NOT NULL
);
GO

CREATE TABLE OwnerTypes (
    Id int IDENTITY(1,1) PRIMARY KEY,
    OwnerTypeValue [varchar](50) NULL   
);
GO

CREATE TABLE Activities (
    Id int IDENTITY(1,1) PRIMARY KEY,
    CreatedAt [datetime] NULL,
    ActivityDescription [nvarchar](1000) NULL,
    UserId [int] NULL FOREIGN KEY REFERENCES Users(Id),
    OwnerTypeId [int] NULL FOREIGN KEY REFERENCES OwnerTypes(Id),
    OwnerId [int] NULL    
);
GO

CREATE TABLE Workspaces (
    Id int IDENTITY(1,1) PRIMARY KEY,
    WorkspaceName [varchar](255) NULL,
    WorkspaceDescription [nvarchar](1000) NULL,
    WorkspaceType [varchar](100) NULL,
    CreatedAt [datetime] NULL,
    CreatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
    UpdatedAt [datetime] NULL,
    UpdatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
    LogoUrl [varchar](500) NULL
);
GO

CREATE TABLE Boards (
    Id int IDENTITY(1,1) PRIMARY KEY,
    BoardName [varchar](255) NULL,
    BoardDescription [nvarchar](1000) NULL,
    CreatedAt [datetime] NULL,
    CreatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
    AccessedAt [datetime] NULL,
    IsStar [bit] NULL,
    BackgroundUrl [varchar](2000) NULL,
    WorkspaceId [int] NULL FOREIGN KEY REFERENCES Workspaces(Id),
    BoardStatus [varchar](50) NULL,    
    UpdatedAt [datetime] NULL,
    UpdatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id)
);
GO

CREATE TABLE Colors (
    Id int IDENTITY(1,1) PRIMARY KEY,
    ColorName [varchar](50) NULL,
    ColorHex [varchar](7) NULL,
    Icon [varchar](255) NULL   
);
GO

CREATE TABLE Stages (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Title [varchar](255) NULL,
    CreatedAt [datetime] NULL,
    CreatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
    BoardId [int] NULL FOREIGN KEY REFERENCES Boards(Id),
    StageStatus [varchar](20) NULL,
    ColorId [int] NULL FOREIGN KEY REFERENCES Colors(Id),
    Position [int] NULL,   
    UpdatedAt [datetime] NULL,
    UpdatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id)
);
GO

CREATE TABLE Cards (
    Id int IDENTITY(1,1) PRIMARY KEY,
    StageId [int] NULL FOREIGN KEY REFERENCES Stages(Id),
    Title [varchar](255) NULL,
    CardDescription [nvarchar](1000) NULL,
    CreatedAt [datetime] NULL,
    CreatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
    CardStatus [varchar](20) NULL,
    CardLocation [varchar](255) NULL,
    StartDate [date] NULL,
    DueDate [date] NULL,
    CoverType [varchar](50) NULL,
    CoverValue [varchar](2000) NULL,
    Position [int] NULL,    
    UpdatedAt [datetime] NULL,
    UpdatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id)
);
GO

CREATE TABLE Attachments (
    Id int IDENTITY(1,1) PRIMARY KEY,
    CardId [int] NULL FOREIGN KEY REFERENCES Cards(Id),
    FileType [varchar](50) NULL,
    AttachmentPath [varchar](255) NULL, --both link and file path store here
    AttachmentName [varchar](255) NULL,
    UpdatedAt [datetime] NULL,
    UpdatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
    IsCover [bit] NULL    
);
GO

CREATE TABLE SettingOptions (
    Id int IDENTITY(1,1) PRIMARY KEY,
    DisplayValue [varchar](255) NULL,
    SettingOptionValue [varchar](50) NULL   
);
GO

CREATE TABLE BillingContacts (
    Id int IDENTITY(1,1) PRIMARY KEY,
    UserId [int] NULL FOREIGN KEY REFERENCES Users(Id),
    WorkspaceId [int] NULL FOREIGN KEY REFERENCES Workspaces(Id),
    BillingContactName [varchar](50) NULL,
    BillingContactEmail [varchar](100) NULL,
    BillingLanguage [int] NULL FOREIGN KEY REFERENCES SettingOptions(Id),
    AdditionalInvoiceDetail [varchar](250) NULL 
);
GO

CREATE TABLE BillingPlans (
    Id int IDENTITY(1,1) PRIMARY KEY,
    PlanName [varchar](100) NULL,
    BillingPlanDescription [varchar](1000) NULL,
    BIllingPlanType [varchar](50) NULL,
    PricePerUser [decimal](10, 2) NULL,
    BillingPlanStatus [varchar](50) NULL  
);
GO

CREATE TABLE Collections (
    Id int IDENTITY(1,1) PRIMARY KEY,
    CollectionName [varchar](255) NULL,
    CreatedAt [datetime] NULL,
    CreatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
    UpdatedAt [datetime] NULL,
    UpdatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
    WorkspaceId [int] NULL FOREIGN KEY REFERENCES Workspaces(Id),    
);
GO

CREATE TABLE BoardCollections (
    BoardId [int] NOT NULL FOREIGN KEY REFERENCES Boards(Id),
    CollectionId [int] NOT NULL FOREIGN KEY REFERENCES Collections(Id)
);
GO

CREATE TABLE PowerUpCategories (
    Id int IDENTITY(1,1) PRIMARY KEY,
    PowerUpCategoryName [varchar](50) NULL    
);
GO

CREATE TABLE PowerUps (
    Id int IDENTITY(1,1) PRIMARY KEY,
    PowerUpName [varchar](50) NULL,
    IconUrl [varchar](2000) NULL,
    BackgroundUrl [varchar](2000) NULL,
    AuthorName [varchar](50) NULL,
    PowerUpDescription [nvarchar](1000) NULL,
    EmailContact [varchar](50) NULL,
    PolicyUrl [varchar](2000) NULL,
    IsStaffPick [bit] NULL,
    IsIntegration [bit] NULL,
    PowerUpCategoryId [int] NULL FOREIGN KEY REFERENCES PowerUpCategories(Id)    
);
GO

CREATE TABLE BoardPowerUps (
    BoardId [int] NOT NULL FOREIGN KEY REFERENCES Boards(Id),
    PowerUpId [int] NOT NULL FOREIGN KEY REFERENCES PowerUps(Id),
    BoardPowerUpStatus [bit] NOT NULL
);
GO

CREATE TABLE UserViewHistory (
    UserId [int] NOT NULL FOREIGN KEY REFERENCES Users(Id),
    OwnerTypeId [int] NOT NULL FOREIGN KEY REFERENCES OwnerTypes(Id),
    OwnerId [int] NOT NULL,
    AccessedAt [datetime] NULL
);
GO

CREATE TABLE Labels (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Title [varchar](100) NULL,
    CreatedAt [datetime] NULL,
    CreatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
    UpdatedAt [datetime] NULL,
    UpdatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
    ColorId [int] NULL FOREIGN KEY REFERENCES Colors(Id),
    IsDefault [bit] NOT NULL,
    BoardId [int] FOREIGN KEY REFERENCES Boards(Id)
);
GO

CREATE TABLE CardLabels (
    CardId [int] NOT NULL FOREIGN KEY REFERENCES Cards(Id),
    LabelId [int] NOT NULL FOREIGN KEY REFERENCES Labels(Id)
);
GO


CREATE TABLE StickerCategories(
    Id int IDENTITY(1,1) PRIMARY KEY,
    CategoryName [varchar](100) NOT NULL,
    Position int NOT NULL
)

CREATE TABLE Stickers (
    Id int IDENTITY(1,1) PRIMARY KEY,
    StickerCategoryId [int] NULL FOREIGN KEY REFERENCES StickerCategories(Id),
    StickerName [varchar](50) NULL,
    StickerUrl [varchar](2000) NULL,
    CreatedAt [datetime] NULL,
    CreatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id)
);
GO

CREATE TABLE CardStickers (
    CardId [int] NOT NULL FOREIGN KEY REFERENCES Cards(Id),
    StickerId [int] NOT NULL FOREIGN KEY REFERENCES Stickers(Id),
    CreatedAt [datetime] NULL,
    CreatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
    PositionX [float] NULL,
    PositionY [float] NULL,
    IndexZ [int] NULL
);
GO

CREATE TABLE CheckLists (
    Id int IDENTITY(1,1) PRIMARY KEY,
    CheckListName [varchar](255) NULL,
    CardId [int] NULL FOREIGN KEY REFERENCES Cards(Id),
    Position [int] NULL,
    CreatedAt [datetime] NULL,
    CreatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
    UpdatedAt [datetime] NULL,
    UpdatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
);
GO

CREATE TABLE RolePermissions (
    Id int IDENTITY(1,1) PRIMARY KEY,
    PermissionName [varchar](50) NULL,
    PermissionCode [varchar](50) NULL    
);
GO

CREATE TABLE Members (
    Id int IDENTITY(1,1) PRIMARY KEY,
    UserId [int] NULL FOREIGN KEY REFERENCES Users(Id),
    PermissionId [int] NULL FOREIGN KEY REFERENCES RolePermissions(Id),
    OwnerTypeId [int] NULL FOREIGN KEY REFERENCES OwnerTypes(Id),
    OwnerId [int] NULL,
    InvitedBy [int] NULL,
    JoinedAt [datetime] NULL,
    MemberStatus [varchar](50) NULL    
);
GO

CREATE TABLE CheckListItems (
    Id int IDENTITY(1,1) PRIMARY KEY,
    CheckListItemName [varchar](255) NULL,
    MemberId [int] NULL FOREIGN KEY REFERENCES Members(Id),
    CheckListId [int] NULL FOREIGN KEY REFERENCES CheckLists(Id),
    DueDate [date] NULL,
    CheckListItemStatus [bit] NULL,
    CreatedAt [datetime] NULL,
    CreatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
    UpdatedAt [datetime] NULL,
    UpdatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
    Position [int] NULL,    
);
GO

CREATE TABLE Comments (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Content [nvarchar](1000) NULL,
    CardId [int] NULL FOREIGN KEY REFERENCES Cards(Id),
    CreatedAt [datetime] NULL,
    CreatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
    UpdatedAt [datetime] NULL,
    UpdatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
);
GO

CREATE TABLE ReactionCategories (
    Id int IDENTITY(1,1) PRIMARY KEY,
    CategoryName [varchar](50) NOT NULL,      -- e.g. Food & Drink
    Icon [varchar](255) NULL,  
    Position [int] NOT NULL,
    IsActive [bit] NOT NULL,
);


CREATE TABLE Reactions (
    Id int IDENTITY(1,1) PRIMARY KEY,
    ReactionsName [varchar](255) NULL, 
    ShortCode [varchar](50) NOT NULL, -- e.g. :neutral_face:
    ReactionCategoryId [int] FOREIGN KEY REFERENCES ReactionCategories(Id),
    Icon [varchar](255) NULL    
);
GO

CREATE TABLE CommentReactions (
    CommentId [int] NOT NULL FOREIGN KEY REFERENCES Comments(Id),
    ReactionId [int] NOT NULL FOREIGN KEY REFERENCES Reactions(Id),
    CreatedBy [int] NOT NULL FOREIGN KEY REFERENCES Users(Id),
    CreatedAt [datetime] NULL
);
GO

CREATE TABLE CustomFields (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Title [varchar](255) NULL,
    DataTypeId [int] NULL FOREIGN KEY REFERENCES DataTypes(Id),
    BoardId [int] NULL FOREIGN KEY REFERENCES Boards(Id),
    CreatedAt [datetime] NULL,
    CreatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
    UpdatedAt [datetime] NULL,
    UpdatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
    Position [int] NULL,
    IsFrontCardShowed [bit] NOT NULL
);
GO

CREATE TABLE Exports (
    Id int IDENTITY(1,1) PRIMARY KEY,
    WorkspaceId [int] NULL FOREIGN KEY REFERENCES Workspaces(Id),
    CreatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
    CreatedAt [datetime] NULL,
    Size [int] NULL    
);
GO

CREATE TABLE FieldItems (
    Id int IDENTITY(1,1) PRIMARY KEY,
    ColorId [int] NULL FOREIGN KEY REFERENCES Colors(Id),
    FieldItemValue [varchar](50) NULL,
    Position [int] NULL,
    CustomFieldId [int] NULL FOREIGN KEY REFERENCES CustomFields(Id)    
);
GO

CREATE TABLE FieldValues (
    Id int IDENTITY(1,1) PRIMARY KEY,
    CardId [int] NULL FOREIGN KEY REFERENCES Cards(Id),
    FieldValue [varchar](255) NULL,
    CustomFieldId [int] NULL FOREIGN KEY REFERENCES CustomFields(Id)    
);
GO

CREATE TABLE Notifications (
    Id int IDENTITY(1,1) PRIMARY KEY,
    ActivityId [int] NULL FOREIGN KEY REFERENCES Activities(Id),
    NotificationStatus [varchar](50) NULL    
);
GO

CREATE TABLE PaymentInformations (
    Id int IDENTITY(1,1) PRIMARY KEY,
    BillingContactId [int] NULL FOREIGN KEY REFERENCES BillingContacts(Id),
    CardNumber [varchar](20) NULL,
    CardBrand [varchar](50) NULL,
    ExpirationDate [date] NULL,
    Cvv [varchar](10) NULL,
    Country [varchar](100) NULL,
    PostalCode [varchar](20) NULL   
);
GO

CREATE TABLE SettingKeys (
    Id int IDENTITY(1,1) PRIMARY KEY,
    KeyName [varchar](100) NULL,
    SettingKeyDescription [nvarchar](1000) NULL,
    OwnerTypeId [int] NULL FOREIGN KEY REFERENCES OwnerTypes(Id),
    DefaultValue [int] NULL,
    DataTypeId [int] NULL   
);
GO

CREATE TABLE SettingKeySettingOptions (
    SettingKeyId [int] NOT NULL FOREIGN KEY REFERENCES SettingKeys(Id),
    SettingOptionId [int] NOT NULL FOREIGN KEY REFERENCES SettingOptions(Id)
);
GO

CREATE TABLE SettingValues (
    Id int IDENTITY(1,1) PRIMARY KEY,
    SettingKeyId [int] NULL FOREIGN KEY REFERENCES SettingKeys(Id),
    SettingValue [int] NULL,
    CreatedAt [datetime] NULL,
    CreatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
    UpdatedAt [datetime] NULL,
    UpdatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
    OwnerId [int] NULL    
);
GO

CREATE TABLE ShareLinks (
    Id int IDENTITY(1,1) PRIMARY KEY,
    OwnerTypeId [int] NULL FOREIGN KEY REFERENCES OwnerTypes(Id),
    OwnerId [int] NULL,
    PermissionId [int] NULL FOREIGN KEY REFERENCES RolePermissions(Id),
    ShareLinkToken [varchar](255) NULL,
    ShareLinkStatus [bit] NOT NULL   
);
GO

CREATE TABLE Subscriptions (
    Id int IDENTITY(1,1) PRIMARY KEY,
    BillingContactId [int] NULL FOREIGN KEY REFERENCES BillingContacts(Id),
    BillingPlanId [int] NULL FOREIGN KEY REFERENCES BillingPlans(Id),
    StartDate [date] NULL,
    EndDate [date] NULL,
    BillingCycle [varchar](20) NULL,
    SubscriptionStatus [varchar](50) NULL,
    AutoRenew [bit] NULL,
    MemberCountBilled [int] NULL    
);
GO

CREATE TABLE TemplateCategories (
    Id int IDENTITY(1,1) PRIMARY KEY,
    TemplateCategoryName [varchar](100) NULL,
    IconUrl [varchar](2000) NULL    
);
GO

CREATE TABLE Templates (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Title [varchar](255) NULL,
    TemplateDescription [nvarchar](1000) NULL,
    TemplateCategoryId [int] NULL FOREIGN KEY REFERENCES TemplateCategories(Id),
    Viewed [int] NULL,
    Copied [int] NULL,
    CreatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
    CreatedAt [datetime] NULL,
    UpdatedAt [datetime] NULL,
    UpdatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
    BoardId [int] NULL FOREIGN KEY REFERENCES Boards(Id),
    BackgroundUrl [varchar](255) NULL    
);
GO

CREATE TABLE WorkspaceMembershipDomains (
    Id int IDENTITY(1,1) PRIMARY KEY,
    WorkspaceId [int] NOT NULL FOREIGN KEY REFERENCES Workspaces(Id),
    Domain [nvarchar](1000) NOT NULL,
    CreatedAt [datetime] NULL    
);
GO