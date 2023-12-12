MERGE INTO dbo.WellUse AS Target
USING (VALUES
(1, 'Irrigation', 'Irrigation'),
(2, 'PublicSupply', 'Public Supply'),
(3, 'Domestic', 'Domestic'),
(4, 'Monitoring', 'Monitoring')
)
AS Source (WellUseID, WellUseName, WellUseDisplayName)
ON Target.WellUseID = Source.WellUseID
WHEN MATCHED THEN
UPDATE SET
	WellUseName = Source.WellUseName,
	WellUseDisplayName = Source.WellUseDisplayName
WHEN NOT MATCHED BY TARGET THEN
	INSERT (WellUseID, WellUseName, WellUseDisplayName)
	VALUES (WellUseID, WellUseName, WellUseDisplayName)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;
