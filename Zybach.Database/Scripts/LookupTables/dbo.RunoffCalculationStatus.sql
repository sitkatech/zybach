MERGE INTO dbo.RunoffCalculationStatus AS Target
USING (VALUES
	(1, 'CalculationNotStarted', 'Calculation Not Started'),
	(2, 'InProgress', 'In Progress'),
	(3, 'Succeeded', 'Succeeded'),
	(4, 'Failed', 'Failed')
)
AS Source (RunoffCalculationStatusID, RunoffCalculationStatusName, RunoffCalculationStatusDisplayName)
ON Target.RunoffCalculationStatusID = Source.RunoffCalculationStatusID
WHEN MATCHED THEN
UPDATE SET
	RunoffCalculationStatusName = Source.RunoffCalculationStatusName,
	RunoffCalculationStatusDisplayName = Source.RunoffCalculationStatusDisplayName
WHEN NOT MATCHED BY TARGET THEN
	INSERT (RunoffCalculationStatusID, RunoffCalculationStatusName, RunoffCalculationStatusDisplayName)
	VALUES (RunoffCalculationStatusID, RunoffCalculationStatusName, RunoffCalculationStatusDisplayName)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;