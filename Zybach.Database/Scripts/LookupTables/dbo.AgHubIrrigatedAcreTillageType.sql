MERGE INTO dbo.AgHubIrrigatedAcreTillageType AS Target
USING (VALUES
(1, 'NTill', 'N Till (No Till)', '#430154', 10),
(2, 'MTill', 'M Till (Minimum Till)', '#453781', 20),
(3, 'CTill', 'C Till (Conventional Till)', '#33638d', 30),
(4, 'STill', 'S Till (Strip Till)', '#238a8d', 40),

(99, 'Other', 'Other', '#00b6b6', 999),
(100, 'NotReported', 'Not Reported', '#e22e1d', 1000)
)
AS Source (AgHubIrrigatedAcreTillageTypeID, AgHubIrrigatedAcreTillageTypeName, AgHubIrrigatedAcreTillageTypeDisplayName, MapColor, SortOrder)
ON Target.AgHubIrrigatedAcreTillageTypeID = Source.AgHubIrrigatedAcreTillageTypeID
WHEN MATCHED THEN
UPDATE SET
	AgHubIrrigatedAcreTillageTypeName = Source.AgHubIrrigatedAcreTillageTypeName,
	AgHubIrrigatedAcreTillageTypeDisplayName = Source.AgHubIrrigatedAcreTillageTypeDisplayName,
	MapColor = Source.MapColor,
	SortOrder = Source.SortOrder
WHEN NOT MATCHED BY TARGET THEN
	INSERT (AgHubIrrigatedAcreTillageTypeID, AgHubIrrigatedAcreTillageTypeName, AgHubIrrigatedAcreTillageTypeDisplayName, MapColor, SortOrder)
	VALUES (AgHubIrrigatedAcreTillageTypeID, AgHubIrrigatedAcreTillageTypeName, AgHubIrrigatedAcreTillageTypeDisplayName, MapColor, SortOrder)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;