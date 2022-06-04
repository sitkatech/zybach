MERGE INTO dbo.County AS Target
USING (VALUES
(1, 'Arthur', 'Arthur'),
(2, 'Keith', 'Keith'),
(3, 'Lincoln', 'Lincoln'),
(4, 'McPherson', 'McPherson')
)
AS Source (CountyID, CountyName, CountyDisplayName)
ON Target.CountyID = Source.CountyID
WHEN MATCHED THEN
UPDATE SET
	CountyName = Source.CountyName,
	CountyDisplayName = Source.CountyDisplayName
WHEN NOT MATCHED BY TARGET THEN
	INSERT (CountyID, CountyName, CountyDisplayName)
	VALUES (CountyID, CountyName, CountyDisplayName)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;
