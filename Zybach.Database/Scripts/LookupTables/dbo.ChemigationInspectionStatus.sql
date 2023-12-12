MERGE INTO dbo.ChemigationInspectionStatus AS Target
USING (VALUES
(1, 'Pending', 'Pending'),
(2, 'Pass', 'Pass'),
(3, 'Fail', 'Fail')
)
AS Source (ChemigationInspectionStatusID, ChemigationInspectionStatusName, ChemigationInspectionStatusDisplayName)
ON Target.ChemigationInspectionStatusID = Source.ChemigationInspectionStatusID
WHEN MATCHED THEN
UPDATE SET
	ChemigationInspectionStatusName = Source.ChemigationInspectionStatusName,
	ChemigationInspectionStatusDisplayName = Source.ChemigationInspectionStatusDisplayName
WHEN NOT MATCHED BY TARGET THEN
	INSERT (ChemigationInspectionStatusID, ChemigationInspectionStatusName, ChemigationInspectionStatusDisplayName)
	VALUES (ChemigationInspectionStatusID, ChemigationInspectionStatusName, ChemigationInspectionStatusDisplayName)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;
