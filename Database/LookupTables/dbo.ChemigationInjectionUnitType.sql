MERGE INTO dbo.ChemigationInjectionUnitType AS Target
USING (VALUES
(1, 'Portable', 'Portable'),
(2, 'Stationary', 'Stationary')
)
AS Source (ChemigationInjectionUnitTypeID, ChemigationInjectionUnitTypeName, ChemigationInjectionUnitTypeDisplayName)
ON Target.ChemigationInjectionUnitTypeID = Source.ChemigationInjectionUnitTypeID
WHEN MATCHED THEN
UPDATE SET
	ChemigationInjectionUnitTypeName = Source.ChemigationInjectionUnitTypeName,
	ChemigationInjectionUnitTypeDisplayName = Source.ChemigationInjectionUnitTypeDisplayName
WHEN NOT MATCHED BY TARGET THEN
	INSERT (ChemigationInjectionUnitTypeID, ChemigationInjectionUnitTypeName, ChemigationInjectionUnitTypeDisplayName)
	VALUES (ChemigationInjectionUnitTypeID, ChemigationInjectionUnitTypeName, ChemigationInjectionUnitTypeDisplayName)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;
