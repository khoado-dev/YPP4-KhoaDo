USE GoogleDrive
GO

 -- Home screen
 -- 1.select User information
 Declare @userId int = 1
 select 
	a.UserName as UserName,
	a.Email as Email
 from Account a
 where a.UserId =@userId 


-- 2.select User Setting
Declare @userId int = 1;
select 
	su.SettingUserId,
	a.UserName as UserName,
	s.SettingKey,
	s.SettingValue
from SettingUser su
join Account a on su.UserId = a.UserId
join AppSetting s on su.SettingId = s.SettingId
where a.UserId =@userId

-- 3.Recomment file
Declare @userId int = 2
select top 10
	f.FileId,
	a.UserName,
	f.UserFileName,
	ar.ActionLog,
	ar.ActionDateTime
from ActionRecent ar 
join Account a on ar.UserId = a.UserId
join UserFile f on ar.ObjectTypeId = 2 and ar.ObjectId = f.FileId
where ar.UserId = @userId
order by ar.ActionDateTime DESC

select * from ActionRecent


-- 4.Recomment folder
Declare @userId int = 3
select top 10
	fo.FolderId,
	a.UserName,
	fo.FolderName,
	ar.ActionLog,
	ar.ActionDateTime
from ActionRecent ar
join Account a on ar.UserId = a.UserId
join Folder fo on ar.ObjectTypeId = 1 and ar.ObjectId = fo.FolderId
where ar.UserId = @userId  
order by ar.ActionDateTime DESC

select * from ActionRecent
--- My Drive Screen
-- 1.select login user file
Declare @LoginUser int = 1 
select 
	uf.FileId,
	a.UserName,
	uf.UserFileName
from UserFile uf 
join Account a on uf.OwnerId = a.UserId
where a.UserId =@LoginUser


-- 2.select login user folder
Declare @userId int = 1
select 
	fo.FolderId,
	a.UserName,
	fo.FolderName
from Folder fo
join Account a on fo.OwnerId = a.UserId
where a.UserId =@userId

--- Share with me Screen
-- 1.select shared file with login user
Declare @userId int = 102
select
	f.FileId,
	a.UserName,
	f.UserFileName
from SharedUser su
join Account a on su.UserId = a.UserId
join Share s on su.ShareId = s.ShareId
join UserFile f on s.ObjectTypeId = 2 and s.ObjectId = f.FileId
where su.UserId = @userId


select * from Share
select * from Shareduser where shareId =2


-- 2.select shared folder with login user
Declare @userId int = 101
select
	fo.FolderId,
	a.UserName,
	fo.FolderName
from SharedUser su
join Account a on su.UserId = a.UserId
join Share s on su.ShareId = s.ShareId
join [Folder] fo on s.ObjectTypeId =1 and s.ObjectId = fo.FolderId
where su.UserId = @userId

select * from share where ObjectTypeId =1
select * from SharedUser where ShareId = 1

--- trash screen
-- 1.select file have been deleted
Declare @userId int = 704
 SELECT
	f.FileId,
    t.TrashId,
    ot.ObjectTypeName,
    f.UserFileName,
    t.RemovedDatetime,
    t.IsPermanent
FROM Trash t
JOIN ObjectType ot ON t.ObjectTypeId = ot.ObjectTypeId
JOIN UserFile f ON t.ObjectTypeId = 2 AND t.ObjectId = f.FileId
WHERE t.UserId = @userId;

select * from Trash


-- 2.select folder have been deleted
Declare @userId int = 1
 SELECT 
	fo.FolderId,
    t.TrashId,
    ot.ObjectTypeName,
    fo.FolderName,
    t.RemovedDatetime,
    t.IsPermanent
FROM Trash t
JOIN ObjectType ot ON t.ObjectTypeId = ot.ObjectTypeId
JOIN Folder fo ON t.ObjectTypeId = 1 AND t.ObjectId = fo.FolderId
WHERE t.UserId = @userId;


--- Stared screen
-- 1. Select file
Declare @userId int = 794
select 
	f.FileId,
	f.UserFileName,
	a.USerName as FileOwnerName,
	a.UserId as UserId,
	ft.FileTypeName
from FavoriteObject fa
left join UserFile f on fa.ObjectTypeId = 2 and fa.ObjectId = f.FileId
left join Account a on f.OwnerId = a.UserId 
join FileType ft on f.FileTypeId = ft.FileTypeId
where fa.OwnerId = @userId

select * from FavoriteObject where OwnerId = 794
select * from UserFile f where f.FileId = 875


--product Screen
-- select all of product
select
	ProductId,
	ProductName,
	Cost,
	Duration
from ProductItem


-- select Product bought by user
Declare @userId int = 100
select 
	pro.ProductId,
	pro.ProductName,
	a.UserId,
	a.UserName,
	case
		when po.IsPercent = 1 then pro.Cost - (pro.Cost * (po.Discount/100))
		else pro.Cost - po.Discount
	end as TotalCost
from UserProduct up
join Account a on up.UserId = a.UserId
join Promotion po on up.PromotionId = po.PromotionId
join ProductItem pro on up.ProductId = pro.ProductId
where up.UserId = @userId

select * from Promotion where PromotionId = 1 or PromotionId = 4
select * from ProductItem
select * from UserProduct up where up.UserId = 100


-- select Top 10 Payers 
select top 10
	a.UserId,
	pro.ProductName,
	a.UserName,
	case
		when po.IsPercent = 1 then pro.Cost * (po.Discount/100)
		else pro.Cost - po.Discount
	end as TotalCost
from UserProduct up
join Account a on up.UserId = a.UserId
join ProductItem pro on up.ProductId = pro.ProductId
join Promotion po on up.PromotionId = po.PromotionId
order by TotalCost DESC


-- select folder shared for user with userid = 101
Declare @userId int = 101
select 
	a.UserName,
	fo.FolderId,
	p.PermissionName,
	fo.FolderName
from SharedUser su
join Account a on su.SharedUserId = a.UserId
join Share s on su.ShareId = s.ShareId
join Permission p on su.PermissionId = p.PermissionId
LEFT join Folder fo on s.ObjectTypeId = 1 and fo.FolderId = s.ObjectId
where su.UserId = @userId

-- select file shared for user with userid = 101
Declare @userId int = 102
select 
	a.UserName,
	p.PermissionName,
	f.FileId,
	f.UserFileName
from SharedUser su
join Account a on su.SharedUserId = a.UserId
join Share s on su.ShareId = s.ShareId
join Permission p on su.PermissionId = p.PermissionId
LEFT join UserFile f on s.ObjectTypeId = 2 and f.FileId = s.ObjectId
where su.UserId = @userId


select * from SharedUser su where su.ShareId = 2
select * from share s where s.ShareId = 1
select * from UserFile f where f.FileId = 298
select * from Account where UserId = 101


--select top 5 largest file of login-user 
DECLARE @userId int = 4
select distinct top 5
	f.UserFileName,
	a.UserName as OwnerName,
	f.Size as FileSize
from UserFile f
join Account a on f.OwnerId = a.UserId
where f.OwnerId = @userid
order by f.Size DESC

select * from UserFile where OwnerId =4

--select banned user of login-user
DECLARE @userId int = 533
SELECT 
    BU.Id AS BanId,
    Banned.UserName AS BannedUserName,
    Banner.UserName AS BannedByUserName,
    BU.BannedAt
FROM BannedUser BU
JOIN Account Banned ON BU.UserId = Banned.UserId
JOIN Account Banner ON BU.BannedUserId = Banner.UserId
where BU.UserId = @userId
ORDER BY BU.BannedAt DESC;


--select product bought by user 
Declare @userId int = 100
select 
	p.ProductId,
	p.ProductName,
	a.UserName,
	up.PayingDatetime,
	up.EndDatetime,
	pr.PromotionName
from UserProduct up
join ProductItem p on up.ProductId = p.ProductId
join Account a on up.UserId = a.UserId
join Promotion pr on up.PromotionId = pr.PromotionId
where a.UserId = @userId


-- select file share by user
Declare @userId int = 100
select 
	a.UserName,
	f.UserFileName
from Share s
join Account a on s.Sharer = a.UserId
join ObjectType ot on s.ObjectTypeId = ot.ObjectTypeId
left join UserFile f on s.ObjectTypeId = 2 and s.ObjectId = f.FileId
where s.Sharer = @userId


-- select folder share by user
Declare @userId int = 1
select 
	a.UserName,
	fo.FolderName
from Share s
join Account a on s.Sharer = a.UserId
join ObjectType ot on s.ObjectTypeId = ot.ObjectTypeId
left join [Folder] fo on s.ObjectTypeId =1 and s.ObjectId = fo.FolderId
where s.Sharer = @userId

select count(*)
from Share
group by Share.Sharer

select *
from Share
where ObjectId = 654


-- select user was shared object with objectId = 5 and objectType is folder
Declare @objectId int = 5
select 
	a.UserName,
	fo.FolderName,
	p.PermissionName
from Share s
	join SharedUser su on s.ShareId = su.ShareId
	join Account a on su.UserId = a.UserId
	join Permission p on su.PermissionId = p.PermissionId
	left join Folder fo on s.ObjectTypeId =1 and s.ObjectId = fo.FolderId
where s.ObjectId = @objectId


select *
from UserFile fm
where f.FileId = 654

select *
from [Folder] f
where f.FolderId = 654


--User Management: Retrieve the names and email addresses of all users who have used more than 50% of their storage capacity.
select top 5
	a.UserName,
	a.Email as UserEmail,
	(a.capacity - a.UsedCapacity) as AllowCapicity
from Account a
where ((CAST(a.UsedCapacity AS FLOAT) / a.Capacity) * 100) > 50

select * from Account


--Folder Structure: List all folders owned by a specific user (e.g., UserId = 1), including their full path and the name of the color associated with each folder.
Declare @userId int = 20
select 
	fo.FolderPath,
	c.ColorName,
	a.UserName
from [Folder] fo
join Account a on fo.OwnerId = a.UserId
join Color c on fo.ColorId = c.ColorId
where fo.OwnerId = @userId


---Select childrent of folder
Declare @FolderId int = 1;
WITH RecursiveFolders AS (
    -- Anchor member: Start with FolderId = 1
    SELECT FolderId, FolderName, ParentId, FolderPath
    FROM Folder
    WHERE FolderId = @FolderId
    UNION ALL
    -- Recursive member: Join with Folder to get all descendants
    SELECT f.FolderId, f.FolderName, f.ParentId, f.FolderPath
    FROM Folder f
    INNER JOIN RecursiveFolders rf ON f.ParentId = rf.FolderId
    WHERE f.FolderPath LIKE rf.FolderPath + '/%'
)
SELECT 
    rf.FolderName,
    rf.ParentId,
    rf.FolderPath,
    fo.FolderName AS ParentFolderName
FROM RecursiveFolders rf
LEFT JOIN Folder fo ON rf.ParentId = fo.FolderId
WHERE rf.FolderId != 1 -- Exclude the root folder itself, only show subfolders
ORDER BY rf.FolderPath;


-- Full-text search query with token
SELECT 
    uf.FileId,
    uf.UserFileName,
    s.Term,
    s.TermFrequency,
	s.TermPositions
FROM SearchIndex s
JOIN FileContent fc on s.FileContentId = fc.ContentId
join UserFile uf on fc.FileId = uf.FileId
WHERE s.Term IN ('project', 'proposal','employ')
order by s.Bm25Score

-- Full-text search query with string
DECLARE @TextQuery nvarchar(50) = 'software';

WITH TokenizedQuery AS (
    SELECT Term
    FROM dbo.fn_TokenizeText(@TextQuery)
),
AllMatches AS (
    SELECT s.FileContentId, COUNT(*) as MatchingTerms
    FROM SearchIndex s
    JOIN TokenizedQuery tq ON s.Term = tq.Term
    GROUP BY s.FileContentId
    HAVING COUNT(*) > 0
)
SELECT 
    fc.FileId,
	uf.UserFileName,
    fc.ContentChunk,
    SUM(s.Bm25Score) AS TotalBm25Score,
    COUNT(s.Term) AS MatchedTerms,
    (SELECT COUNT(*) FROM TokenizedQuery) AS TotalQueryTerms
FROM AllMatches am
JOIN FileContent fc ON am.FileContentId = fc.ContentId
JOIN SearchIndex s ON s.FileContentId = fc.ContentId
join UserFile uf on fc.FileId = uf.FileId
WHERE s.Term IN (SELECT Term FROM TokenizedQuery)
GROUP BY fc.FileId, fc.ContentChunk, uf.UserFileName
ORDER BY TotalBm25Score DESC;

-- thêm column bm25_score and table term

select 
	uf.UserFileName
from UserFile uf
where FileId = 1

---Sort UserFile By ShareUser where UserId = 2---
declare @Sharer int = 2
declare @shared int = 102
select 
	uf.FileId,
	ft.Icon,
	uf.UserFileName as NameOfFile,
	a1.UserName as SharerName,
	s.CreatedAt as ShareDateTime,
	a.UserName as sharedName
from SharedUser su
join Share s on su.ShareId = s.ShareId
left join UserFile uf on s.ObjectTypeId = 2 and s.ObjectId = uf.FileId
join FileType ft on uf.FileTypeId = ft.FileTypeId
join Account a on su.UserId = a.UserId
join Account a1 on s.Sharer = a1.UserId
where su.UserId = @shared and s.Sharer = @Sharer

select * from SharedUser

---sort UserFile by FileType---
declare @OwnerId int = 1
declare @FileType int = 3

Select 
	uf.FileId,
	ft.Icon,
	uf.UserFileName,
	a.UserName as OwnerName,
	uf.CreatedAt
from UserFile uf
join FileType ft on uf.FileTypeId = ft.FileTypeId
join Account a on uf.OwnerId = a.UserId
where uf.FileTypeId = @FileType and uf.OwnerId = @OwnerId

---Sort by Action recent---
WITH RecentObjects AS (

    SELECT 
        ar.ObjectId,
        ar.ObjectTypeId,
        ar.ActionDateTime,
		ar.ActionLog,
        CASE 
            WHEN ar.ObjectTypeId = 1 THEN 'Folder'
            WHEN ar.ObjectTypeId = 2 THEN 'File'
            ELSE 'Unknown'
        END AS ObjectType,
        uf.OwnerId AS FileOwnerId,
        f.OwnerId AS FolderOwnerId
    FROM ActionRecent ar
    LEFT JOIN UserFile uf ON ar.ObjectId = uf.FileId AND ar.ObjectTypeId = 2
    LEFT JOIN Folder f ON ar.ObjectId = f.FolderId AND ar.ObjectTypeId = 1
    WHERE (uf.OwnerId = 11 OR f.OwnerId = 11)
)
SELECT 
    ro.ObjectId,
    ro.ObjectTypeId,
    ro.ObjectType,
    CASE 
        WHEN ro.ObjectType = N'File' THEN uf.UserFileName
        WHEN ro.ObjectType = N'Folder' THEN f.FolderName
        ELSE NULL
    END AS ObjectName,
	ro.ActionLog,
    ro.ActionDateTime
FROM RecentObjects ro
LEFT JOIN UserFile uf ON ro.ObjectId = uf.FileId AND ro.ObjectTypeId = 2
LEFT JOIN Folder f ON ro.ObjectId = f.FolderId AND ro.ObjectTypeId = 1
WHERE ro.FileOwnerId =11 OR ro.FolderOwnerId = 11
ORDER BY ro.ActionDateTime DESC;

select * from ActionRecent
select * from UserFile
select * from Folder where OwnerId = 11 and FolderId= 37