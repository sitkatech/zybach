CREATE TABLE [dbo].[PrismDailyRecord]
(    
    [PrismDailyRecordID]        INT NOT NULL IDENTITY(1, 1),
    [PrismMonthlySyncID]        INT NOT NULL,
    [PrismSyncStatusID]			INT NOT NULL DEFAULT(1),
	[PrismDataTypeID]			INT NOT NULL,
	[BlobResourceID]			INT NULL,

	[Year]						INT NOT NULL,
	[Month]						INT NOT NULL,
	[Day]						INT NOT NULL,
	[ErrorMessage]				VARCHAR(MAX) NULL,

    CONSTRAINT [PK_PrismDailyRecord_PrismDailyRecordID]					PRIMARY KEY CLUSTERED ([PrismDailyRecordID]),

	CONSTRAINT [FK_PrismDailyRecord_PrismMonthlySyncID]					FOREIGN KEY ([PrismMonthlySyncID])	REFERENCES [dbo].[PrismMonthlySync] ([PrismMonthlySyncID]),
	CONSTRAINT [FK_PrismDailyRecord_PrismSyncStatusID]					FOREIGN KEY ([PrismSyncStatusID])	REFERENCES [dbo].[PrismSyncStatus] ([PrismSyncStatusID]),
	CONSTRAINT [FK_PrismDailyRecord_PrismDataTypeID]					FOREIGN KEY ([PrismDataTypeID])		REFERENCES [dbo].[PrismDataType] ([PrismDataTypeID]),
	CONSTRAINT [FK_PrismDailyRecord_BlobResourceID]						FOREIGN KEY ([BlobResourceID])		REFERENCES [dbo].[BlobResource] ([BlobResourceID]),

	CONSTRAINT [AK_PrismDailyRecord_Year_Month_Day_PrismDataTypeID]		UNIQUE ([Year] ASC, [Month] ASC, [Day] ASC, [PrismDataTypeID] ASC)
)
