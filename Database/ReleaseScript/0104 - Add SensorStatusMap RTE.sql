insert into dbo.CustomRichTextType (CustomRichTextTypeID, CustomRichTextTypeName, CustomRichTextTypeDisplayName)
values (30, 'SensorStatusMap', 'Sensor Status Map')

insert into dbo.CustomRichText (CustomRichTextTypeID, CustomRichTextContent)
values (30, 'The following grid shows all sensors that have not sent a message in over 24 hours, have a most recent voltage reading of less than 2500 millivolts, are Continuity Devices with a status of Always On or Always Off, or have an open support ticket.')