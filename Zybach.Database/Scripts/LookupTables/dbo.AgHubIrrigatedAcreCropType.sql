MERGE INTO dbo.AgHubIrrigatedAcreCropType AS Target
USING (VALUES
(1, 'Corn', '#00b600', 10),
(2, 'Popcorn', '#007b00', 20),
(3, 'Soybeans', '#003e00', 30),
(4, 'Sorghum', '#d9ae00', 40),
(5, 'Dry Edible Beans', '#d57c00', 50),
(6, 'Alfalfa', '#dade00', 60),
(7, 'Small Grains', '#d500d9', 70),
(8, 'Winter Wheat', '#b521b8', 80),
(9, 'Fallow Fields', '#d9d9d9', 90),
(10, 'Sunflower', '#d890a2', 100),
(11, 'Sugar Beets', '#7000cb', 110),
(12, 'Potatoes', '#780012', 120),
(13, 'Range, Pasture, Grassland', '#a08c62', 130),
(14, 'Forage', '#7c6c4b', 140),
(15, 'Turf Grass', '#574c35', 150),
(16, 'Pasture', '#a08c62', 160),

(99, 'Other', '#00b6b6', 999),
(100, 'Not Reported', '#e22e1d', 1000)
)
AS Source (AgHubIrrigatedAcreCropTypeID, AgHubIrrigatedAcreCropTypeName, MapColor, SortOrder)
ON Target.AgHubIrrigatedAcreCropTypeID = Source.AgHubIrrigatedAcreCropTypeID
WHEN MATCHED THEN
UPDATE SET
	AgHubIrrigatedAcreCropTypeName = Source.AgHubIrrigatedAcreCropTypeName,
	MapColor = Source.MapColor,
	SortOrder = Source.SortOrder
WHEN NOT MATCHED BY TARGET THEN
	INSERT (AgHubIrrigatedAcreCropTypeID, AgHubIrrigatedAcreCropTypeName, MapColor, SortOrder)
	VALUES (AgHubIrrigatedAcreCropTypeID, AgHubIrrigatedAcreCropTypeName, MapColor, SortOrder)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;