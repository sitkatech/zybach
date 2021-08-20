MERGE INTO dbo.MeasurementType AS Target
USING (VALUES
(1, 'FlowMeter', 'Flow Meter'),
(2, 'ContinuityMeter', 'Continuity Meter'),
(3, 'ElectricalUsage', 'Electrical Usage')
)
AS Source (MeasurementTypeID, MeasurementTypeName, MeasurementTypeDisplayName)
ON Target.MeasurementTypeID = Source.MeasurementTypeID
WHEN MATCHED THEN
UPDATE SET
	MeasurementTypeName = Source.MeasurementTypeName,
	MeasurementTypeDisplayName = Source.MeasurementTypeDisplayName
WHEN NOT MATCHED BY TARGET THEN
	INSERT (MeasurementTypeID, MeasurementTypeName, MeasurementTypeDisplayName)
	VALUES (MeasurementTypeID, MeasurementTypeName, MeasurementTypeDisplayName)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;
