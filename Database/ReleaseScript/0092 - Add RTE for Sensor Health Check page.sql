insert into dbo.CustomRichTextType (CustomRichTextTypeID, CustomRichTextTypeName, CustomRichTextTypeDisplayName)
values (29, 'SensorHealthCheck', 'Sensor Health Check')

insert into dbo.CustomRichText (CustomRichTextTypeID, CustomRichTextContent)
values (29, 'Enter a device serial number to see the most recent message received from that device. Results will automatically refresh every 30 seconds to check for new messages.')