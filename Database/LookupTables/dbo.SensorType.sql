MERGE INTO dbo.SensorType AS Target
USING (VALUES
(1, 'FlowMeter', 'Flow Meter'),
(2, 'PumpMonitor', 'Continuity Meter'),
(3, 'WellPressure', 'Well Pressure')
)
AS Source (SensorTypeID, SensorTypeName, SensorTypeDisplayName)
ON Target.SensorTypeID = Source.SensorTypeID
WHEN MATCHED THEN
UPDATE SET
	SensorTypeName = Source.SensorTypeName,
	SensorTypeDisplayName = Source.SensorTypeDisplayName
WHEN NOT MATCHED BY TARGET THEN
	INSERT (SensorTypeID, SensorTypeName, SensorTypeDisplayName)
	VALUES (SensorTypeID, SensorTypeName, SensorTypeDisplayName)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;
