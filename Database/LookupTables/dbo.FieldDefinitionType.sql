MERGE INTO dbo.FieldDefinitionType AS Target
USING (VALUES
(1, 'Name', 'Name'),
(2, 'HasWaterLevelInspections', 'Has Water Level Inspections?'),
(3, 'HasWaterQualityInspections', 'Has Water Quality Inspections?'),
(4, 'LatestWaterLevelInspectionDate', 'Latest Water Level Inspection Date'),
(5, 'LatestWaterQualityInspectionDate', 'Latest Water Quality Inspection Date'),
(6, 'WellRegistrationNumber', 'Well Registration #'),
(7, 'WellNickname', 'Well Nickname'),
(8, 'AgHubRegisteredUser', 'AgHub Registered User'),
(9, 'WellFieldName', 'Field Name'),
(10, 'IrrigationUnitID', 'Irrigation Unit ID'),
(11, 'WellIrrigatedAcres', 'Irrigated Acres'),
(12, 'WellChemigationInspectionParticipation', 'Requires Chemigation Inspections?'),
(13, 'WellWaterLevelInspectionParticipation', 'Requires Water Level Inspections?'),
(14, 'WellWaterQualityInspectionParticipation', 'Water Quality Inspection Type'),
(15, 'WellProgramParticipation', 'Well Participation'),
(16, 'WellOwnerName', 'Owner'),
(17, 'SensorLastMessageAgeHours', 'Last Message Age (Hours)'),
(18, 'SensorLastVoltageReading', 'Last Voltage Reading (mV)'),
(19, 'SensorFirstReadingDate', 'First Reading Date'),
(20, 'SensorLastReadingDate', 'Last Reading Date'),
(21, 'SensorStatus', 'Status'),
(22, 'SensorType', 'Sensor Type'),
(23, 'IrrigationUnitAcres', 'Irrigation Unit Area (ac)'),
(24, 'SensorLastVoltageReadingDate', 'Last Voltage Reading Date')
(25, 'SensorRetirementDate', 'Sensor Retirement Date')
)
AS Source (FieldDefinitionTypeID, FieldDefinitionTypeName, FieldDefinitionTypeDisplayName)
ON Target.FieldDefinitionTypeID = Source.FieldDefinitionTypeID
WHEN MATCHED THEN
UPDATE SET
	FieldDefinitionTypeName = Source.FieldDefinitionTypeName,
	FieldDefinitionTypeDisplayName = Source.FieldDefinitionTypeDisplayName
WHEN NOT MATCHED BY TARGET THEN
	INSERT (FieldDefinitionTypeID, FieldDefinitionTypeName, FieldDefinitionTypeDisplayName)
	VALUES (FieldDefinitionTypeID, FieldDefinitionTypeName, FieldDefinitionTypeDisplayName)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;
