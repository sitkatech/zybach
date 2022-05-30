MERGE INTO dbo.ChemigationLowPressureValve AS Target
USING (VALUES
(1, 'RubberDam', 'Rubber Dam'),
(2, 'SpringLoaded', 'Spring Loaded')
)
AS Source (ChemigationLowPressureValveID, ChemigationLowPressureValveName, ChemigationLowPressureValveDisplayName)
ON Target.ChemigationLowPressureValveID = Source.ChemigationLowPressureValveID
WHEN MATCHED THEN
UPDATE SET
	ChemigationLowPressureValveName = Source.ChemigationLowPressureValveName,
	ChemigationLowPressureValveDisplayName = Source.ChemigationLowPressureValveDisplayName
WHEN NOT MATCHED BY TARGET THEN
	INSERT (ChemigationLowPressureValveID, ChemigationLowPressureValveName, ChemigationLowPressureValveDisplayName)
	VALUES (ChemigationLowPressureValveID, ChemigationLowPressureValveName, ChemigationLowPressureValveDisplayName)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;
