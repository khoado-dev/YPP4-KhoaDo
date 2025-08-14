USE master;
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'Trello')
BEGIN
    ALTER DATABASE Trello SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE Trello;
END;
GO

CREATE DATABASE Trello;
GO

USE [Trello]
GO

-- Users table
CREATE TABLE [dbo].[Users](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Username] [varchar](255) NULL,
    [Bio] [nvarchar](1000) NULL,
    [Email] [varchar](255) NULL,
    [LastActive] [datetime] NULL,
    [CreatedAt] [datetime] NULL,
    [UpdatedAt] [datetime] NULL,
    [PictureUrl] [varchar](255) NULL,
    [FullName] [nchar](100) NULL,
    PRIMARY KEY ([Id])
)

-- WorkspaceType table
CREATE TABLE [dbo].[WorkspaceType](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [TypeValue] [varchar](50) NOT NULL,
    [DisplayValue] [varchar](50) NOT NULL,
    PRIMARY KEY ([Id])
)

-- Workspace table
CREATE TABLE [dbo].[Workspace](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [WorkspaceName] [varchar](255) NULL,
    [WorkspaceDescription] [nvarchar](1000) NULL,
    [ShortName] [varchar](100) NULL,
    [Website] [varchar](255) NULL,
    [TypeId] [int] NULL,
    [CreatedAt] [datetime] NULL,
    [CreatedBy] [int] NULL,
    [UpdatedAt] [datetime] NULL,
    [UpdatedBy] [int] NULL,
    [LogoUrl] [varchar](500) NULL,
    PRIMARY KEY ([Id]),
    FOREIGN KEY([TypeId]) REFERENCES [dbo].[WorkspaceType]([Id]),
    FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[Users]([Id]),
    FOREIGN KEY([UpdatedBy]) REFERENCES [dbo].[Users]([Id])
)

-- Board table
CREATE TABLE [dbo].[Board](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [BoardName] [varchar](255) NULL,
    [BoardDescription] [nvarchar](1000) NULL,
    [CreatedAt] [datetime] NULL,
    [CreatedBy] [int] NULL,
    [BackgroundUrl] [varchar](2000) NULL,
    [WorkspaceId] [int] NULL,
    [BoardStatus] [varchar](50) NULL,
    [UpdatedAt] [datetime] NULL,
    [UpdatedBy] [int] NULL,
    [IsTemplate] [bit] NULL,
    PRIMARY KEY ([Id]),
    FOREIGN KEY([WorkspaceId]) REFERENCES [dbo].[Workspace]([Id]),
    FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[Users]([Id]),
    FOREIGN KEY([UpdatedBy]) REFERENCES [dbo].[Users]([Id])
)

-- Color table
CREATE TABLE [dbo].[Color](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [ColorName] [varchar](50) NULL,
    [ColorHex] [varchar](7) NULL,
    [Icon] [varchar](255) NULL,
    PRIMARY KEY ([Id])
)

-- Stage table
CREATE TABLE [dbo].[Stage](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Title] [varchar](255) NULL,
    [CreatedAt] [datetime] NULL,
    [CreatedBy] [int] NULL,
    [BoardId] [int] NULL,
    [StageStatus] [varchar](20) NULL,
    [ColorId] [int] NULL,
    [Position] [int] NULL,
    [UpdatedAt] [datetime] NULL,
    [UpdatedBy] [int] NULL,
    PRIMARY KEY ([Id]),
    FOREIGN KEY([BoardId]) REFERENCES [dbo].[Board]([Id]),
    FOREIGN KEY([ColorId]) REFERENCES [dbo].[Color]([Id]),
    FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[Users]([Id]),
    FOREIGN KEY([UpdatedBy]) REFERENCES [dbo].[Users]([Id])
)

-- CardCoverType table
CREATE TABLE [dbo].[CardCoverType](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [TypeValue] [varchar](50) NOT NULL,
    [DisplayValue] [varchar](50) NOT NULL,
    PRIMARY KEY ([Id])
)

-- Cards table
CREATE TABLE [dbo].[Cards](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [StageId] [int] NULL,
    [Title] [varchar](255) NULL,
    [CardDescription] [nvarchar](1000) NULL,
    [CreatedAt] [datetime] NULL,
    [CreatedBy] [int] NULL,
    [CardStatus] [varchar](20) NULL,
    [CardLocation] [varchar](255) NULL,
    [StartDate] [date] NULL,
    [DueDate] [date] NULL,
    [CardCoverTypeId] [int] NULL,
    [CoverValue] [varchar](300) NULL,
    [Position] [int] NULL,
    [UpdatedAt] [datetime] NULL,
    [UpdatedBy] [int] NULL,
    [IsTemplate] [bit] NULL,
    [IsCompleted] [bit] NULL,
    PRIMARY KEY ([Id]),
    FOREIGN KEY([StageId]) REFERENCES [dbo].[Stage]([Id]),
    FOREIGN KEY([CardCoverTypeId]) REFERENCES [dbo].[CardCoverType]([Id]),
    FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[Users]([Id]),
    FOREIGN KEY([UpdatedBy]) REFERENCES [dbo].[Users]([Id])
)

-- Labels table
CREATE TABLE [dbo].[Labels](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Title] [varchar](100) NULL,
    [CreatedAt] [datetime] NULL,
    [CreatedBy] [int] NULL,
    [UpdatedAt] [datetime] NULL,
    [UpdatedBy] [int] NULL,
    [ColorId] [int] NULL,
    [IsDefault] [bit] NOT NULL,
    [BoardId] [int] NULL,
    PRIMARY KEY ([Id]),
    FOREIGN KEY([BoardId]) REFERENCES [dbo].[Board]([Id]),
    FOREIGN KEY([ColorId]) REFERENCES [dbo].[Color]([Id]),
    FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[Users]([Id]),
    FOREIGN KEY([UpdatedBy]) REFERENCES [dbo].[Users]([Id])
)

-- CardLabel junction table
CREATE TABLE [dbo].[CardLabel](
    [CardId] [int] NOT NULL,
    [LabelId] [int] NOT NULL,
    FOREIGN KEY([CardId]) REFERENCES [dbo].[Cards]([Id]),
    FOREIGN KEY([LabelId]) REFERENCES [dbo].[Labels]([Id])
)

-- AttachmentType table
CREATE TABLE [dbo].[AttachmentType](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [TypeValue] [varchar](50) NOT NULL,
    [DisplayValue] [varchar](50) NOT NULL,
    PRIMARY KEY ([Id])
)

-- Attachment table
CREATE TABLE [dbo].[Attachment](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [CardId] [int] NULL,
    [AttachmentTypeId] [int] NULL,
    [AttachmentPath] [varchar](255) NULL,
    [AttachmentName] [varchar](255) NULL,
    [CreatedAt] [datetime] NULL,
    [CreatedBy] [int] NULL,
    [Size] [varchar](255) NULL,
    [IsCover] [bit] NULL,
    [Thumbnail] [varchar](350) NULL,
    PRIMARY KEY ([Id]),
    FOREIGN KEY([CardId]) REFERENCES [dbo].[Cards]([Id]),
    FOREIGN KEY([AttachmentTypeId]) REFERENCES [dbo].[AttachmentType]([Id]),
    FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[Users]([Id])
)

-- CheckList table
CREATE TABLE [dbo].[CheckList](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [CheckListName] [varchar](255) NULL,
    [CardId] [int] NULL,
    [Position] [int] NULL,
    [CreatedAt] [datetime] NULL,
    [CreatedBy] [int] NULL,
    [UpdatedAt] [datetime] NULL,
    [UpdatedBy] [int] NULL,
    PRIMARY KEY ([Id]),
    FOREIGN KEY([CardId]) REFERENCES [dbo].[Cards]([Id]),
    FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[Users]([Id]),
    FOREIGN KEY([UpdatedBy]) REFERENCES [dbo].[Users]([Id])
)

-- RolePermission table
CREATE TABLE [dbo].[RolePermission](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [PermissionName] [varchar](50) NULL,
    [PermissionCode] [varchar](50) NULL,
    PRIMARY KEY ([Id])
)

-- OwnerType table
CREATE TABLE [dbo].[OwnerType](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [OwnerTypeValue] [varchar](50) NOT NULL,
    PRIMARY KEY ([Id])
)

-- Members table
CREATE TABLE [dbo].[Members](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [int] NULL,
    [RolePermissonId] [int] NULL,
    [OwnerTypeId] [int] NULL,
    [OwnerId] [int] NULL,
    [InvitedBy] [int] NULL,
    [JoinedAt] [datetime] NULL,
    [MemberStatus] [varchar](50) NULL,
    PRIMARY KEY ([Id]),
    FOREIGN KEY([UserId]) REFERENCES [dbo].[Users]([Id]),
    FOREIGN KEY([RolePermissonId]) REFERENCES [dbo].[RolePermission]([Id]),
    FOREIGN KEY([OwnerTypeId]) REFERENCES [dbo].[OwnerType]([Id])
)

-- CheckListItem table
CREATE TABLE [dbo].[CheckListItem](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [CheckListItemName] [varchar](255) NULL,
    [MemberId] [int] NULL,
    [CheckListId] [int] NULL,
    [DueDate] [date] NULL,
    [CheckListItemStatus] [bit] NULL,
    [CreatedAt] [datetime] NULL,
    [CreatedBy] [int] NULL,
    [UpdatedAt] [datetime] NULL,
    [UpdatedBy] [int] NULL,
    [Position] [int] NULL,
    PRIMARY KEY ([Id]),
    FOREIGN KEY([CheckListId]) REFERENCES [dbo].[CheckList]([Id]),
    FOREIGN KEY([MemberId]) REFERENCES [dbo].[Members]([Id]),
    FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[Users]([Id]),
    FOREIGN KEY([UpdatedBy]) REFERENCES [dbo].[Users]([Id])
)

-- Comment table
CREATE TABLE [dbo].[Comment](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Content] [nvarchar](1000) NULL,
    [CardId] [int] NULL,
    [CreatedAt] [datetime] NULL,
    [CreatedBy] [int] NULL,
    [UpdatedAt] [datetime] NULL,
    [UpdatedBy] [int] NULL,
    PRIMARY KEY ([Id]),
    FOREIGN KEY([CardId]) REFERENCES [dbo].[Cards]([Id]),
    FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[Users]([Id]),
    FOREIGN KEY([UpdatedBy]) REFERENCES [dbo].[Users]([Id])
)

-- DataType table
CREATE TABLE [dbo].[DataType](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [DataTypeValue] [varchar](50) NOT NULL,
    PRIMARY KEY ([Id])
)

-- CustomField table
CREATE TABLE [dbo].[CustomField](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Title] [varchar](255) NULL,
    [DataTypeId] [int] NULL,
    [BoardId] [int] NULL,
    [CreatedAt] [datetime] NULL,
    [CreatedBy] [int] NULL,
    [UpdatedAt] [datetime] NULL,
    [UpdatedBy] [int] NULL,
    [Position] [int] NULL,
    [IsFrontCardShowed] [bit] NOT NULL,
    PRIMARY KEY ([Id]),
    FOREIGN KEY([DataTypeId]) REFERENCES [dbo].[DataType]([Id]),
    FOREIGN KEY([BoardId]) REFERENCES [dbo].[Board]([Id]),
    FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[Users]([Id]),
    FOREIGN KEY([UpdatedBy]) REFERENCES [dbo].[Users]([Id])
)

-- FieldItem table
CREATE TABLE [dbo].[FieldItem](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [ColorId] [int] NULL,
    [FieldItemValue] [varchar](50) NULL,
    [Position] [int] NULL,
    [CustomFieldId] [int] NULL,
    PRIMARY KEY ([Id]),
    FOREIGN KEY([ColorId]) REFERENCES [dbo].[Color]([Id]),
    FOREIGN KEY([CustomFieldId]) REFERENCES [dbo].[CustomField]([Id])
)

-- FieldValue table
CREATE TABLE [dbo].[FieldValue](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [CardId] [int] NULL,
    [FieldValue] [varchar](255) NULL,
    [CustomFieldId] [int] NULL,
    PRIMARY KEY ([Id]),
    FOREIGN KEY([CardId]) REFERENCES [dbo].[Cards]([Id]),
    FOREIGN KEY([CustomFieldId]) REFERENCES [dbo].[CustomField]([Id])
)

-- Collections table
CREATE TABLE [dbo].[Collections](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [CollectionName] [varchar](255) NULL,
    [CreatedAt] [datetime] NULL,
    [CreatedBy] [int] NULL,
    [UpdatedAt] [datetime] NULL,
    [UpdatedBy] [int] NULL,
    [WorkspaceId] [int] NULL,
    PRIMARY KEY ([Id]),
    FOREIGN KEY([WorkspaceId]) REFERENCES [dbo].[Workspace]([Id]),
    FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[Users]([Id]),
    FOREIGN KEY([UpdatedBy]) REFERENCES [dbo].[Users]([Id])
)

-- BoardCollection junction table
CREATE TABLE [dbo].[BoardCollection](
    [BoardId] [int] NOT NULL,
    [CollectionId] [int] NOT NULL,
    FOREIGN KEY([BoardId]) REFERENCES [dbo].[Board]([Id]),
    FOREIGN KEY([CollectionId]) REFERENCES [dbo].[Collections]([Id])
)

-- PowerUpCategory table
CREATE TABLE [dbo].[PowerUpCategory](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [CategoryValue] [varchar](50) NOT NULL,
    [DisplayValue] [varchar](50) NOT NULL,
    PRIMARY KEY ([Id])
)

-- PowerUp table
CREATE TABLE [dbo].[PowerUp](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [PowerUpName] [varchar](50) NULL,
    [IconUrl] [varchar](2000) NULL,
    [BackgroundUrl] [varchar](2000) NULL,
    [AuthorName] [varchar](50) NULL,
    [PowerUpDescription] [nvarchar](1000) NULL,
    [EmailContact] [varchar](50) NULL,
    [PolicyUrl] [varchar](2000) NULL,
    [IsStaffPick] [bit] NULL,
    [IsIntegration] [bit] NULL,
    [CategoryId] [int] NULL,
    PRIMARY KEY ([Id]),
    FOREIGN KEY([CategoryId]) REFERENCES [dbo].[PowerUpCategory]([Id])
)

-- BoardPowerUp junction table
CREATE TABLE [dbo].[BoardPowerUp](
    [BoardId] [int] NOT NULL,
    [PowerUpId] [int] NOT NULL,
    [BoardPowerUpStatus] [bit] NOT NULL,
    FOREIGN KEY([BoardId]) REFERENCES [dbo].[Board]([Id]),
    FOREIGN KEY([PowerUpId]) REFERENCES [dbo].[PowerUp]([Id])
)

-- StickerCategory table
CREATE TABLE [dbo].[StickerCategory](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [CategoryValue] [varchar](50) NOT NULL,
    [DisplayValue] [varchar](50) NOT NULL,
    PRIMARY KEY ([Id])
)

-- Sticker table
CREATE TABLE [dbo].[Sticker](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [CategoryId] [int] NULL,
    [StickerName] [varchar](50) NULL,
    [StickerUrl] [varchar](2000) NULL,
    [CreatedAt] [datetime] NULL,
    [CreatedBy] [int] NULL,
    PRIMARY KEY ([Id]),
    FOREIGN KEY([CategoryId]) REFERENCES [dbo].[StickerCategory]([Id]),
    FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[Users]([Id])
)

-- CardSticker junction table
CREATE TABLE [dbo].[CardSticker](
    [CardId] [int] NOT NULL,
    [StickerId] [int] NOT NULL,
    [CreatedAt] [datetime] NULL,
    [CreatedBy] [int] NULL,
    [PositionX] [float] NULL,
    [PositionY] [float] NULL,
    [IndexZ] [int] NULL,
    FOREIGN KEY([CardId]) REFERENCES [dbo].[Cards]([Id]),
    FOREIGN KEY([StickerId]) REFERENCES [dbo].[Sticker]([Id]),
    FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[Users]([Id])
)

-- ReactionCategory table
CREATE TABLE [dbo].[ReactionCategory](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [CategoryValue] [varchar](50) NOT NULL,
    [DisplayValue] [varchar](50) NOT NULL,
    PRIMARY KEY ([Id])
)

-- Reaction table
CREATE TABLE [dbo].[Reaction](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [ReactionsName] [varchar](255) NULL,
    [ShortCode] [varchar](50) NOT NULL,
    [CategoryId] [int] NULL,
    [Icon] [varchar](255) NULL,
    PRIMARY KEY ([Id]),
    FOREIGN KEY([CategoryId]) REFERENCES [dbo].[ReactionCategory]([Id])
)

-- CommentReaction junction table
CREATE TABLE [dbo].[CommentReaction](
    [CommentId] [int] NOT NULL,
    [ReactionId] [int] NOT NULL,
    [CreatedBy] [int] NOT NULL,
    [CreatedAt] [datetime] NULL,
    FOREIGN KEY([CommentId]) REFERENCES [dbo].[Comment]([Id]),
    FOREIGN KEY([ReactionId]) REFERENCES [dbo].[Reaction]([Id]),
    FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[Users]([Id])
)

-- Activity table
CREATE TABLE [dbo].[Activity](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [CreatedAt] [datetime] NULL,
    [ActivityDescription] [nvarchar](1000) NULL,
    [UserId] [int] NULL,
    [OwnerTypeId] [int] NULL,
    [OwnerId] [int] NULL,
    PRIMARY KEY ([Id]),
    FOREIGN KEY([UserId]) REFERENCES [dbo].[Users]([Id])
)

-- Notification table
CREATE TABLE [dbo].[Notification](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [ActivityId] [int] NULL,
    [IsRead] [bit] NOT NULL,
    PRIMARY KEY ([Id]),
    FOREIGN KEY([ActivityId]) REFERENCES [dbo].[Activity]([Id])
)

-- ShareLink table
CREATE TABLE [dbo].[ShareLink](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [OwnerTypeId] [int] NULL,
    [RolePermissonId] [int] NULL,
    [OwnerId] [int] NULL,
    [ShareLinkToken] [varchar](255) NULL,
    [ShareLinkStatus] [bit] NOT NULL,
    PRIMARY KEY ([Id]),
    FOREIGN KEY([OwnerTypeId]) REFERENCES [dbo].[OwnerType]([Id]),
    FOREIGN KEY([RolePermissonId]) REFERENCES [dbo].[RolePermission]([Id])
)

-- UserStarredBoard junction table
CREATE TABLE [dbo].[UserStarredBoard](
    [UserId] [int] NOT NULL,
    [BoardId] [int] NOT NULL,
    [CreatedAt] [datetime] NOT NULL DEFAULT (getdate()),
    [StarredBoardsStatus] [bit] NOT NULL,
    FOREIGN KEY([UserId]) REFERENCES [dbo].[Users]([Id]),
    FOREIGN KEY([BoardId]) REFERENCES [dbo].[Board]([Id])
)

-- UserViewHistory table
CREATE TABLE [dbo].[UserViewHistory](
    [UserId] [int] NOT NULL,
    [OwnerTypeId] [int] NULL,
    [OwnerId] [int] NOT NULL,
    [AccessedAt] [datetime] NULL,
    FOREIGN KEY([UserId]) REFERENCES [dbo].[Users]([Id]),
    FOREIGN KEY([OwnerTypeId]) REFERENCES [dbo].[OwnerType]([Id])
)

-- WorkspaceMembershipDomain table
CREATE TABLE [dbo].[WorkspaceMembershipDomain](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [WorkspaceId] [int] NOT NULL,
    [Domain] [nvarchar](1000) NOT NULL,
    [CreatedAt] [datetime] NULL,
    PRIMARY KEY ([Id]),
    FOREIGN KEY([WorkspaceId]) REFERENCES [dbo].[Workspace]([Id])
)

-- TemplateCategory table
CREATE TABLE [dbo].[TemplateCategory](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [CategoryValue] [varchar](50) NOT NULL,
    [DisplayValue] [varchar](50) NOT NULL,
    [IconUrl] [varchar](255) NULL,
    PRIMARY KEY ([Id])
)

-- Template table
CREATE TABLE [dbo].[Template](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Title] [varchar](255) NULL,
    [TemplateDescription] [nvarchar](1000) NULL,
    [CategoryId] [int] NULL,
    [Viewed] [int] NULL,
    [Copied] [int] NULL,
    [CreatedBy] [int] NULL,
    [CreatedAt] [datetime] NULL,
    [UpdatedAt] [datetime] NULL,
    [UpdatedBy] [int] NULL,
    [BoardId] [int] NULL,
    [BackgroundUrl] [varchar](255) NULL,
    PRIMARY KEY ([Id]),
    FOREIGN KEY([CategoryId]) REFERENCES [dbo].[TemplateCategory]([Id]),
    FOREIGN KEY([BoardId]) REFERENCES [dbo].[Board]([Id]),
    FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[Users]([Id]),
    FOREIGN KEY([UpdatedBy]) REFERENCES [dbo].[Users]([Id])
)

-- SettingOption table
CREATE TABLE [dbo].[SettingOption](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [DisplayValue] [varchar](255) NULL,
    [SettingOptionValue] [varchar](50) NULL,
    PRIMARY KEY ([Id])
)

-- SettingKey table
CREATE TABLE [dbo].[SettingKey](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [KeyName] [varchar](100) NULL,
    [SettingKeyDescription] [nvarchar](1000) NULL,
    [OwnerTypeId] [int] NULL,
    [DefaultValue] [int] NULL,
    [IsBoolean] [bit] NOT NULL,
    PRIMARY KEY ([Id]),
    FOREIGN KEY([OwnerTypeId]) REFERENCES [dbo].[OwnerType]([Id])
)

-- SettingKeySettingOption junction table
CREATE TABLE [dbo].[SettingKeySettingOption](
    [SettingKeyId] [int] NOT NULL,
    [SettingOptionId] [int] NOT NULL,
    FOREIGN KEY([SettingKeyId]) REFERENCES [dbo].[SettingKey]([Id]),
    FOREIGN KEY([SettingOptionId]) REFERENCES [dbo].[SettingOption]([Id])
)

-- SettingValue table
CREATE TABLE [dbo].[SettingValue](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [SettingKeyId] [int] NULL,
    [SettingContent] [int] NULL,
    [CreatedAt] [datetime] NULL,
    [CreatedBy] [int] NULL,
    [UpdatedAt] [datetime] NULL,
    [UpdatedBy] [int] NULL,
    [OwnerId] [int] NULL,
    PRIMARY KEY ([Id]),
    FOREIGN KEY([SettingKeyId]) REFERENCES [dbo].[SettingKey]([Id]),
    FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[Users]([Id]),
    FOREIGN KEY([UpdatedBy]) REFERENCES [dbo].[Users]([Id])
)

-- BillingPlan table
CREATE TABLE [dbo].[BillingPlan](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [PlanName] [varchar](100) NULL,
    [BillingPlanDescription] [varchar](1000) NULL,
    [PricePerUser] [decimal](10, 2) NULL,
    [IsActive] [bit] NOT NULL,
    PRIMARY KEY ([Id])
)

-- BillingContact table
CREATE TABLE [dbo].[BillingContact](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [int] NULL,
    [WorkspaceId] [int] NULL,
    [BillingContactName] [varchar](50) NULL,
    [BillingContactEmail] [varchar](100) NULL,
    [BillingLanguage] [int] NULL,
    [AdditionalInvoiceDetail] [varchar](250) NULL,
    PRIMARY KEY ([Id]),
    FOREIGN KEY([UserId]) REFERENCES [dbo].[Users]([Id]),
    FOREIGN KEY([WorkspaceId]) REFERENCES [dbo].[Workspace]([Id]),
    FOREIGN KEY([BillingLanguage]) REFERENCES [dbo].[SettingOption]([Id])
)

-- PaymentInformation table
CREATE TABLE [dbo].[PaymentInformation](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [BillingContactId] [int] NULL,
    [CardNumber] [varchar](20) NULL,
    [CardBrand] [varchar](50) NULL,
    [ExpirationDate] [date] NULL,
    [Cvv] [varchar](10) NULL,
    [Country] [varchar](100) NULL,
    [PostalCode] [varchar](20) NULL,
    PRIMARY KEY ([Id]),
    FOREIGN KEY([BillingContactId]) REFERENCES [dbo].[BillingContact]([Id])
)

-- Subscription table
CREATE TABLE [dbo].[Subscription](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [BillingContactId] [int] NULL,
    [BillingPlanId] [int] NULL,
    [StartDate] [date] NULL,
    [EndDate] [date] NULL,
    [IsMonthly] [bit] NOT NULL,
    [SubscriptionStatus] [bit] NOT NULL,
    [AutoRenew] [bit] NULL,
    [MemberCountBilled] [int] NULL,
    PRIMARY KEY ([Id]),
    FOREIGN KEY([BillingContactId]) REFERENCES [dbo].[BillingContact]([Id]),
    FOREIGN KEY([BillingPlanId]) REFERENCES [dbo].[BillingPlan]([Id])
)

-- Export table
CREATE TABLE [dbo].[Export](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [WorkspaceId] [int] NULL,
    [CreatedBy] [int] NULL,
    [CreatedAt] [datetime] NULL,
    [Size] [int] NULL,
    PRIMARY KEY ([Id]),
    FOREIGN KEY([WorkspaceId]) REFERENCES [dbo].[Workspace]([Id]),
    FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[Users]([Id])
)