MERGE INTO dbo.MeasurementType AS Target
USING (VALUES
(1, 'FlowMeter', 'Flow Meter', 'gallons', 'pumped', 'gallon', 'gallons'),
(2, 'ContinuityMeter', 'Continuity Meter', 'continuity', 'on', 'gallon', 'gallons'),
(3, 'ElectricalUsage', 'Electrical Usage', null, null, 'gallon', 'gallons'),
(4, 'WellPressure', 'Well Pressure', 'depth', 'water-bgl', 'foot', 'feet'),
(5, 'BatteryVoltage', 'Battery Voltage', 'battery-voltage', 'millivolts', 'millivolt', 'millivolts')
)
AS Source (MeasurementTypeID, MeasurementTypeName, MeasurementTypeDisplayName, InfluxMeasurementName, InfluxFieldName, UnitsDisplay, UnitsDisplayPlural)
ON Target.MeasurementTypeID = Source.MeasurementTypeID
WHEN MATCHED THEN
UPDATE SET
	MeasurementTypeName = Source.MeasurementTypeName,
	MeasurementTypeDisplayName = Source.MeasurementTypeDisplayName,
	InfluxMeasurementName = Source.InfluxMeasurementName, 
	InfluxFieldName = Source.InfluxFieldName,
    UnitsDisplay = Source.UnitsDisplay, 
    UnitsDisplayPlural = Source.UnitsDisplayPlural
WHEN NOT MATCHED BY TARGET THEN
	INSERT (MeasurementTypeID, MeasurementTypeName, MeasurementTypeDisplayName, InfluxMeasurementName, InfluxFieldName, UnitsDisplay, UnitsDisplayPlural)
	VALUES (MeasurementTypeID, MeasurementTypeName, MeasurementTypeDisplayName, InfluxMeasurementName, InfluxFieldName, UnitsDisplay, UnitsDisplayPlural)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;
