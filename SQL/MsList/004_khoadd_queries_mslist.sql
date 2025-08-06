-- Query list of template based on specific Provider 
SELECT 
	lt.Id template_id,
	lt.HeaderImage,
	lt.Title Template_title,
	lt.TemplateDescription,
	tp.ProviderName
FROM ListTemplate lt
JOIN TemplateProvider tp ON tp.Id = lt.ProviderId
WHERE tp.ProviderName = 'Microsoft'

-- Query all cell in a template
SELECT 
	tc.Id template_column_id,
	tc.ColumnName,
	tsr.Id template_row_id,
	tsr.DisplayOrder,
	tsc.CellValue
FROM TemplateColumn tc
CROSS JOIN TemplateSampleRow tsr
LEFT JOIN TemplateSampleCell tsc ON tsc.TemplateColumnId = tc.Id AND tsc.TemplateSampleRowId = tsr.Id
WHERE tsr.ListTemplateId = tc.ListTemplateId AND tc.ListTemplateId = 1
ORDER BY tsr.DisplayOrder

-- Query comments of a specific row of list
SELECT
	ListRowId,
	ac.Avatar,
	ac.FirstName + ' ' + ac.LastName full_name,
	lrc.Content,
	COALESCE(lrc.UpdatedAt, lrc.CreatedAt) edited_at
FROM ListRowComment lrc
JOIN Account ac ON ac.Id = lrc.CreatedBy
WHERE ListRowId = 1

--Full Text Search in MS List
--SELECT FULLTEXTSERVICEPROPERTY('IsFullTextInstalled') AS IsFullTextInstalled; -- check wheather fultext search installed
--EXEC sp_helpindex 'TemplateSampleCell'; -- view pk name
--CREATE FULLTEXT CATALOG ftCatalog AS DEFAULT;

CREATE FULLTEXT INDEX ON TemplateSampleCell(CellValue)
KEY INDEX PK__Template__3214EC076D753957  -- tên PK hoặc unique index
ON ftCatalog;

SELECT *
FROM TemplateSampleCell
WHERE CONTAINS(CellValue, '"asset*" OR "row*"');

-- Paging List in MSList by row(limit the number of display row)
DECLARE @PageSize INT = 2;
DECLARE @PageIndex INT = 2;
SELECT 
    tc.Id AS template_column_id,
    tc.ColumnName,
    tsr.Id template_row_id,
    tsr.DisplayOrder,
    tsc.CellValue
FROM TemplateSampleRow tsr
CROSS JOIN TemplateColumn tc
LEFT JOIN TemplateSampleCell tsc 
    ON tsc.TemplateColumnId = tc.Id 
    AND tsc.TemplateSampleRowId = tsr.Id
WHERE tc.ListTemplateId = 1 AND tsr.ListTemplateId = 1
  AND tsr.DisplayOrder BETWEEN (@PageIndex - 1) * @PageSize + 1 AND @PageIndex * @PageSize
ORDER BY tsr.DisplayOrder;


-- PITVOT
DECLARE @PageSize INT = 2;
DECLARE @PageIndex INT = 2;

DECLARE @columns NVARCHAR(MAX);
SELECT @columns = STRING_AGG(QUOTENAME(ColumnName), ',')
FROM TemplateColumn
WHERE ListTemplateId = 1;

DECLARE @sql NVARCHAR(MAX) = '
SELECT *
FROM (
    SELECT 
        tc.ColumnName,
        tsr.Id AS template_row_id,
        tsc.CellValue,
        tsr.DisplayOrder
    FROM TemplateSampleRow tsr
    CROSS JOIN TemplateColumn tc
    LEFT JOIN TemplateSampleCell tsc 
        ON tsc.TemplateColumnId = tc.Id 
        AND tsc.TemplateSampleRowId = tsr.Id
    WHERE tsr.ListTemplateId = 1
      AND tc.ListTemplateId = 1
      AND tsr.DisplayOrder BETWEEN (@PageIndex - 1) * @PageSize + 1 AND @PageIndex * @PageSize
) AS Source
PIVOT (
    MAX(CellValue)
    FOR ColumnName IN (' + @columns + ')
) AS Pivoted
ORDER BY DisplayOrder;
';

EXEC sp_executesql @sql, 
    N'@PageSize INT, @PageIndex INT', 
    @PageSize = @PageSize, 
    @PageIndex = @PageIndex;

-- Paging List in MSList by column(limit the number of display column)

--CACHING
