Insert into dbo.CustomRichTextType (CustomRichTextTypeID, CustomRichTextTypeName, CustomRichTextTypeDisplayName)
values 
(25, 'WellPumpingSummary', 'Well Pumping Summary')

Insert into dbo.CustomRichText(CustomRichTextTypeID, CustomRichTextContent)
values 
(25, '<p>This list displays the total pumped volume (in gallons) by data source type for all wells over the entered date range. Select a new date range and click "Update" to refreh grid data.</p>')