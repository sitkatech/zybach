MERGE INTO dbo.AgHubIrrigatedAcreTillageType AS Target
USING (VALUES
(1, 'N Till', 'No Till', '#430154'),
(2, 'M Till', 'Minimum Till', '#453781'),
(3, 'C Till', 'Conventional Till', '#33638d'),
(4, 'S Till', 'Strip Till', '#238a8d'),

(99, 'Other', '#00b6b6'),
(100, 'Not Reported', '#e22e1d')
)
AS Source (AgHubIrrigatedAcreTillageTypeID, AgHubIrrigatedAcreTillageTypeName, AgHubIrrigatedAcreTillageTypeDisplayName, MapColor)
ON Target.AgHubIrrigatedAcreTillageTypeID = Source.AgHubIrrigatedAcreTillageTypeID
WHEN MATCHED THEN
UPDATE SET
	AgHubIrrigatedAcreTillageTypeName = Source.AgHubIrrigatedAcreTillageTypeName,
	AgHubIrrigatedAcreTillageTypeDisplayName = Source.AgHubIrrigatedAcreTillageTypeDisplayName,
	MapColor = Source.MapColor
WHEN NOT MATCHED BY TARGET THEN
	INSERT (AgHubIrrigatedAcreTillageTypeID, AgHubIrrigatedAcreTillageTypeName, AgHubIrrigatedAcreTillageTypeDisplayName, MapColor)
	VALUES (AgHubIrrigatedAcreTillageTypeID, AgHubIrrigatedAcreTillageTypeName, AgHubIrrigatedAcreTillageTypeDisplayName, MapColor)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;