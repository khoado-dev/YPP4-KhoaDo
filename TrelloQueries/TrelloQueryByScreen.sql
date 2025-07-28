--1.Slide 4 | Home Page on the Boards tab → Recently viewed section, list all boards that the user has accessed recently.
SELECT 
        bo.[Name] AS board_name, 
        bo.BackgroundUrl
FROM BoardUsers bu
JOIN Boards bo ON bo.Id = bu.BoardId
WHERE UserId = 1
ORDER BY bu.AccessedAt DESC;

--2.Slide 4 | Home Page on the Boards tab → Your Workspaces section, list all workspaces that the current user is a member of.
SELECT 
    wo.Name, 
    wo.LogoUrl
FROM Workspaces wo
JOIN Members me ON me.OwnerId = wo.Id
WHERE me.OwnerTypeId = 1 AND me.UserId = 1;

--3.Slide 4 | Home Page on the Boards tab → Workspace item → Boards button, list all boards  that the current user is a member of belonging to a specific workspace.
SELECT 
    bo.[Name] AS board_name, 
    bo.BackgroundUrl AS board_background,
    wo.[Name] AS workspace_name
FROM Boards bo
JOIN Members me ON me.OwnerId = bo.Id
JOIN Workspaces wo ON wo.Id = bo.WorkspaceId
WHERE me.UserId = 1 AND me.OwnerTypeId = 2; --2:Board

--4.Slide 4 | Home Page on the Header (top right corner), query boards have name that contains the keyword 'ab'
SELECT 
    bo.[Name], 
    wo.[Name], 
    bo.[Status]
FROM Boards bo
JOIN Workspaces wo ON wo.Id = bo.Id
WHERE bo.[Name] LIKE '%ab%';

--5.Slide 4 | Home Page on the Header (top right corner), show the total number of unread notifications of the user.
SELECT 
    COUNT(ac.UserId) AS number_of_notifications
FROM Notifications [no]
JOIN Activities ac ON ac.Id = no.ActivityId
WHERE ac.UserId = 2 AND no.[Status] = 'UNREAD';

--6.Slide 5 | Home Page on the Templates tab → Main area, list all available public or user-created templates.
SELECT 
    us.PictureUrl AS user_picture, 
    us.Username AS author, 
    bo.BackgroundUrl AS board_background, 
    te.Title AS template_title, 
    te.[Description] AS tempalte_description, 
    te.Viewed, 
    te.Copied   
FROM Templates te
JOIN Users us ON us.Id = te.CreatedBy
JOIn Boards bo ON bo.Id = te.BoardId
ORDER BY Viewed DESC, Copied DESC;

--7.Slide 5 | Home Page on the Templates tab → Sidebar, list all template categories available for filtering.
SELECT 
    IconUrl AS template_category_icon, 
    [Name] AS template_name
FROM TemplateCategories;

--8.Slide 5 | Home Page on the Templates tab, query templates have title that contains the keyword 'da' 
SELECT 
    bo.BackgroundUrl AS board_background, 
    us.Username AS created_by, 
    te.Title AS template_title
FROM Templates te
JOIN Users us ON us.Id = te.CreatedBy
JOIN Boards bo ON bo.Id = te.BoardId
WHERE Title LIKE '%da%';

--9. Slide 6 | Home Page on the Templates tab → Main area → Select specific tempalte, show information of a specific template.
WITH selected_template AS (
    SELECT
        Title,
        CreatedBy,
        Copied,
        Viewed,
        Description,
        BoardId
    FROM Templates
    WHERE Id = 1
)

SELECT
    us.PictureUrl AS user_picture,
    st.[Description] AS template_description,
    st.Title AS template_title,
    us.Username,
    st.Copied AS copied_number,
    st.Viewed AS viewed_number,
    st.BoardId
FROM selected_template st
JOIN Users us ON us.Id = st.CreatedBy;

--10.Slide 7 | Home Page on the Home tab → Checklist section, list all checklist items assigned to the user with status set to false (incomplete).
SELECT 
    cli.[Name] AS checklist_item_name, 
    cli.[Status] AS checklist_item_status,
    ca.Title AS card_title, 
    bo.[Name] AS board_name,
    us.PictureUrl
FROM CheckListItems cli
JOIN CheckLists cl ON cl.Id = cli.CheckListId
JOIN Cards ca ON ca.Id = cl.CardId
JOIN Stages st ON st.Id = ca.StageId
JOIN Boards bo ON bo.Id = st.BoardId
JOIN Members me ON me.Id = cli.MemberId
JOIN Users us ON us.Id = me.UserId
WHERE cli.[Status] = 0 AND me.UserId = 1;

--11.Slide 7 Home Page on the Home tab → Assigned cards section, list all cards that are currently assigned to the user.
SELECT 
    ca.Title AS card_title,
    bo.[Name] AS board_name,
    st.Title AS stage_title,
    us.PictureUrl AS user_picture,
    ABS(DATEDIFF(DAY, GETDATE(), me.JoinedAt)) AS day_ago
FROM (Cards ca 
    JOIN Members me ON me.OwnerId = ca.Id 
                        AND me.UserId = 1 
                        AND me.OwnerTypeId = 3)
JOIN Stages st ON st.Id = ca.StageId
JOIN Boards bo ON bo.Id = st.BoardId
JOIN Users us ON us.Id = me.UserId
ORDER BY day_ago;
--12.Slide 7 Home Page on the Home tab → Activity feed section, list all recent card's activities in the user's card.
SELECT 
    ca.Title AS card_title,
    wo.[Name] AS workspace_name,
    bo.[Name] AS board_name,
    st.Title AS stage_title,
    us.Username AS username,
    us.PictureUrl AS user_picture,
    ABS(DATEDIFF(DAY, GETDATE(), ac.CreatedAt)) AS day_ago,
    ac.[Description] AS activity_description
FROM (SELECT UserId, OwnerTypeId, OwnerId, [Description], CreatedAt
    FROM Activities WHERE OwnerTypeId = 3) ac
JOIN Cards ca ON ca.Id = ac.OwnerId AND ca.CreatedBy = 1
JOIN Stages st ON st.Id = ca.StageId
JOIN Boards bo ON bo.Id = st.BoardId
JOIN Workspaces wo ON wo.Id = bo.WorkspaceId
JOIN Users us ON us.Id = ac.UserId
Order By day_ago;

--13.Slide 9 Home Page on the Workspace page → Boards section, list all boards under the selected workspace.
SELECT 
    bo.BackgroundUrl AS board_background,
    bo.[Name] AS board_name
FROM Boards bo
WHERE bo.WorkspaceId = 1;

--14.Slide 13 Home Page on the Workspace page → Members section, list all members in the workspace along with their permission on roles.
WITH BoardCountByEachUser AS(
    SELECT UserId, COUNT(UserId) AS board_count
    FROM Members
    WHERE OwnerTypeId = 2
    GROUP BY UserId
)

SELECT 
    us.PictureUrl AS user_picture,
    us.Username,
    us.Email AS user_email,
    us.LastActive AS user_last_active,
    pe.[Name] AS [permission_name],
    bcb.board_count
FROM (
    SELECT 
        UserId, OwnerTypeId, OwnerId,PermissionId 
    FROM Members
    WHERE OwnerTypeId = 1 AND OwnerId = 1
    ) AS me
JOIN [Permissions] pe ON pe.Id = me.PermissionId
JOIN Users us ON us.Id = me.UserId
JOIN BoardCountByEachUser bcb ON bcb.UserId = me.UserId
ORDER BY OwnerId;

--15.Slide 13 Home Page on the Workspace page → Members section, count the total number of members in the selected workspace.
SELECT 
    COUNT(me.UserId) AS workspace_member_number
FROM Members me
JOIN [Permissions] pe ON pe.Id = me.PermissionId
WHERE me.OwnerTypeId = 1 AND me.OwnerId = 153;

--16.Slide 14 Board Page on the Share Board pop-up, list all members in the board along with their permission on roles.
SELECT
    us.PictureUrl user_picture,
    us.Username,
    us.Email user_mail,
    pe.[Name] [permission_name]
FROM (
    SELECT UserId, PermissionId, OwnerId
    FROM Members
    WHERE OwnerTypeId = 2 AND OwnerId = 1
) AS me
JOIN [Permissions] pe ON pe.Id = me.PermissionId
JOIN Boards bo ON bo.Id = me.OwnerId
JOIN Users us ON us.Id = me.UserId;

--17.Slide 15 Board Page on the Share Board pop-up, list all permission options can choose
SELECT 
    [Name] AS [permission_name]
FROM [Permissions]


--18.Slide 17 Home Page on the Workspace page → Settings section, list all workspace setting keys with the current user's selected values.
SELECT 
    sk.KeyName, 
    COALESCE(sv.Value, sk.DefaultValue) AS Value,
    sv.OwnerId
FROM SettingKeys sk 
LEFT JOIN SettingValues sv ON sv.SettingKeyId = sk.Id AND sk.OwnerTypeId = 4 AND sv.OwnerId = 1;

--19.Slide 17 Home Page on the Workspace page → Settings section, list all settingoption of a specific workspace setting.
WITH sk AS (
    SELECT 
    KeyName,
    [Description],
    Id
    FROM SettingKeys
    WHERE OwnerTypeId = 1 AND Id = 1
)

SELECT 
    sk.KeyName AS setting_key,
    sk.[Description] AS setting_key_description,
    so.DisplayValue AS setting_option_display_value
FROM SettingKeySettingOptions sso 
JOIN sk ON sso.SettingKeyId = sk.Id
JOIN SettingOptions so ON so.Id = sso.SettingOptionId;


--20.Slide 19 Board Page on the Setting pop-up, list all board's setting key and user's choice of a specific user
SELECT 
    sk.KeyName, 
    COALESCE(sv.Value, sk.DefaultValue) as setting_value
FROM (
    SELECT *
    FROM SettingKeys
    WHERE OwnerTypeId = 2
) AS sk
LEFT JOIN SettingValues sv ON sv.SettingKeyId = sk.Id AND sv.OwnerId = 1;

--21.Slide 21 Home Page on the Workspace page → Power-Ups section, list on power-ups of a specific workspace are using
WITH boards_in_specific_workspace AS (
    SELECT Id, WorkspaceId
    FROM Boards
    WHERE WorkspaceId = 1
), 
power_ups_in_workspace AS (
    SELECT
        bop.PowerUpId AS power_ups_id,
        COUNT(bop.PowerUpId) AS number_of_boards

    FROM boards_in_specific_workspace bo
    JOIN BoardPowerUps bop ON bop.BoardId = bo.Id
    GROUP BY bop.PowerUpId
)

SELECT 
    pu.IconUrl AS power_up_icon,
    pu.[Name] AS power_up_name,
    puiw.number_of_boards
FROM power_ups_in_workspace puiw
JOIN PowerUps pu ON pu.Id = puiw.power_ups_id;

--22.Slide 22 Home Page on the Workspace page → Power-Ups section → click a specific power-ups, show information of a specific power-up
WITH board_using_powerup_count AS (
    SELECT
        PowerUpId,
        COUNT(PowerUpId) AS number_of_board
    FROM BoardPowerUps
    WHERE PowerUpId = 1
    GROUP BY PowerUpId
)

SELECT
    po.Id,
    po.IconUrl,
    po.AuthorName,
    po.PowerUpCategoryId,
    po.EmailContact,
    po.PolicyUrl,
    po.[Name],
    po.[Description],
    bc.number_of_board
FROM PowerUps po
JOIN board_using_powerup_count bc ON bc.PowerUpId = po.Id;

--23.Slide 24 Home Page on the Workspace page → Upgrade section, list all available billing plans that the workspace can upgrade to.
SELECT 
    [Name] AS billing_plan_name,
    [Description] AS billing_plan_description,
    [Type] AS biling_plan_type,
    PricePerUser
FROM BillingPlans;

--24.Slide 26.Home Page on the Workspace page → Billing section, show information of payment and billing
WITH billing_selected_workspace AS (
    SELECT
        Id,
        WorkspaceId,
        [Name],
        Email,
        AdditionalInvoiceDetail
    FROM BillingContacts
    WHERE WorkspaceId = 1
),
member_in_workspace AS (
    SELECT
        OwnerId,
        COUNT(OwnerId) AS number_of_member
    FROM Members
    WHERE OwnerTypeId = 1 AND OwnerId = 1
    GROUP BY OwnerId
)

SELECT 
    su.EndDate AS end_day_subscription,
    bp.[Name] AS plan_name,
    bp.PricePerUser AS plan_price_per_person,
    miw.number_of_member,
    bp.[Type] AS type_of_plan,
    pai.CardNumber AS credit_card_number,
    bsw.[Name] AS billing_contact_name,
    bsw.Email AS billing_contact_email,
    bsw.AdditionalInvoiceDetail AS invoice_details

FROM billing_selected_workspace bsw
JOIN PaymentInformations pai ON pai.BillingId = bsw.Id
JOIN Subscriptions su ON su.BillingId = bsw.Id
JOIN BillingPlans bp ON bp.Id = su.BillingPlanId
JOIN member_in_workspace miw ON miw.OwnerId = bsw.WorkspaceId;

--25.Slide 30. Home Page on the Workspace page → Export section, list all history exports of a selected workspace
SELECT
    CreatedAt,
    Size
FROM Exports
WHERE WorkspaceId = 1;

--26. Slide 33. Select a board → Board page, list all Stage in a selected board include Card
SELECT
    ca.Title AS card_title,
    ca.Position AS card_postion,
    st.Title AS stage_title,
    bo.[Name] AS board_name,
    bo.[Status] AS board_status
FROM (
    SELECT
        Id,
        [Name],
        [Status],
        WorkspaceId
    FROM Boards
    WHERE Id = 1
) bo
JOIN Stages st ON st.BoardId = bo.Id
JOIN Cards ca ON ca.StageId = st.Id
ORDER BY st.Position, ca.Position;

--27. Slide 34. Select a board → Board page → in a stage, show information of a stage
WITH card_in_specific_stage AS (
    SELECT 
        Id AS card_id,
        Title AS card_title,
        [Location] AS card_location,
        StartDate AS card_start_date,
        DueDate AS card_due_date,
        CoverType,
        CoverValue,
        Position AS card_position,
        [Status],
        StageId
    FROM Cards
    WHERE StageId = 1
),
attachment_count_by_card AS (
    SELECT
        CardId, 
        COUNT(CardId) AS number_of_attachment
    FROM Attachments
    WHERE CardId in (SELECT card_id FROM card_in_specific_stage)
    GROUP BY CardId
),
checklist_item_count AS (
    SELECT 
        cl.CardId,
        COUNT(CardId) AS number_of_checklist_item
    FROM CheckLists cl
    JOIN CheckListItems cli ON cli.CheckListId = cl.Id
    WHERE CardId in (
        SELECT
            card_id
        FROM card_in_specific_stage
    )
    GROUP BY CardId
)

SELECT 
     ca.card_id,
     ca.card_title,
     ca.card_location,
     ca.card_start_date,
     ca.card_due_date,
     ca.CoverType,
     ca.CoverValue,
     card_position,
     ca.[Status],
     ca.StageId,
    at.number_of_attachment,
    ch.number_of_checklist_item
FROM card_in_specific_stage ca
JOIN attachment_count_by_card at ON at.CardId = ca.card_id
JOIN checklist_item_count ch ON ch.CardId = ca.card_id

--28 Slide 34. Select a board → Board page → in a stage, show picture of each user is member of specific card
SELECT
    me.card_id,
    us.PictureUrl AS user_picture
FROM (
    SELECT 
        UserId,
        OwnerId AS card_id
    FROM Members me
    JOIN OwnerTypes ot ON ot.Id = me.OwnerTypeId
    WHERE ot.[Value] = 'Card' AND me.OwnerId = 1 --OwnerId is CardId
) me
JOIN Users us ON us.Id = me.UserId;

--29. Slide 35. Select a board → Board page → in a stage → select a card, show labels of a card
SELECT
    ca.Id AS card_id,
    ca.Title AS card_title,
    la.Id AS [label_id],
    co.[Name] AS color_name,
    co.Icon AS color_icon
FROM (
    SELECT 
        CardId,
        LabelId
    FROM CardLabels
    WHERE CardId = 1
) cl
JOIN Cards ca ON ca.Id = cl.CardId
JOIN Labels la ON la.Id = cl.LabelId
JOIN Colors co ON co.Id = la.ColorId