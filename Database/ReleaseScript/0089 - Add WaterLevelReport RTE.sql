update dbo.CustomRichTextType
set CustomRichTextTypeName = 'WaterQualityReport',
	CustomRichTextTypeDisplayName = 'Water Quality Report'
where CustomRichTextTypeID = 18

insert into dbo.CustomRichTextType (CustomRichTextTypeID, CustomRichTextTypeName, CustomRichTextTypeDisplayName)
values (28, 'WaterLevelsReport', 'Water Levels Report')

insert into dbo.CustomRichText (CustomRichTextTypeID, CustomRichTextContent)
values (28, 'Sort and filter the wells in the grid to narrow your selection. Once you hit Generate, a Water Level Report will be created in the same order as the Well Groups listed in the grid. Download the grid to a CSV to create a mail merge for address labels.')