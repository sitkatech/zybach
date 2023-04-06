MERGE INTO dbo.ReportTemplateModel AS Target
USING (VALUES
(1, 'ChemigationPermit', 'Chemigation Permit', 'Templates will be with the "ChemigationPermit" model.'),
(2, 'WellWaterQualityInspection', 'Well Water Quality Inspection', 'Templates will be with the "Well" model.'),
(3, 'WellGroupWaterLevelInspection', 'Well Water Level Inspection', 'Templates will be with the "WellGroup" model.')
)
AS Source (ReportTemplateModelID, ReportTemplateModelName, ReportTemplateModelDisplayName, ReportTemplateModelDescription)
ON Target.ReportTemplateModelID = Source.ReportTemplateModelID
WHEN MATCHED THEN
UPDATE SET
	ReportTemplateModelName = Source.ReportTemplateModelName,
	ReportTemplateModelDisplayName = Source.ReportTemplateModelDisplayName,
	ReportTemplateModelDescription = Source.ReportTemplateModelDescription
WHEN NOT MATCHED BY TARGET THEN
	INSERT (ReportTemplateModelID, ReportTemplateModelName, ReportTemplateModelDisplayName, ReportTemplateModelDescription)
	VALUES (ReportTemplateModelID, ReportTemplateModelName, ReportTemplateModelDisplayName, ReportTemplateModelDescription)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;