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
