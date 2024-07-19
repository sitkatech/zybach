CREATE TABLE [dbo].[PrismSyncStatus]
(
	[PrismSyncStatusID]				INT NOT NULL,
	[PrismSyncStatusName]			VARCHAR(100) NOT NULL,
	[PrismSyncStatusDisplayName]	VARCHAR(100) NOT NULL,
	
	CONSTRAINT [PK_PrismSyncStatus_PrismyncStatusID] PRIMARY KEY CLUSTERED ([PrismSyncStatusID] ASC)
)
