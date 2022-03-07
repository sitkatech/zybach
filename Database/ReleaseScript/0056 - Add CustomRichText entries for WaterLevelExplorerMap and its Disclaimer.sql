Insert into dbo.CustomRichTextType (CustomRichTextTypeID, CustomRichTextTypeName, CustomRichTextTypeDisplayName)
values 
(20, 'WaterLevelExplorerMap', 'Water Level Explorer Map'),
(21, 'WaterLevelExplorerMapDisclaimer', 'Water Level Explorer Map Disclaimer')

Insert into dbo.CustomRichText(CustomRichTextTypeID, CustomRichTextContent)
values 
(20, 'Welcome to the water level explorer map. Click on a well to view historical water level details and use the date pickers to adjust date ranges.'),
(21, 'Disclaimer: this data is provisional in nature')

