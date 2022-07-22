insert into dbo.FieldDefinitionType (FieldDefinitionTypeID, FieldDefinitionTypeName, FieldDefinitionTypeDisplayName)
values (26, 'ContinuityMeterStatus', 'Continuity Meter Always On/Off')

go 

insert into dbo.FieldDefinition (FieldDefinitionTypeID, FieldDefinitionValue)
values (26, '<p>Continuity Meter is Always On if all messages received for device in the last 7 days are "On" messages.</p><p>Continuity Meter is Always Off if all messages recieved for the device in the last 14 days are "Off" messages (only applicable June through September).</p>')