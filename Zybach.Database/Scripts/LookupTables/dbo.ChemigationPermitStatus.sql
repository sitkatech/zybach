MERGE INTO dbo.ChemigationPermitStatus AS Target
USING (VALUES
(1, 'Active', 'Active'),
(2, 'Inactive', 'Inactive'),
(3, 'PermInactive', 'Permanently Inactive')
)
AS Source (ChemigationPermitStatusID, ChemigationPermitStatusName, ChemigationPermitStatusDisplayName)
ON Target.ChemigationPermitStatusID = Source.ChemigationPermitStatusID
WHEN MATCHED THEN
UPDATE SET
	ChemigationPermitStatusName = Source.ChemigationPermitStatusName,
	ChemigationPermitStatusDisplayName = Source.ChemigationPermitStatusDisplayName
WHEN NOT MATCHED BY TARGET THEN
	INSERT (ChemigationPermitStatusID, ChemigationPermitStatusName, ChemigationPermitStatusDisplayName)
	VALUES (ChemigationPermitStatusID, ChemigationPermitStatusName, ChemigationPermitStatusDisplayName)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;
