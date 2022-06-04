MERGE INTO dbo.MeasurementType AS Target
USING (VALUES
(1, 'FlowMeter', 'Flow Meter', 'gallons', 'pumped'),
(2, 'ContinuityMeter', 'Continuity Meter', 'continuity', 'on'),
(3, 'ElectricalUsage', 'Electrical Usage', null, null),
(4, 'WellPressure', 'Well Pressure', 'depth', 'water-bgl'),
(5, 'BatteryVoltage', 'Battery Voltage', 'battery-voltage', 'millivolts')
)
AS Source (MeasurementTypeID, MeasurementTypeName, MeasurementTypeDisplayName, InfluxMeasurementName, InfluxFieldName)
ON Target.MeasurementTypeID = Source.MeasurementTypeID
WHEN MATCHED THEN
UPDATE SET
	MeasurementTypeName = Source.MeasurementTypeName,
	MeasurementTypeDisplayName = Source.MeasurementTypeDisplayName,
	InfluxMeasurementName = Source.InfluxMeasurementName, 
	InfluxFieldName = Source.InfluxFieldName
WHEN NOT MATCHED BY TARGET THEN
	INSERT (MeasurementTypeID, MeasurementTypeName, MeasurementTypeDisplayName, InfluxMeasurementName, InfluxFieldName)
	VALUES (MeasurementTypeID, MeasurementTypeName, MeasurementTypeDisplayName, InfluxMeasurementName, InfluxFieldName)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;
