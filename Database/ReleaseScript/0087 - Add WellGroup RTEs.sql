Insert into dbo.CustomRichTextType (CustomRichTextTypeID, CustomRichTextTypeName, CustomRichTextTypeDisplayName)
values 
(26, 'WellGroupList', 'Well Group List'),
(27, 'WellGroupEdit', 'Well Group Edit')

Insert into dbo.CustomRichText(CustomRichTextTypeID, CustomRichTextContent)
values 
(26, '<p>A complete list of all user-created well groups and their associated wells.</p>'),
(27, '<p>Here are some instructions for creating or editing a well group using this form...</p>')