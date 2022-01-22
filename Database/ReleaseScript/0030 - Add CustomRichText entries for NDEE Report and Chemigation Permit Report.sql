Insert into dbo.CustomRichTextType (CustomRichTextTypeID, CustomRichTextTypeName, CustomRichTextTypeDisplayName)
values

(10, 'NDEEChemicalsReport', 'NDEE Chemicals Report'),
(11, 'ChemigationPermitReport', 'Chemigation Permit Report')

Insert into dbo.CustomRichText(CustomRichTextTypeID, CustomRichTextContent)
values

(10, 'Default content for: NDEE Chemicals Report'),
(11, 'Default content for: Chemigation Permit Report')