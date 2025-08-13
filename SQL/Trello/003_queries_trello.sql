--BOARD TAB
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
ORDER BY usb.CreatedAt DESC

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

--Query information of a user
SELECT 
    Id,
    PictureUrl,
    Email,
    Username,
    Bio
FROM [User]
WHERE Email = 'james85@booth-daniels.net'

--WORKSPACE
--Retrieve all workspace types
SELECT
    Id WorkspaceTypeId,
    TypeValue,
    DisplayValue
FROM 
    WorkspaceType;

--List all boards in a specific workspace, created by a specific user, where the current user is a member.
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
    brd.BoardName AS board_name, 
    brd.BackgroundUrl AS board_background,
    wo.WorkspaceName AS workspace_name
FROM Board brd
JOIN Members me ON me.OwnerId = brd.Id
JOIN Workspace wo ON wo.Id = brd.WorkspaceId
JOIN OwnerType owt ON owt.Id = me.OwnerTypeId
WHERE me.UserId = 1 AND owt.OwnerTypeValue = 'BOARD'
ORDER BY brd.CreatedAt;

--Retrieve workspace information.
SELECT 
    Id WorkspaceId,
    LogoUrl,
    WorkspaceName,
    ShortName,
    Website,
    WorkspaceDescription
FROM Workspace
WHERE Id = 1

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

--BOARD SCREEN
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
ORDER BY stg.Position, crd.Position

--Query avatar's member in a specific board
SELECT 
    usr.Id UserId,
    usr.PictureUrl UserPicture,
    owt.OwnerTypeValue,
    mmb.OwnerId BoardId
FROM Members mmb
JOIN OwnerType owt ON owt.Id = mmb.OwnerTypeId
JOIN [User] usr ON usr.Id = mmb.UserId
WHERE owt.OwnerTypeValue = 'BOARD' AND mmb.OwnerId = 1
--CARD
--query information of specific card
SELECT 
    crd.Id CardId,
    crd.Title CardTitle,
    crd.CardDescription,
    crd.CardLocation,
    stg.Title StageTitle
FROM Cards crd
JOIN Stage stg ON stg.Id = crd.Id
WHERE crd.Id = 1

--query avatar of members in a specific card
SELECT 
    usr.Id UserId,
    usr.PictureUrl UserPicture,
    crd.Id CardId
FROM Cards crd
JOIN Members mmb ON mmb.OwnerId = crd.Id
JOIN OwnerType owt ON owt.Id = mmb.OwnerTypeId
JOIN [User] usr ON usr.Id = mmb.UserId
WHERE owt.OwnerTypeValue = 'CARD' AND crd.Id = 1

--query labels in a specific card
SELECT
    crd.Id CardId,
    lbl.Id LabelId,
    lbl.Title LabelTitle,
    clr.ColorName,
    clr.Icon
FROM Cards crd
JOIN CardLabel clb ON clb.CardId = crd.Id
JOIN [Label] lbl ON lbl.Id = clb.LabelId
JOIN Color clr ON clr.Id = lbl.ColorId

--query comments in a specific card
SELECT 
    usr.Id UserId,
    usr.PictureUrl UserPicture,
    usr.Username,
    cmt.Content,
    cmt.Id CommentId,
    cmt.CreatedAt,
    cmt.UpdatedAt,
    crd.Id CardId
FROM Cards crd
JOIN Comment cmt ON cmt.CardId = crd.Id
JOIN [User] usr ON usr.Id = cmt.CreatedBy
WHERE crd.Id = 1;

--query reaction of comments in specific card
SELECT 
    cmt.Id CommentId,
    rct.Id ReactionId,
    rct.ReactionsName,
    COUNT(rct.Id) ReactionCount
FROM Cards crd
JOIN Comment cmt ON cmt.CardId = crd.Id
JOIN CommentReaction cmr ON cmr.CommentId = cmt.Id
JOIN Reaction rct ON rct.Id = cmr.ReactionId
WHERE crd.Id = 1
GROUP BY cmt.Id, rct.Id, rct.ReactionsName;
     
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
JOIN OwnerType owt ON owt.Id = atv.CategoryId
JOIN [User] usr ON usr.Id = atv.UserId
WHERE owt.OwnerTypeValue = 'CARD' AND atv.OwnerId = 1 
--MEMBER
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
JOIN [User] us ON us.Id = mmb.UserId
JOIN BoardCountByEachUser bcb ON bcb.UserId = mmb.UserId
WHERE owt.OwnerTypeValue = 'WORKSPACE' AND mmb.OwnerId = 11
ORDER BY mmb.JoinedAt;

--List All Role Permission
SELECT
    Id RolePermissionId,
    PermissionName,
    PermissionCode
FROM RolePermission

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
JOIN [User] us ON us.Id = mmb.UserId
WHERE OwnerTypeValue = 'BOARD' AND mmb.OwnerId = 1;

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
JOIN [User] usr ON usr.Id = mmb.UserId
ORDER BY owt.Id DESC, JoinedAt DESC;

--SETTING
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

--List all workspace setting keys with the current user's selected values.
SELECT 
    sk.KeyName, 
    COALESCE(sv.SettingContent, sk.DefaultValue) AS Value
FROM SettingKey sk 
JOIN OwnerType owt ON owt.Id = sk.OwnerTypeId
LEFT JOIN SettingValue sv ON sv.SettingKeyId = sk.Id 
WHERE OwnerTypeValue = 'USER' AND sv.OwnerId = 1;

--List all options of setting is not boolean in board setting.
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

--TEMPLATE
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
JOIN [User] us ON us.Id = tpl.CreatedBy
WHERE tpl.Id = 1;

--BOARD COLLECTION
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
    clt.Id CollectionId,
    clt.CollectionName,
    clt.CreatedAt,
    clt.WorkspaceId
FROM Collections clt
WHERE WorkspaceId = 2
ORDER BY clt.CreatedAt

--CARD STICKER
--Show list of general sticker can select
SELECT 
    stk.Id StickerId,
    stk.StickerName,
    stk.StickerUrl,
    skc.Id StickerCateogry,
    skc.DisplayValue
FROM Sticker stk
JOIN StickerCategory skc ON skc.Id = stk.CategoryId
WHERE DisplayValue != 'Custom Stickers';

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

--Notification
--Show all notification are unread
SELECT 
    noti.Id AS notification_id,
    us.Id UserId,
    us.PictureUrl AS user_picture,
    us.Username,
    ac.ActivityDescription AS activity_description,
    noti.IsRead,
    owt.OwnerTypeValue,
    ac.OwnerId
FROM [Notification] noti
JOIN Activity ac ON ac.Id = noti.ActivityId
JOIN OwnerType owt ON owt.Id = ac.CategoryId
JOIN [User] us ON us.Id = ac.UserId
WHERE ac.UserId = 2 AND noti.IsRead = 0;

--Show all notification are read
SELECT 
    noti.Id AS notification_id,
    us.Id UserId,
    us.PictureUrl AS user_picture,
    us.Username,
    ac.ActivityDescription AS activity_description,
    noti.IsRead,
    owt.OwnerTypeValue,
    ac.OwnerId
FROM [Notification] noti
JOIN Activity ac ON ac.Id = noti.ActivityId
JOIN OwnerType owt ON owt.Id = ac.CategoryId
JOIN [User] us ON us.Id = ac.UserId
WHERE ac.UserId = 2 AND noti.IsRead = 1;

