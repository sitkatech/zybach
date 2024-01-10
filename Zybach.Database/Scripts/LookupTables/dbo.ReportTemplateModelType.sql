MERGE INTO dbo.ReportTemplateModelType AS Target
USING (VALUES
(1, 'SingleModel', 'Single Model', 'Reports with the "Single Model" model type will be provided a single model per report. If multiple elements are selected and generated with this model type, the template will be run for each of the elements and then joined into a final word document.'),
(2, 'MultipleModels', 'Multiple Models', 'Reports with the "Multiple Models" model type will be provided with a list of the elements selected for the report. The template will be run once, but will have access to all of the models selected for iteration within it.')
)
AS Source (ReportTemplateModelTypeID, ReportTemplateModelTypeName, ReportTemplateModelTypeDisplayName, ReportTemplateModelTypeDescription)
ON Target.ReportTemplateModelTypeID = Source.ReportTemplateModelTypeID
WHEN MATCHED THEN
UPDATE SET
	ReportTemplateModelTypeName = Source.ReportTemplateModelTypeName,
	ReportTemplateModelTypeDisplayName = Source.ReportTemplateModelTypeDisplayName,
	ReportTemplateModelTypeDescription = Source.ReportTemplateModelTypeDescription
WHEN NOT MATCHED BY TARGET THEN
	INSERT (ReportTemplateModelTypeID, ReportTemplateModelTypeName, ReportTemplateModelTypeDisplayName, ReportTemplateModelTypeDescription)
	VALUES (ReportTemplateModelTypeID, ReportTemplateModelTypeName, ReportTemplateModelTypeDisplayName, ReportTemplateModelTypeDescription)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;
