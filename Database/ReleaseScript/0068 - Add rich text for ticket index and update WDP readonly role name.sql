Insert into dbo.CustomRichTextType (CustomRichTextTypeID, CustomRichTextTypeName, CustomRichTextTypeDisplayName)
values 
(24, 'SupportTicketIndex', 'Support Ticket Index')

Insert into dbo.CustomRichText(CustomRichTextTypeID, CustomRichTextContent)
values 
(24, 'Welcome to the support ticket index page. Use the filters to quickly find the most pertinent ticket')

--update water data program read-only to be called support only
update dbo.[Role] set RoleName = 'WaterDataProgramSupportOnly', RoleDisplayName = 'Water Data Program Support Only' where RoleID = 5
