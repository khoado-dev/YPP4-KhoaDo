CREATE TABLE Users (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Username [varchar](255) NULL,
    Bio [text] NULL,
    Email [varchar](255) NULL,
    LastActive [datetime] NULL,
    CreatedAt [datetime] NULL,
    PictureUrl [varchar](2000) NULL    
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
    ActivityDescription [text] NULL,
    UserId [int] NULL FOREIGN KEY REFERENCES Users(Id),
    OwnerTypeId [int] NULL FOREIGN KEY REFERENCES OwnerTypes(Id),
    OwnerId [int] NULL    
);
GO

CREATE TABLE Workspaces (
    Id int IDENTITY(1,1) PRIMARY KEY,
    WorkspaceName [varchar](255) NULL,
    WorkspaceDescription [text] NULL,
    WorkspaceType [varchar](100) NULL,
    CreatedAt [datetime] NULL,
    CreatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
    LogoUrl [varchar](500) NULL
);
GO

CREATE TABLE Boards (
    Id int IDENTITY(1,1) PRIMARY KEY,
    BoardName [varchar](255) NULL,
    BoardDescription [text] NULL,
    CreatedAt [datetime] NULL,
    CreatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
    AccessedAt [datetime] NULL,
    IsStar [bit] NULL,
    BackgroundUrl [varchar](2000) NULL,
    WorkspaceId [int] NULL FOREIGN KEY REFERENCES Workspaces(Id),
    BoardStatus [varchar](50) NULL    
);
GO

CREATE TABLE Colors (
    Id int IDENTITY(1,1) PRIMARY KEY,
    ColorName [text] NULL,
    Icon [text] NULL,    
);
GO

CREATE TABLE Stages (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Title [varchar](255) NULL,
    CreatedAt [datetime] NULL,
    BoardId [int] NULL FOREIGN KEY REFERENCES Boards(Id),
    StageStatus [varchar](20) NULL,
    ColorId [int] NULL FOREIGN KEY REFERENCES Colors(Id),
    Position [int] NULL   
);
GO

CREATE TABLE Cards (
    Id int IDENTITY(1,1) PRIMARY KEY,
    StageId [int] NULL FOREIGN KEY REFERENCES Stages(Id),
    Title [varchar](255) NULL,
    CardDescription [text] NULL,
    CreatedAt [datetime] NULL,
    CreatedBy [int] NULL,
    CardStatus [varchar](20) NULL,
    CardLocation [varchar](255) NULL,
    StartDate [date] NULL,
    DueDate [date] NULL,
    CoverType [varchar](50) NULL,
    CoverValue [varchar](2000) NULL,
    Position [int] NULL    
);
GO

CREATE TABLE Attachments (
    Id int IDENTITY(1,1) PRIMARY KEY,
    CardId [int] NULL FOREIGN KEY REFERENCES Cards(Id),
    Link [varchar](255) NULL,
    FileType [varchar](50) NULL,
    FilePath [varchar](255) NULL,
    AttachmentName [varchar](255) NULL,
    UploadAt [datetime] NULL,
    UploadBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
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
    BillingContactName [varchar](255) NULL,
    Email [varchar](255) NULL,
    BillingContactLanguage [int] NULL FOREIGN KEY REFERENCES SettingOptions(Id),
    AdditionalInvoiceDetail [varchar](250) NULL 
);
GO

CREATE TABLE BillingPlans (
    Id int IDENTITY(1,1) PRIMARY KEY,
    BillingPlanName [varchar](100) NULL,
    BillingPlanDescription [varchar](1000) NULL,
    BillingPlanType [varchar](50) NULL,
    PricePerUser [decimal](10, 2) NULL,
    BillingPlanStatus [varchar](50) NULL  
);
GO

CREATE TABLE Collections (
    Id int IDENTITY(1,1) PRIMARY KEY,
    CollectionName [varchar](255) NULL,
    CreatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
    CreatedAt [datetime] NULL,
    WorkspaceId [int] NULL,    
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
    PowerUpDescription [text] NULL,
    EmailContact [varchar](50) NULL,
    PolicyUrl [varchar](2000) NULL,
    IsStaffPick [bit] NULL,
    IsIntegration [bit] NULL,
    PowerUpCategoryId [int] NULL FOREIGN KEY REFERENCES PowerUpCategories(Id)    
);
GO

CREATE TABLE BoardPowerUps (
    BoardId [int] NOT NULL FOREIGN KEY REFERENCES Boards(Id),
    PowerUpId [int] NOT NULL FOREIGN KEY REFERENCES PowerUps(Id)
);
GO

CREATE TABLE BoardUsers (
    BoardId [int] NOT NULL FOREIGN KEY REFERENCES Boards(Id),
    UserId [int] NOT NULL FOREIGN KEY REFERENCES Users(Id),
    AccessedAt [datetime] NULL
);
GO

CREATE TABLE Labels (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Title [varchar](100) NULL,
    ColorId [int] NULL FOREIGN KEY REFERENCES Colors(Id)    
);
GO

CREATE TABLE CardLabels (
    CardId [int] NOT NULL FOREIGN KEY REFERENCES Cards(Id),
    LabelId [int] NOT NULL FOREIGN KEY REFERENCES Labels(Id)
);
GO

CREATE TABLE Stickers (
    Id int IDENTITY(1,1) PRIMARY KEY,
    StickerName [varchar](50) NULL,
    StickerUrl [varchar](2000) NULL    
);
GO

CREATE TABLE CardStickers (
    CardId [int] NOT NULL FOREIGN KEY REFERENCES Cards(Id),
    StickerId [int] NOT NULL FOREIGN KEY REFERENCES Stickers(Id),
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
);
GO

CREATE TABLE RolePermissions (
    Id int IDENTITY(1,1) PRIMARY KEY,
    RolePermissionName [varchar](50) NULL,
    Code [varchar](50) NULL    
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
    Position [int] NULL,    
);
GO

CREATE TABLE Comments (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Content [text] NULL,
    CardId [int] NULL FOREIGN KEY REFERENCES Cards(Id),
    CreatedAt [datetime] NULL,
    CreatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),    
);
GO

CREATE TABLE Reactions (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Icon [varchar](50) NULL    
);
GO

CREATE TABLE CommentReactions (
    CommentId [int] NOT NULL FOREIGN KEY REFERENCES Comments(Id),
    ReactionId [int] NOT NULL FOREIGN KEY REFERENCES Reactions(Id),
    CreatedBy [int] NOT NULL,
    CreatedAt [datetime] NULL
);
GO

CREATE TABLE CustomFields (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Title [varchar](255) NULL,
    FieldType [varchar](50) NULL,
    BoardId [int] NULL FOREIGN KEY REFERENCES Boards(Id),
    Position [int] NULL    
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
    FieldItemPriority [int] NULL,
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

CREATE TABLE MemberReactions (
    MemberId [int] NOT NULL FOREIGN KEY REFERENCES Members(Id),
    ReactionId [int] NOT NULL FOREIGN KEY REFERENCES Reactions(Id)
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
    BillingId [int] NULL FOREIGN KEY REFERENCES BillingContacts(Id),
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
    SettingKeyDescription [text] NULL,
    OwnerTypeId [int] NULL FOREIGN KEY REFERENCES OwnerTypes(Id),
    DefaultValue [int] NULL,
    TypeValue [varchar](50) NULL    
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
    OwnerId [int] NULL    
);
GO

CREATE TABLE ShareLinks (
    Id int IDENTITY(1,1) PRIMARY KEY,
    OwnerTypeId [int] NULL FOREIGN KEY REFERENCES OwnerTypes(Id),
    OwnerId [int] NULL,
    PermissionId [int] NULL FOREIGN KEY REFERENCES RolePermissions(Id),
    Token [varchar](255) NULL,
    ShareLinkStatus [varchar](50) NULL   
);
GO

CREATE TABLE Subscriptions (
    Id int IDENTITY(1,1) PRIMARY KEY,
    BillingId [int] NULL FOREIGN KEY REFERENCES BillingContacts(Id),
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
    TemplateDescription [text] NULL,
    TemplateCategoryId [int] NULL FOREIGN KEY REFERENCES TemplateCategories(Id),
    Viewed [int] NULL,
    Copied [int] NULL,
    CreatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
    CreatedAt [datetime] NULL,
    BoardId [int] NULL FOREIGN KEY REFERENCES Boards(Id),
    BackgroundUrl [varchar](2000) NULL    
);
GO

CREATE TABLE WorkspaceMembershipDomains (
    Id int IDENTITY(1,1) PRIMARY KEY,
    WorkspaceId [int] NOT NULL FOREIGN KEY REFERENCES Workspaces(Id),
    Domain [text] NOT NULL,
    CreatedAt [datetime] NULL    
);
GO