insert into dbo.FieldDefinitionType (FieldDefinitionTypeID, FieldDefinitionTypeName, FieldDefinitionTypeDisplayName)
values
(6, 'WellRegistrationNumber', 'Well Registration #'),
(7, 'WellNickname', 'Well Nickname'),
(8, 'AgHubRegisteredUser', 'AgHub Registered User'),
(9, 'WellFieldName', 'Field Name'),
(10, 'IrrigationUnitID', 'Irrigation Unit ID'),
(11, 'IrrigatedAcres', 'Irrigated Acres'),
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
(22, 'SensorType', 'Sensor Type')


insert into dbo.FieldDefinition (FieldDefinitionTypeID, FieldDefinitionValue)
values
(6, 'Well Registration #'),
(7, 'Well Nickname'),
(8, 'AgHub Registered User'),
(9, 'Field Name: A nickname for the associated field'),
(10, 'Irrigation Unit ID: Unique ID for an irrigation unit; each unit has one or more associated wells'),
(11, 'Irrigated Acres: Acres irrigated by this well'),
(12, 'Requires Chemigation Inspections?'),
(13, 'Requires Water Level Inspections?'),
(14, 'Water Quality Inspection Type: Full-panel or Nitrates Only'),
(15, 'Well Participation: Specify program this well participates in'),
(16, 'Well Owner'),
(17, 'Last Message Age (Hours)'),
(18, 'Last Voltage Reading (mV)'),
(19, 'First Reading Date'),
(20, 'Last Reading Date'),
(21, 'Sensor Status'),
(22, 'Sensor Type')
