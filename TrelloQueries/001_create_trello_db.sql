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
    Value [varchar](50) NULL   
);
GO

CREATE TABLE Activities (
    Id int IDENTITY(1,1) PRIMARY KEY,
    CreatedAt [datetime] NULL,
    Description [text] NULL,
    UserId [int] NULL FOREIGN KEY REFERENCES Users(Id),
    OwnerTypeId [int] NULL FOREIGN KEY REFERENCES OwnerTypes(Id),
    OwnerId [int] NULL    
);
GO

CREATE TABLE Workspaces (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Name [varchar](255) NULL,
    Description [text] NULL,
    Type [varchar](100) NULL,
    CreatedAt [datetime] NULL,
    CreatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
    LogoUrl [varchar](500) NULL
);
GO

CREATE TABLE Boards (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Name [varchar](255) NULL,
    Description [text] NULL,
    CreatedAt [datetime] NULL,
    CreatedBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
    AccessedAt [datetime] NULL,
    IsStar [bit] NULL,
    BackgroundUrl [varchar](2000) NULL,
    WorkspaceId [int] NULL FOREIGN KEY REFERENCES Workspaces(Id),
    Status [varchar](50) NULL    
);
GO

CREATE TABLE Colors (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Name [text] NULL,
    Icon [text] NULL,    
);
GO

CREATE TABLE Stages (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Title [varchar](255) NULL,
    CreatedAt [datetime] NULL,
    BoardId [int] NULL FOREIGN KEY REFERENCES Boards(Id),
    Status [varchar](20) NULL,
    ColorId [int] NULL FOREIGN KEY REFERENCES Colors(Id),
    Position [int] NULL   
);
GO

CREATE TABLE Cards (
    Id int IDENTITY(1,1) PRIMARY KEY,
    StageId [int] NULL FOREIGN KEY REFERENCES Stages(Id),
    Title [varchar](255) NULL,
    Description [text] NULL,
    CreatedAt [datetime] NULL,
    CreatedBy [int] NULL,
    Status [varchar](20) NULL,
    Location [varchar](255) NULL,
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
    Name [varchar](255) NULL,
    UploadAt [datetime] NULL,
    UploadBy [int] NULL FOREIGN KEY REFERENCES Users(Id),
    IsCover [bit] NULL    
);
GO

CREATE TABLE SettingOptions (
    Id int IDENTITY(1,1) PRIMARY KEY,
    DisplayValue [varchar](255) NULL,
    Value [varchar](50) NULL   
);
GO

CREATE TABLE BillingContacts (
    Id int IDENTITY(1,1) PRIMARY KEY,
    UserId [int] NULL FOREIGN KEY REFERENCES Users(Id),
    WorkspaceId [int] NULL FOREIGN KEY REFERENCES Workspaces(Id),
    Name [varchar](255) NULL,
    Email [varchar](255) NULL,
    Language [int] NULL FOREIGN KEY REFERENCES SettingOptions(Id),
    AdditionalInvoiceDetail [varchar](250) NULL 
);
GO

CREATE TABLE BillingPlans (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Name [varchar](100) NULL,
    Description [varchar](1000) NULL,
    Type [varchar](50) NULL,
    PricePerUser [decimal](10, 2) NULL,
    Status [varchar](50) NULL  
);
GO

CREATE TABLE Collections (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Name [varchar](255) NULL,
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
    Name [varchar](50) NULL    
);
GO

CREATE TABLE PowerUps (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Name [varchar](50) NULL,
    IconUrl [varchar](2000) NULL,
    BackgroundUrl [varchar](2000) NULL,
    AuthorName [varchar](50) NULL,
    Description [text] NULL,
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
    Name [varchar](50) NULL,
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
    Name [varchar](255) NULL,
    CardId [int] NULL FOREIGN KEY REFERENCES Cards(Id),
    Position [int] NULL,    
);
GO

CREATE TABLE Permissions (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Name [varchar](50) NULL,
    Code [varchar](50) NULL    
);
GO

CREATE TABLE Members (
    Id int IDENTITY(1,1) PRIMARY KEY,
    UserId [int] NULL FOREIGN KEY REFERENCES Users(Id),
    PermissionId [int] NULL FOREIGN KEY REFERENCES Permissions(Id),
    OwnerTypeId [int] NULL FOREIGN KEY REFERENCES OwnerTypes(Id),
    OwnerId [int] NULL,
    InvitedBy [int] NULL,
    JoinedAt [datetime] NULL,
    Status [varchar](50) NULL    
);
GO

CREATE TABLE CheckListItems (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Name [varchar](255) NULL,
    MemberId [int] NULL FOREIGN KEY REFERENCES Members(Id),
    CheckListId [int] NULL FOREIGN KEY REFERENCES CheckLists(Id),
    DueDate [date] NULL,
    Status [bit] NULL,
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
    Value [varchar](50) NULL,
    Priority [int] NULL,
    CustomFieldId [int] NULL FOREIGN KEY REFERENCES CustomFields(Id)    
);
GO

CREATE TABLE FieldValues (
    Id int IDENTITY(1,1) PRIMARY KEY,
    CardId [int] NULL FOREIGN KEY REFERENCES Cards(Id),
    Value [varchar](255) NULL,
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
    Status [varchar](50) NULL    
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
    Description [text] NULL,
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
    Value [int] NULL,
    OwnerId [int] NULL    
);
GO

CREATE TABLE ShareLinks (
    Id int IDENTITY(1,1) PRIMARY KEY,
    OwnerTypeId [int] NULL FOREIGN KEY REFERENCES OwnerTypes(Id),
    OwnerId [int] NULL,
    PermissionId [int] NULL FOREIGN KEY REFERENCES Permissions(Id),
    Token [varchar](255) NULL,
    Status [varchar](50) NULL   
);
GO

CREATE TABLE Subscriptions (
    Id int IDENTITY(1,1) PRIMARY KEY,
    BillingId [int] NULL FOREIGN KEY REFERENCES BillingContacts(Id),
    BillingPlanId [int] NULL FOREIGN KEY REFERENCES BillingPlans(Id),
    StartDate [date] NULL,
    EndDate [date] NULL,
    BillingCycle [varchar](20) NULL,
    Status [varchar](50) NULL,
    AutoRenew [bit] NULL,
    MemberCountBilled [int] NULL    
);
GO

CREATE TABLE TemplateCategories (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Name [varchar](100) NULL,
    IconUrl [varchar](2000) NULL    
);
GO

CREATE TABLE Templates (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Title [varchar](255) NULL,
    Description [text] NULL,
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