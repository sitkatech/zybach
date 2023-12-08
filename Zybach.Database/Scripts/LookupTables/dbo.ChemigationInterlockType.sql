MERGE INTO dbo.ChemigationInterlockType AS Target
USING (VALUES
(1, 'Mechanical', 'Mechanical'),
(2, 'Electrical', 'Electrical')
)
AS Source (ChemigationInterlockTypeID, ChemigationInterlockTypeName, ChemigationInterlockTypeDisplayName)
ON Target.ChemigationInterlockTypeID = Source.ChemigationInterlockTypeID
WHEN MATCHED THEN
UPDATE SET
	ChemigationInterlockTypeName = Source.ChemigationInterlockTypeName,
	ChemigationInterlockTypeDisplayName = Source.ChemigationInterlockTypeDisplayName
WHEN NOT MATCHED BY TARGET THEN
	INSERT (ChemigationInterlockTypeID, ChemigationInterlockTypeName, ChemigationInterlockTypeDisplayName)
	VALUES (ChemigationInterlockTypeID, ChemigationInterlockTypeName, ChemigationInterlockTypeDisplayName)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;
