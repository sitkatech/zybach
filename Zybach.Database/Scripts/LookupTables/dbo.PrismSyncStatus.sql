MERGE INTO dbo.PrismSyncStatus AS Target
USING (VALUES
	(1, 'SynchronizationNotStarted', 'Synchronization Not Started'),
	(2, 'InProgress', 'In Progress'),
	(3, 'Succeeded', 'Succeeded'),
	(4, 'Failed', 'Failed')
)
AS Source (PrismSyncStatusID, PrismSyncStatusName, PrismSyncStatusDisplayName)
ON Target.PrismSyncStatusID = Source.PrismSyncStatusID
WHEN MATCHED THEN
UPDATE SET
	PrismSyncStatusName = Source.PrismSyncStatusName,
	PrismSyncStatusDisplayName = Source.PrismSyncStatusDisplayName
WHEN NOT MATCHED BY TARGET THEN
	INSERT (PrismSyncStatusID, PrismSyncStatusName, PrismSyncStatusDisplayName)
	VALUES (PrismSyncStatusID, PrismSyncStatusName, PrismSyncStatusDisplayName)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;
