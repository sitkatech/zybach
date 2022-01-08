Insert into dbo.CustomRichTextType (CustomRichTextTypeID, CustomRichTextTypeName, CustomRichTextTypeDisplayName)
values

(15, 'WellRegistrationIDChangeHelpText', 'Well Registration ID Change Help Text')

Insert into dbo.CustomRichText(CustomRichTextTypeID, CustomRichTextContent)
values

(15, 'Well Registration IDs must be unique and fewer than 100 characters. Please add more help text as needed here.')

--ALTER TABLE dbo.Well add constraint AK_Well_WellRegistrationID unique (WellRegistrationID)
