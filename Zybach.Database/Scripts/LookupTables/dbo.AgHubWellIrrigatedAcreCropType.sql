MERGE INTO dbo.AgHubWellIrrigatedAcreCropType AS Target
USING (VALUES
(1, 'Corn', '#00b600'),
(2, 'Popcorn', '#007b00'),
(3, 'Soybeans', '#003e00'),
(4, 'Sorghum', '#d9ae00'),
(5, 'Dry Edible Beans', '#d57c00'),
(6, 'Alfalfa', '#dade00'),
(7, 'Small Grains', '#d500d9'),
(8, 'Winter Wheat', '##b521b8'),
(9, 'Fallow Fields', '#d9d9d9'),
(10, 'Sunflower', '#d890a2'),
(11, 'Sugar Beets', '#7000cb'),
(12, 'Potatoes', '#780012'),
(13, 'Range, Pasture, Grassland', '#a08c62'),
(14, 'Forage', '#a7956e'),
(15, 'Turf Grass', '#af9e7a')
)
AS Source (AgHubWellIrrigatedAcreCropTypeID, AgHubWellIrrigatedAcreCropTypeName, MapColor)
ON Target.AgHubWellIrrigatedAcreCropTypeID = Source.AgHubWellIrrigatedAcreCropTypeID
WHEN MATCHED THEN
UPDATE SET
	AgHubWellIrrigatedAcreCropTypeName = Source.AgHubWellIrrigatedAcreCropTypeName,
	MapColor = Source.MapColor
WHEN NOT MATCHED BY TARGET THEN
	INSERT (AgHubWellIrrigatedAcreCropTypeID, AgHubWellIrrigatedAcreCropTypeName, MapColor)
	VALUES (AgHubWellIrrigatedAcreCropTypeID, AgHubWellIrrigatedAcreCropTypeName, MapColor)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;