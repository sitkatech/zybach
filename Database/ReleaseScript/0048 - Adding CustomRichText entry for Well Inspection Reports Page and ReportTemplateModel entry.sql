Insert into dbo.CustomRichTextType (CustomRichTextTypeID, CustomRichTextTypeName, CustomRichTextTypeDisplayName)
values

(18, 'WellInspectionReports', 'Well Inspection Reports')

Insert into dbo.CustomRichText(CustomRichTextTypeID, CustomRichTextContent)
values

(18, 'Default content for: Well Inspection Reports')

insert INTO dbo.ReportTemplateModel(ReportTemplateModelID, ReportTemplateModelName, ReportTemplateModelDisplayName, ReportTemplateModelDescription)
values
(2, 'WellWaterQualityInspection', 'Well Water Quality Inspection', 'Templates will be with the "Well" model.')