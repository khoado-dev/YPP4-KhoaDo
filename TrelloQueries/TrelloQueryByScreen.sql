--1. Home Page Home Page Home Page on the Boards tab → Recently viewed section, list all boards that the user has accessed recently.
SELECT bu.UserId, bo.[Name] AS board_name
FROM BoardUsers bu
JOIN Boards bo ON bo.Id = bu.BoardId
WHERE UserId = 1
ORDER BY bu.AccessedAt DESC

--2. Home Page on the Boards tab → Your Workspaces section, list all workspaces that the current user is a member of.
SELECT *
FROM Workspaces wo
JOIN Members me ON me.OwnerId = wo.Id
WHERE me.OwnerTypeId = 1 AND me.UserId = 1

--3. Home Page on the Boards tab → Workspace item → Boards button, list all boards  that the current user is a member of belonging to a specific workspace.
SELECT *
FROM Boards bo
JOIN Members me ON me.OwnerId = bo.Id
WHERE bo.WorkspaceId = 1 and me.UserId = 1 AND me.OwnerTypeId = 2

--4. Home Page on the Header (top right corner), query boards have name that contains the keyword 'ab'
SELECT * FROM Boards WHERE [Name] LIKE '%ab%'

--5. Home Page on the Header (top right corner), show the total number of unread notifications of the user.
SELECT COUNT(*) AS number_of_notifications
FROM Notifications [no]
JOIN Activities ac ON ac.Id = no.ActivityId
WHERE ac.UserId = 2 AND no.[Status] = 'UNREAD'

--6. Home Page on the Templates tab → Main area, list all available public or user-created templates.
SELECT * 
FROM Templates
ORDER BY Viewed DESC, Copied DESC

--7. Home Page on the Templates tab → Sidebar, list all template categories available for filtering.
SELECT *
FROM TemplateCategories

--8. Home Page on the Templates tab, query templates have title that contains the keyword 'da' 
SELECT * FROM Templates WHERE Title LIKE '%da%'

--9. Home Page on the Home tab → Checklist section, list all checklist items assigned to the user with status set to false (incomplete).
SELECT * 
FROM CheckListItems cli
JOIN Members me ON me.Id = cli.MemberId
WHERE cli.[Status] = 0 AND me.UserId = 1

--10. Home Page on the Home tab → Assigned cards section, list all cards that are currently assigned to the user.
SELECT *
FROM Cards ca
JOIN Members me ON me.OwnerId = ca.Id
WHERE me.OwnerTypeId = 3 AND me.UserId = 1 

--11. Home Page on the Home tab → Activity feed section, list all recent activities performed by the user.
SELECT *
FROM Activities
WHERE UserId = 1
Order By CreatedAt DESC

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

--12. Home Page on the Workspace page → Members section, count the total number of members in the selected workspace.
SELECT COUNT(*)
FROM Members me
JOIN [Permissions] pe ON pe.Id = me.PermissionId
WHERE me.OwnerTypeId = 1 AND me.OwnerId = 153

--13. Home Page on the Workspace page → Settings section, list all workspace setting keys with the current user's selected values.
SELECT 
    sk.KeyName, 
    COALESCE(sv.Value, sk.DefaultValue) AS Value,
    sv.OwnerId
FROM SettingKeys sk 
LEFT JOIN SettingValues sv ON sv.SettingKeyId = sk.Id AND sk.OwnerTypeId = 4 AND sv.OwnerId = 1

--14. Home Page on the Workspace page → Upgrade section, list all available billing plans that the workspace can upgrade to.
SELECT * 
FROM BillingPlans
