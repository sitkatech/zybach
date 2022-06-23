insert into dbo.FieldDefinitionType (FieldDefinitionTypeID, FieldDefinitionTypeName, FieldDefinitionTypeDisplayName)
values
(23, 'IrrigationUnitAcres', 'Irrigation Unit Area (ac)')

insert into dbo.FieldDefinition (FieldDefinitionTypeID, FieldDefinitionValue)
values
(23, 'Irrigation Unit Area: Acres currently in this irrigation unit, should be identical to the most recent acreage displayed for any associated well.')