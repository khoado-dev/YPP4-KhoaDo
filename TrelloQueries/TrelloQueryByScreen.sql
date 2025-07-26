--1. Home Page on the Boards tab → Recently viewed section, list all boards that the user has accessed recently.
SELECT 
        bo.[Name] AS board_name, 
        bo.BackgroundUrl
FROM BoardUsers bu
JOIN Boards bo ON bo.Id = bu.BoardId
WHERE UserId = 1
ORDER BY bu.AccessedAt DESC

--2. Home Page on the Boards tab → Your Workspaces section, list all workspaces that the current user is a member of.
SELECT 
    wo.Name, 
    wo.LogoUrl
FROM Workspaces wo
JOIN Members me ON me.OwnerId = wo.Id
WHERE me.OwnerTypeId = 1 AND me.UserId = 1

--3. Home Page on the Boards tab → Workspace item → Boards button, list all boards  that the current user is a member of belonging to a specific workspace.
SELECT 
    bo.[Name] AS board_name, 
    bo.BackgroundUrl
FROM Boards bo
JOIN Members me ON me.OwnerId = bo.Id
WHERE bo.WorkspaceId = 1 
    AND me.UserId = 1 
    AND me.OwnerTypeId = 2

--4. Home Page on the Header (top right corner), query boards have name that contains the keyword 'ab'
SELECT 
    bo.[Name], 
    wo.[Name], 
    bo.[Status]
FROM Boards bo
JOIN Workspaces wo ON wo.Id = bo.Id
WHERE bo.[Name] LIKE '%ab%'

--5. Home Page on the Header (top right corner), show the total number of unread notifications of the user.
SELECT 
    COUNT(ac.UserId) AS number_of_notifications
FROM Notifications [no]
JOIN Activities ac ON ac.Id = no.ActivityId
WHERE ac.UserId = 2 AND no.[Status] = 'UNREAD'

--6. Home Page on the Templates tab → Main area, list all available public or user-created templates.
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
ORDER BY Viewed DESC, Copied DESC

--7. Home Page on the Templates tab → Sidebar, list all template categories available for filtering.
SELECT 
    IconUrl AS template_category_icon, 
    [Name] AS template_name
FROM TemplateCategories

--8. Home Page on the Templates tab, query templates have title that contains the keyword 'da' 
SELECT 
    bo.BackgroundUrl AS board_background, 
    us.Username AS created_by, 
    te.Title AS template_title
FROM Templates te
JOIN Users us ON us.Id = te.CreatedBy
JOIN Boards bo ON bo.Id = te.BoardId
WHERE Title LIKE '%da%'

--9. Home Page on the Home tab → Checklist section, list all checklist items assigned to the user with status set to false (incomplete).
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
WHERE cli.[Status] = 0 AND me.UserId = 1

--10. Home Page on the Home tab → Assigned cards section, list all cards that are currently assigned to the user.
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
ORDER BY day_ago
--11. Home Page on the Home tab → Activity feed section, list all recent card's activities in the user's card.
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
Order By day_ago

--12. Home Page on the Workspace page → Boards section, list all boards under the selected workspace.
SELECT *
FROM Boards bo
WHERE bo.WorkspaceId = 1

--13. Home Page on the Workspace page → Members section, list all members in the workspace along with their permission on roles.
SELECT *
FROM Members me
JOIN [Permissions] pe ON pe.Id = me.PermissionId
WHERE me.OwnerTypeId = 1
ORDER BY OwnerId

--14. Home Page on the Workspace page → Members section, count the total number of members in the selected workspace.
SELECT COUNT(*)
FROM Members me
JOIN [Permissions] pe ON pe.Id = me.PermissionId
WHERE me.OwnerTypeId = 1 AND me.OwnerId = 153

--15. Home Page on the Workspace page → Settings section, list all workspace setting keys with the current user's selected values.
SELECT 
    sk.KeyName, 
    COALESCE(sv.Value, sk.DefaultValue) AS Value,
    sv.OwnerId
FROM SettingKeys sk 
LEFT JOIN SettingValues sv ON sv.SettingKeyId = sk.Id AND sk.OwnerTypeId = 4 AND sv.OwnerId = 1

--16. Home Page on the Workspace page → Settings section, list all settingoption of a specific workspace setting.
SELECT *
FROM SettingKeySettingOptions sso
JOIN SettingKeys sk ON sk.Id = sso.SettingKeyId
JOIN SettingOptions so ON so.Id = sso.SettingOptionId
WHERE sk.Id = 68

--17. Home Page on the Workspace page → Upgrade section, list all available billing plans that the workspace can upgrade to.
SELECT * 
FROM BillingPlans

--18. Board Page on the Share Board pop-up, list all members in the board along with their permission on roles.
SELECT * 
FROM Members me
JOIN Boards bo ON bo.Id = me.OwnerId AND me.OwnerTypeId = 2
WHERE bo.Id = 1

--19. Board Page on the Share Board pop-up, list all permission options can choose
SELECT *
FROM [Permissions]

--20 Board Page on the Setting pop-up, list all board's setting key and user's choice of a specific user
SELECT sk.KeyName, COALESCE(sv.Value, sk.DefaultValue) as setting_value, OwnerId
FROM SettingKeys sk
LEFT JOIN SettingValues sv ON sv.SettingKeyId = sk.Id AND sk.OwnerTypeId = 2 AND sv.OwnerId = 1


