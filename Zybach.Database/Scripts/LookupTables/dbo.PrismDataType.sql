MERGE INTO dbo.PrismDataType AS Target
USING (VALUES
	(1, 'ppt', 'PPT'),
	(2, 'tmin', 'T Min'),
	(3, 'tmax', 'T Max')
)
AS Source (PrismDataTypeID, PrismDataTypeName, PrismDataTypeDisplayName)
ON Target.PrismDataTypeID = Source.PrismDataTypeID
WHEN MATCHED THEN
UPDATE SET
	PrismDataTypeName = Source.PrismDataTypeName,
	PrismDataTypeDisplayName = Source.PrismDataTypeDisplayName
WHEN NOT MATCHED BY TARGET THEN
	INSERT (PrismDataTypeID, PrismDataTypeName, PrismDataTypeDisplayName)
	VALUES (PrismDataTypeID, PrismDataTypeName, PrismDataTypeDisplayName)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;
