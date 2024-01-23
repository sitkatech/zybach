MERGE INTO dbo.AgHubIrrigatedAcreTillageType AS Target
USING (VALUES
(1, 'CTill', 'C Till (Conventional Till)', '#430154'),
(2, 'MTill', 'M Till (Minimum Till)', '#453781'),
(3, 'NTill', 'N Till (No Till)', '#33638d'),
(4, 'STill', 'S Till (Strip Till)', '#238a8d'),

(99, 'Other', 'Other', '#00b6b6'),
(100, 'NotReported', 'Not Reported', '#e22e1d')
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