insert into FieldDefinitionType (FieldDefinitionTypeID, FieldDefinitionTypeName, FieldDefinitionTypeDisplayName)
values 
(2, 'HasWaterLevelInspections', 'Has Water Level Inspections?'),
(3, 'HasWaterQualityInspections', 'Has Water Quality Inspections?'),
(4, 'LatestWaterLevelInspectionDate', 'Latest Water Level Inspection Date'),
(5, 'LatestWaterQualityInspectionDate', 'Latest Water Quality Inspection Date')


insert into FieldDefinition (FieldDefinitionTypeID, FieldDefinitionValue)
values 
(2, 'If the well has any Seasonal Water Level Inspection records in the database this column will display Yes'),
(3, 'If the well has any Water Quality Inspection records in the database this column will display Yes'),
(4, 'The date of the most recent Seasonal Water Level Inspection for this well.'),
(5, 'The date of the most recent Water Quality Inspection for this well.')
