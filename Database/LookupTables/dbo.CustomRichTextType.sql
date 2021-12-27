MERGE INTO dbo.CustomRichTextType AS Target
USING (VALUES
(1, 'Platform Overview', 'Platform Overview'),
(2, 'Disclaimer', 'Disclaimer'),
(3, 'Home page', 'Home page'),
(4, 'Help', 'Help'),
(5, 'LabelsAndDefinitionsList', 'Labels and Definitions List'),
(6, 'Training', 'Training'),
(7, 'RobustReviewScenario', 'Robust Review Scenario'),
(8, 'ReportsList', 'Reports List'),
(9, 'Chemigation', 'Chemigation'),
(10, 'NDEEChemicalsReport', 'NDEE Chemicals Report'),
(11, 'ChemigationPermitReport', 'Chemigation Permit Report'),
(12, 'ChemigationInspections', 'Chemigation Inspections'),
(13, 'WaterQualityInspections', 'Water Quality Inspections')
)
AS Source (CustomRichTextTypeID, CustomRichTextTypeName, CustomRichTextTypeDisplayName)
ON Target.CustomRichTextTypeID = Source.CustomRichTextTypeID
WHEN MATCHED THEN
UPDATE SET
	CustomRichTextTypeName = Source.CustomRichTextTypeName,
	CustomRichTextTypeDisplayName = Source.CustomRichTextTypeDisplayName
WHEN NOT MATCHED BY TARGET THEN
	INSERT (CustomRichTextTypeID, CustomRichTextTypeName, CustomRichTextTypeDisplayName)
	VALUES (CustomRichTextTypeID, CustomRichTextTypeName, CustomRichTextTypeDisplayName)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;
