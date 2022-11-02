insert into dbo.FieldDefinitionType (FieldDefinitionTypeID, FieldDefinitionTypeName, FieldDefinitionTypeDisplayName)
values (27, 'ActiveSupportTicket', 'Active Support Ticket')

go 

insert into dbo.FieldDefinition (FieldDefinitionTypeID, FieldDefinitionValue)
values (27, 'The most recently updated active support ticket associated with this sensor.')