--1. BOARD TAB SCREEN
--List all Board that the user has starred.
SELECT
    usb.UserId,
    brd.Id BoardId,
    brd.BackgroundUrl,
    brd.BoardName,
    brd.BoardStatus,
    usb.StarredBoardsStatus,
    usb.CreatedAt
FROM UserStarredBoard usb
JOIN Board brd ON brd.Id = usb.BoardId
WHERE UserId = 1 AND brd.BoardStatus = 'active' AND usb.StarredBoardsStatus = 1
ORDER BY usb.CreatedAt DESC;

--List all Board that the user has accessed recently 
SELECT 
    uvh.UserId,
    brd.Id BoardId,
    brd.BoardName, 
    brd.BackgroundUrl,
    uvh.AccessedAt,
    brd.BoardStatus
FROM UserViewHistory uvh
JOIN Board brd ON brd.Id = uvh.OwnerId
JOIN OwnerType owt ON owt.Id = uvh.OwnerTypeId
WHERE uvh.UserId = 5 AND owt.OwnerTypeValue = 'BOARD' AND brd.BoardStatus = 'active'
ORDER BY uvh.AccessedAt DESC;

--List all Workspace that the current user is a member of.
SELECT 
    wsp.Id WorkspaceId,
    wsp.WorkspaceName, 
    wsp.LogoUrl,
    me.UserId,
    wsp.CreatedAt
FROM Workspace wsp
JOIN Members me ON me.OwnerId = wsp.Id
JOIN OwnerType owt ON owt.Id = me.OwnerTypeId
WHERE owt.OwnerTypeValue = 'WORKSPACE' AND me.UserId = 1
ORDER BY wsp.CreatedAt;

--List all Board  that the current user is a member of belonging to a specific workspace.
SELECT 
    brd.Id BoardId,
    brd.BoardName AS BoardName, 
    brd.BackgroundUrl AS BoardBackground,
    wo.WorkspaceName AS WorkspaceName,
    wo.Id WorkspaceId,
    brd.CreatedAt
FROM Board brd
JOIN Members me ON me.OwnerId = brd.Id
JOIN Workspace wo ON wo.Id = brd.WorkspaceId
JOIN OwnerType owt ON owt.Id = me.OwnerTypeId
WHERE me.UserId = 1 AND owt.OwnerTypeValue = 'BOARD' AND wo.Id = 1
ORDER BY brd.CreatedAt;

--2. USER TAB SCREEN
--Query information of a user
SELECT 
    Id,
    PictureUrl,
    Email,
    Username,
    Bio
FROM [Users]
WHERE Email = 'james85@booth-daniels.net';

--3. WORKSPACE CREATE SCREEN
--Retrieve all workspace types
SELECT
    Id WorkspaceTypeId,
    TypeValue,
    DisplayValue
FROM 
    WorkspaceType;

--Retrieve workspace information.
SELECT 
    Id WorkspaceId,
    LogoUrl,
    WorkspaceName,
    ShortName,
    Website,
    WorkspaceDescription
FROM Workspace
WHERE Id = 1;

--4. SELECT A WORKSPACE SCREEN
--List all boards in a specific workspace, where the current user is member and owner.
SELECT 
    brd.Id BoardId,
    brd.BoardName AS BoardName, 
    brd.BackgroundUrl AS BoardBackground,
    wo.Id WorkspaceId,
    wo.WorkspaceName AS WorkspaceName,
    brd.CreatedBy,
    brd.CreatedAt
FROM Board brd
JOIN Members me ON me.OwnerId = brd.Id
JOIN Workspace wo ON wo.Id = brd.WorkspaceId
JOIN OwnerType owt ON owt.Id = me.OwnerTypeId
WHERE 
me.UserId = 4  AND brd.CreatedBy = me.UserId AND owt.OwnerTypeValue = 'BOARD' AND wo.Id = 13
ORDER BY brd.CreatedAt;

--List all Board  that the current user is a member of belonging to a specific workspace.
SELECT 
    brd.Id BoardId,
    brd.BoardName AS BoardName, 
    brd.BackgroundUrl AS BoardBackground,
    wo.WorkspaceName AS WorkspaceName,
    wo.Id WorkspaceId,
    brd.CreatedAt
FROM Board brd
JOIN Members me ON me.OwnerId = brd.Id
JOIN Workspace wo ON wo.Id = brd.WorkspaceId
JOIN OwnerType owt ON owt.Id = me.OwnerTypeId
WHERE me.UserId = 1 AND owt.OwnerTypeValue = 'BOARD' AND wo.Id = 1
ORDER BY brd.CreatedAt;

--List all Workspace that the current user is a member of.
SELECT 
    wsp.Id WorkspaceId,
    wsp.WorkspaceName, 
    wsp.LogoUrl
FROM Workspace wsp
JOIN Members me ON me.OwnerId = wsp.Id
JOIN OwnerType owt ON owt.Id = me.OwnerTypeId
WHERE owt.OwnerTypeValue = 'WORKSPACE' AND me.UserId = 1
ORDER BY wsp.CreatedAt;

--5. BOARD SCREEN
--query all stage in a specific board and all card in each stage
WITH CardComment AS (
    SELECT
        crd.Id CardId,
        COUNT(crd.Id) NumberOfComments
    FROM Cards crd
    JOIN Comment cmt ON cmt.CardId = crd.Id
    GROUP BY crd.Id
),
CardCheckListItem AS (
    SELECT
        crd.Id CardId,
        COUNT(crd.Id) NumberOfCheckListItem
    FROM Cards crd
    JOIN CheckList chl ON chl.CardId = crd.Id
    JOIN CheckListItem cli ON cli.CheckListId = chl.Id
    GROUP BY crd.Id
),
CardAttachment AS (
    SELECT
        crd.Id CardId,
        COUNT(crd.Id) NumberOfAttachment
    FROM Cards crd
    JOIN Attachment atm ON atm.CardId = crd.Id
    GROUP BY crd.Id
)
SELECT 
    crd.Position CardPosition,
    stg.Position StagePosition,
    crd.Id CardId,
    crd.Title CardTitle,
    crd.CardLocation,
    crd.CoverValue CardCover,
    ccm.NumberOfComments,
    cci.NumberOfCheckListItem,
    cam.NumberOfAttachment,
    stg.Id StageId,
    stg.Title StageTitle,
    clr.ColorName StageColor,
    brd.Id BoardId,
    brd.BoardName
FROM Cards crd
JOIN Stage stg ON stg.Id = crd.StageId
JOIN Color clr ON clr.Id = stg.ColorId 
JOIN Board brd ON brd.Id = stg.BoardId
LEFT JOIN CardComment ccm ON ccm.CardId = crd.Id
LEFT JOIN CardCheckListItem cci ON cci.CardId = crd.Id
LEFT JOIN CardAttachment cam ON cam.CardId = crd.Id
WHERE brd.Id = 1
ORDER BY stg.Position, crd.Position;

--Query avatar's member in a specific board
SELECT 
    usr.Id UserId,
    usr.PictureUrl UserPicture,
    owt.OwnerTypeValue,
    mmb.OwnerId BoardId
FROM Members mmb
JOIN OwnerType owt ON owt.Id = mmb.OwnerTypeId
JOIN [Users] usr ON usr.Id = mmb.UserId
WHERE owt.OwnerTypeValue = 'BOARD' AND mmb.OwnerId = 1;

--CARD SCREEN
--List all members available for selection on the card, including members from the workspace and from the board containing that card.
WITH CardWithBoardWorkspace AS (
    SELECT 
        crd.Id CardId,
        crd.Title CardTitle,
        brd.Id BoardId,
        brd.BoardName,
        wsp.Id WorkspaceId,
        wsp.WorkspaceName
    FROM Cards crd
    JOIN Stage stg ON stg.Id = crd.StageId
    JOIN Board brd ON brd.Id = stg.BoardId
    JOIN Workspace wsp ON wsp.Id = brd.WorkspaceId
    WHERE crd.Id = 1

)
SELECT 
    usr.Id UserId,
    usr.PictureUrl UserPicture,
    usr.Username,
    OwnerTypeValue,
    JoinedAt
FROM Members mmb
JOIN OwnerType owt ON owt.Id = mmb.OwnerTypeId
JOIN CardWithBoardWorkspace cwbw ON 
                                (OwnerTypeValue = 'CARD' AND cwbw.CardId = mmb.OwnerId) OR
                                (OwnerTypeValue = 'BOARD' AND cwbw.BoardId = mmb.OwnerId) OR
                                (OwnerTypeValue = 'WORKSPACE' AND cwbw.WorkspaceId = mmb.OwnerId)
JOIN [Users] usr ON usr.Id = mmb.UserId
ORDER BY owt.Id DESC, JoinedAt DESC;

--query information of specific card
SELECT 
    crd.Id CardId,
    crd.Title CardTitle,
    crd.CardDescription,
    crd.CardLocation,
    stg.Title StageTitle
FROM Cards crd
JOIN Stage stg ON stg.Id = crd.Id
WHERE crd.Id = 1;

--query avatar of members in a specific card
SELECT 
    usr.Id UserId,
    usr.PictureUrl UserPicture,
    crd.Id CardId
FROM Cards crd
JOIN Members mmb ON mmb.OwnerId = crd.Id
JOIN OwnerType owt ON owt.Id = mmb.OwnerTypeId
JOIN [Users] usr ON usr.Id = mmb.UserId
WHERE owt.OwnerTypeValue = 'CARD' AND crd.Id = 1;

--query labels in a specific card
SELECT
    crd.Id CardId,
    lbl.Id LabelId,
    lbl.Title LabelTitle,
    clr.ColorName,
    clr.Icon LabelIcon
FROM Cards crd
JOIN CardLabel clb ON clb.CardId = crd.Id
JOIN Labels lbl ON lbl.Id = clb.LabelId
JOIN Color clr ON clr.Id = lbl.ColorId
WHERE crd.Id = 16;

--query comments and reactions in a specific card
SELECT 
  usr.Id UserId, 
  usr.PictureUrl UserPicture, 
  usr.Username, 
  cmt.Content, 
  cmt.Id CommentId, 
  cmt.CreatedAt, 
  cmt.UpdatedAt, 
  crd.Id CardId, 
  rct.Id ReactionId, 
  rct.ReactionName, 
  COUNT(rct.Id) ReactionCount 
FROM 
  Cards crd 
  JOIN Comment cmt ON cmt.CardId = crd.Id 
  JOIN Users usr ON usr.Id = cmt.CreatedBy 
  JOIN CommentReaction cmr ON cmr.CommentId = cmt.Id 
  JOIN Reaction rct ON rct.Id = cmr.ReactionId 
WHERE 
  crd.Id = 1 
GROUP BY 
  usr.Id, 
  usr.PictureUrl, 
  usr.Username, 
  cmt.Content, 
  cmt.Id, 
  cmt.CreatedAt, 
  cmt.UpdatedAt, 
  crd.Id, 
  cmt.Id, 
  rct.Id, 
  rct.ReactionName;
 
--query activity in specific card
SELECT 
    usr.Id UserId,
    usr.PictureUrl UserPicture,
    usr.Username,
    atv.Id ActivityId,
    atv.ActivityDescription,
    atv.CreatedAt,
    owt.OwnerTypeValue Category,
    atv.OwnerId CardId
FROM Activity atv
JOIN OwnerType owt ON owt.Id = atv.OwnerTypeId
JOIN [Users] usr ON usr.Id = atv.UserId
WHERE owt.OwnerTypeValue = 'CARD' AND atv.OwnerId = 1;
--query retrieves all custom fields for a specific board, 
--  along with their possible options if the field type is dropdown
SELECT
    crd.Id CardId,
    brd.Id BoardId,
    ctf.Id CustomFieldId,
    ctf.Title CustomFieldTitle,
    dtt.DataTypeValue,
    ftm.Id FieldItemId,
    ftm.FieldItemValue,
    ctf.Position
FROM Cards crd
JOIN Stage stg ON stg.Id = crd.StageId
JOIN Board brd ON brd.Id = stg.BoardId
JOIN CustomField ctf ON ctf.BoardId = brd.Id
JOIN DataType dtt ON dtt.Id = ctf.DataTypeId
LEFT JOIN FieldItem ftm ON ftm.CustomFieldId = ctf.Id
WHERE crd.Id = 1
ORDER BY ctf.Position;

-- DYNAMIC PIVOT: Query retrieves all custom fields for a specific board
DECLARE @CardId INT = 1;
DECLARE @cols NVARCHAR(MAX);
DECLARE @sql  NVARCHAR(MAX);

-- 1) Lấy danh sách cột = Title của các CustomField trong board chứa card
SELECT @cols = STRING_AGG(QUOTENAME(ctf.Title), ',')
FROM Cards crd
JOIN Stage stg ON stg.Id = crd.StageId
JOIN Board brd ON brd.Id = stg.BoardId
JOIN CustomField ctf ON ctf.BoardId = brd.Id
WHERE crd.Id = @CardId;

-- 2) Dynamic PIVOT
SET @sql = N'
WITH Base AS (
    SELECT
        crd.Id            AS CardId,
        brd.Id            AS BoardId,
        ctf.Title         AS CustomFieldTitle,
        ctf.Position,
        ROW_NUMBER() OVER (
            PARTITION BY crd.Id, ctf.Title
            ORDER BY ctf.Position
        ) AS rn
    FROM Cards crd
    JOIN Stage stg ON stg.Id = crd.StageId
    JOIN Board brd ON brd.Id = stg.BoardId
    JOIN CustomField ctf ON ctf.BoardId = brd.Id
    JOIN DataType dtt ON dtt.Id = ctf.DataTypeId
    WHERE crd.Id = @CardId
)
SELECT CardId, ' + @cols + N'
FROM (
    SELECT CardId, CustomFieldTitle, Position
    FROM Base
    WHERE rn = 1
) AS src
PIVOT (
    MAX(Position) FOR CustomFieldTitle IN (' + @cols + N')
) AS p
ORDER BY CardId;';

EXEC sp_executesql @sql, N'@CardId INT', @CardId=@CardId;

--query retrieves all custom fields for a specific board, 
--  along with their value
WITH FieldValueCast AS (
    SELECT
        fvl.Id,
        fvl.CardId,
        fvl.CustomFieldId,
        dtt.DataTypeValue,
        CASE
            WHEN dtt.DataTypeValue = 'DROPDOWN' 
                AND ISNUMERIC(fvl.FieldValue) = 1
            THEN CAST(fvl.FieldValue AS INT)
            ELSE NULL
        END AS ItemId,
        fvl.FieldValue
    FROM FieldValue fvl
    JOIN CustomField ctf ON ctf.Id = fvl.CustomFieldId
    JOIN DataType dtt ON dtt.Id = ctf.DataTypeId 
)
SELECT
    crd.Id CardId,
    brd.Id BoardId,
    ctf.Id CustomFieldId,
    ctf.Title CustomFieldTitle,
    fvc.DataTypeValue,
    fvc.FieldValue,
    ftm.FieldItemValue,
    ctf.Position
FROM Cards crd
JOIN Stage stg ON stg.Id = crd.StageId
JOIN Board brd ON brd.Id = stg.BoardId
JOIN CustomField ctf ON ctf.BoardId = brd.Id
LEFT JOIN FieldValueCast fvc ON fvc.CardId = crd.Id AND fvc.CustomFieldId = ctf.Id
LEFT JOIN FieldItem ftm ON ftm.Id = fvc.ItemId
WHERE crd.Id = 1
ORDER BY ctf.Position;

-- PIVOT: query retrieves all custom fields for a specific board, 
--  along with their value
DECLARE @CardId INT = 1;
DECLARE @cols NVARCHAR(MAX);
DECLARE @sql  NVARCHAR(MAX);

-- 1) Lấy danh sách cột = Title của các CustomField trong board chứa card
SELECT @cols = STRING_AGG(QUOTENAME(ctf.Title), ',')
FROM Cards crd
JOIN Stage stg  ON stg.Id  = crd.StageId
JOIN Board brd  ON brd.Id  = stg.BoardId
JOIN CustomField ctf ON ctf.BoardId = brd.Id
WHERE crd.Id = @CardId;

-- 2) Dynamic PIVOT
SET @sql = N'
WITH FieldValueCast AS (
    SELECT
        fvl.Id,
        fvl.CardId,
        fvl.CustomFieldId,
        dtt.DataTypeValue,
        CASE
            WHEN dtt.DataTypeValue = ''DROPDOWN'' AND ISNUMERIC(fvl.FieldValue) = 1
                 THEN CAST(fvl.FieldValue AS INT)
            ELSE NULL
        END AS ItemId,
        fvl.FieldValue
    FROM FieldValue fvl
    JOIN CustomField ctf ON ctf.Id = fvl.CustomFieldId
    JOIN DataType dtt ON dtt.Id = ctf.DataTypeId
),
Base AS (
    SELECT
        crd.Id                           AS CardId,
        ctf.Title                        AS CustomFieldTitle,
        COALESCE(ftm.FieldItemValue, fvc.FieldValue) AS ValueToPivot,
        ROW_NUMBER() OVER (PARTITION BY crd.Id, ctf.Id ORDER BY fvc.Id DESC) AS rn
    FROM Cards crd
    JOIN Stage stg  ON stg.Id  = crd.StageId
    JOIN Board brd  ON brd.Id  = stg.BoardId
    JOIN CustomField ctf ON ctf.BoardId = brd.Id
    LEFT JOIN FieldValueCast fvc 
        ON fvc.CardId = crd.Id AND fvc.CustomFieldId = ctf.Id
    LEFT JOIN FieldItem ftm 
        ON ftm.Id = fvc.ItemId
    WHERE crd.Id = @CardId
),
OneVal AS (
    SELECT CardId, CustomFieldTitle, ValueToPivot
    FROM Base
    WHERE rn = 1
)
SELECT CardId, ' + @cols + '
FROM OneVal
PIVOT (
    MAX(ValueToPivot) FOR CustomFieldTitle IN (' + @cols + ')
) p
ORDER BY CardId;';

EXEC sp_executesql @sql, N'@CardId INT', @CardId=@CardId;


--List all attachments belonging to a specific card, including their file details and upload information
SELECT 
    atm.Id AttachmentId,
    att.DisplayValue AttachmentType,
    atm.AttachmentName,
    atm.AttachmentPath,
    atm.Size,
    atm.CreatedAt,
    atm.CreatedBy,
    atm.IsCover,
    atm.CardId
FROM Attachment atm
JOIN AttachmentType att ON att.Id = atm.AttachmentTypeId
WHERE atm.CardId = 1

--Retrieve all checklists and their items for a specific card, 
-- including item details such as status, assigned members, and due dates
SELECT 
    clt.Id AS ChecklistId, 
    clt.ChecklistName,
    cli.Id AS ChecklistItemId, 
    cli.CheckListItemName, 
    cli.CheckListItemStatus,
    cli.Position CheckListItemPosition,
    cli.DueDate,
    cli.MemberId,
    usr.PictureUrl

FROM Checklist clt
JOIN ChecklistItem cli ON cli.ChecklistId = clt.Id
LEFT JOIN Members mmb ON mmb.Id = cli.MemberId
LEFT JOIN [Users] usr ON usr.Id = mmb.UserId
WHERE clt.CardId =1
ORDER BY clt.Id, cli.Position;


--6. WORKSPACE MEMBER SCREEN
--List all members in the workspace along with their permission on roles.
WITH BoardCountByEachUser AS(
    SELECT 
        UserId, 
        COUNT(UserId) AS BoardCount
    FROM Members mmb
    JOIN OwnerType owt ON owt.Id = mmb.OwnerTypeId
    JOIN Board brd ON brd.Id = mmb.OwnerId
    WHERE OwnerTypeValue = 'BOARD' AND brd.WorkspaceId = 11
    GROUP BY UserId
)

SELECT 
    us.PictureUrl AS UserPicture,
    us.Username,
    us.Email AS UserEmail,
    us.LastActive AS UserLastActive,
    pe.PermissionName,
    bcb.BoardCount
FROM Members mmb
JOIN OwnerType owt ON owt.Id = mmb.OwnerTypeId
JOIN RolePermission pe ON pe.Id = mmb.RolePermissonId
JOIN [Users] us ON us.Id = mmb.UserId
JOIN BoardCountByEachUser bcb ON bcb.UserId = mmb.UserId
WHERE owt.OwnerTypeValue = 'WORKSPACE' AND mmb.OwnerId = 1
ORDER BY mmb.JoinedAt;

--List All Role Permission
SELECT
    Id RolePermissionId,
    PermissionName,
    PermissionCode
FROM RolePermission

--7. BOARD MEMBER SCREEN
--List all members in the board along with their permission on roles.
SELECT
    us.Id UserId,
    us.PictureUrl user_picture,
    us.Username,
    us.Email user_mail,
    pe.PermissionName
FROM Members mmb
JOIN OwnerType owt ON owt.Id = mmb.OwnerTypeId
JOIN RolePermission pe ON pe.Id = mmb.RolePermissonId
JOIN Board bo ON bo.Id = mmb.OwnerId
JOIN [Users] us ON us.Id = mmb.UserId
WHERE OwnerTypeValue = 'BOARD' AND mmb.OwnerId = 1;


--8. WORKSPACE SETTING SCREEN
--List all workspace setting keys with the current user's selected values.
SELECT 
    sk.KeyName, 
    COALESCE(sv.SettingContent, sk.DefaultValue) AS Value
FROM SettingKey sk 
JOIN OwnerType owt ON owt.Id = sk.OwnerTypeId
LEFT JOIN SettingValue sv ON sv.SettingKeyId = sk.Id
WHERE OwnerTypeValue = 'WORKSPACE' AND sv.OwnerId = 1;

--List all options of setting is not boolean in workspace setting.
SELECT 
    sk.KeyName AS setting_key,
    sk.SettingKeyDescription,
    so.DisplayValue AS setting_option_display_value
FROM SettingKey sk
JOIN OwnerType owt ON owt.Id = sk.OwnerTypeId
JOIN SettingKeySettingOption sso ON sso.SettingKeyId = sk.Id
JOIN SettingOption so ON so.Id = sso.SettingOptionId
WHERE OwnerTypeValue = 'WORKSPACE'
ORDER BY sk.KeyName;

--9. BOARD SETTING SCREEN
--List all board's setting key and user's choice of a specific user
SELECT 
    sk.KeyName, 
    COALESCE(sv.SettingContent, sk.DefaultValue) AS Value
FROM SettingKey sk 
JOIN OwnerType owt ON owt.Id = sk.OwnerTypeId
LEFT JOIN SettingValue sv ON sv.SettingKeyId = sk.Id
WHERE OwnerTypeValue = 'BOARD' AND sv.OwnerId = 1; 

--List all options of setting is not boolean in board setting.
SELECT 
    sk.KeyName AS setting_key,
    sk.SettingKeyDescription,
    so.DisplayValue AS setting_option_display_value
FROM SettingKey sk
JOIN OwnerType owt ON owt.Id = sk.OwnerTypeId
JOIN SettingKeySettingOption sso ON sso.SettingKeyId = sk.Id
JOIN SettingOption so ON so.Id = sso.SettingOptionId
WHERE OwnerTypeValue = 'BOARD'
ORDER BY sk.KeyName;

--10. USER SETTING SCREEN
--List all user setting keys with the current user's selected values.
SELECT 
    sk.KeyName, 
    COALESCE(sv.SettingContent, sk.DefaultValue) AS Value
FROM SettingKey sk 
JOIN OwnerType owt ON owt.Id = sk.OwnerTypeId
LEFT JOIN SettingValue sv ON sv.SettingKeyId = sk.Id 
WHERE OwnerTypeValue = 'USER' AND sv.OwnerId = 1;

--List all options of setting is not boolean in user setting.
SELECT 
    sk.KeyName AS setting_key,
    sk.SettingKeyDescription,
    so.DisplayValue AS setting_option_display_value
FROM SettingKey sk
JOIN OwnerType owt ON owt.Id = sk.OwnerTypeId
JOIN SettingKeySettingOption sso ON sso.SettingKeyId = sk.Id
JOIN SettingOption so ON so.Id = sso.SettingOptionId
WHERE OwnerTypeValue = 'USER'
ORDER BY sk.KeyName;

--11. TEMPLATE TYPE SCREEN
--List all template categories available for filtering.
SELECT
    tpc.Id TemplateCategoryId,
    tpc.IconUrl,
    tpc.DisplayValue
FROM TemplateCategory tpc;


-- Select a template category to display all templates within that category.
SELECT
    tpl.Id TemplateId,
    tpl.Title TemplateTitle,
    tpl.TemplateDescription,
    tpl.Viewed,
    tpl.Copied,
    tpl.UpdatedAt,
    tpc.Id TemplateCategoryId,
    tpc.DisplayValue TemplateCategory
FROM Template tpl
JOIN TemplateCategory tpc ON tpc.Id = tpl.CategoryId
WHERE tpc.Id = 1
ORDER BY tpl.UpdatedAt DESC;

--12. TEMPLATE SCREEN
--Select specific template, show information of a specific template.
SELECT
    tpl.Id TemplateId,
    us.PictureUrl AS user_picture,
    tpl.TemplateDescription AS template_description,
    tpl.Title AS template_title,
    us.Username,
    tpl.Copied AS copied_number,
    tpl.Viewed AS viewed_number,
    tpl.BoardId
FROM Template tpl
JOIN [Users] us ON us.Id = tpl.CreatedBy
WHERE tpl.Id = 1;

--13. BOARD COLLECTION SCREEN
--Show board and collection its belong with in a specific workspace
SELECT 
    bo.Id board_id,
    bo.BoardName board_name,
    bo.BackgroundUrl BoardBackgroundImage,
    co.Id collection_id,
    co.CollectionName,
    bo.WorkspaceId WorkspaceId
FROM Board bo
JOIN BoardCollection bc ON bc.BoardId = bo.Id 
JOIN Collections co ON co.Id = bc.CollectionId
WHERE bo.WorkspaceId = 1 AND co.WorkspaceId = bo.WorkspaceId
ORDER BY bo.CreatedAt

--Show all the collections in a specific workspace
SELECT 
  bo.Id BoardId, 
  bo.BoardName BoardName, 
  bo.BackgroundUrl BoardBackgroundImage, 
  co.Id CollectionId, 
  co.CollectionName, 
  bo.WorkspaceId WorkspaceId 
FROM 
  Board bo 
  JOIN BoardCollection bc ON bc.BoardId = bo.Id 
  JOIN Collections co ON co.Id = bc.CollectionId 
WHERE 
  bo.WorkspaceId = 2
  AND co.WorkspaceId = bo.WorkspaceId 
ORDER BY 
  bo.CreatedAt

--14. CARD STICKER SCREEN
--Show list of general sticker can select
SELECT 
    stk.Id StickerId,
    stk.StickerName,
    stk.StickerUrl,
    skc.Id StickerCateogry,
    skc.DisplayValue
FROM Sticker stk
JOIN StickerCategory skc ON skc.Id = stk.CategoryId
WHERE DisplayValue != 'Custom Stickers'
ORDER BY skc.DisplayValue;

--Show list of custom sticker can select
SELECT 
    stk.Id StickerId,
    stk.StickerName,
    stk.StickerUrl,
    skc.Id StickerCateogry,
    skc.DisplayValue
    ,stk.CreatedBy
FROM Sticker stk
JOIN StickerCategory skc ON skc.Id = stk.CategoryId
WHERE DisplayValue = 'Custom Stickers' AND stk.CreatedBy = 1;

--Show sticker in cover of a card
SELECT 
    st.Id AS sticker_id,
    st.StickerName AS sticker_name,
    st.StickerUrl AS sticker_url,
    PositionX,
    PositionY,
    IndexZ,
    CardId AS card_id
FROM CardSticker cs
JOIN Sticker st ON st.Id = cs.StickerId
WHERE CardId = 1;

--15. Notification SCREEN
--Show all notification are unread
SELECT 
    noti.Id AS NotificationId,
    us.Id UserId,
    us.PictureUrl AS UserPicture,
    us.Username,
    ac.ActivityDescription AS ActivityDescription,
    noti.IsRead,
    owt.OwnerTypeValue,
    ac.OwnerId
FROM [Notification] noti
JOIN Activity ac ON ac.Id = noti.ActivityId
JOIN OwnerType owt ON owt.Id = ac.OwnerTypeId
JOIN [Users] us ON us.Id = ac.UserId
WHERE ac.UserId = 2 AND noti.IsRead = 0;

--Show all notification are read
SELECT 
    noti.Id AS NotificationId,
    us.Id UserId,
    us.PictureUrl AS UserPicture,
    us.Username,
    ac.ActivityDescription AS ActivityDescription,
    noti.IsRead,
    owt.OwnerTypeValue,
    ac.OwnerId
FROM [Notification] noti
JOIN Activity ac ON ac.Id = noti.ActivityId
JOIN OwnerType owt ON owt.Id = ac.OwnerTypeId
JOIN [Users] us ON us.Id = ac.UserId
WHERE ac.UserId = 2 AND noti.IsRead = 1;

