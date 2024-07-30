CREATE TABLE [dbo].[PrismMonthlySync]
(    
    [PrismMonthlySyncID]			INT				NOT NULL IDENTITY(1, 1),
    [PrismSyncStatusID]				INT				NOT NULL DEFAULT(1),
    [RunoffCalculationStatusID]		INT				NOT NULL DEFAULT(1),

	[PrismDataTypeID]				INT				NOT NULL,
	[Year]							INT				NOT NULL,
	[Month]							INT				NOT NULL,
	
	[LastSynchronizedDate]			DATETIME		NULL,
	[LastSynchronizedByUserID]		INT				NULL,

	[LastRunoffCalculationDate]		DATETIME		NULL,
	[LastRunoffCalculatedByUserID]	INT				NULL,
	[LastRunoffCalculationError]	VARCHAR(MAX)	NULL,

	[FinalizeDate]					DATETIME		NULL,
	[FinalizeByUserID]				INT				NULL,

    CONSTRAINT [PK_PrismMonthlySync_PrismMonthlySyncID]				PRIMARY KEY CLUSTERED ([PrismMonthlySyncID]),

	CONSTRAINT [FK_PrismMonthlySync_PrismSyncStatusID]				FOREIGN KEY ([PrismSyncStatusID])				REFERENCES [dbo].[PrismSyncStatus] ([PrismSyncStatusID]),
	CONSTRAINT [FK_PrismMonthlySync_RunoffCalculationStatusID]		FOREIGN KEY ([RunoffCalculationStatusID])		REFERENCES [dbo].[RunoffCalculationStatus] ([RunoffCalculationStatusID]),
	CONSTRAINT [FK_PrismMonthlySync_PrismDataTypeID]				FOREIGN KEY ([PrismDataTypeID])					REFERENCES [dbo].[PrismDataType] ([PrismDataTypeID]),
	CONSTRAINT [FK_PrismMonthlySync_LastSynchronizedByUserID]		FOREIGN KEY ([LastSynchronizedByUserID])		REFERENCES [dbo].[User] ([UserID]),
	CONSTRAINT [FK_PrismMonthlySync_LastRunoffCalculatedByUserID]	FOREIGN KEY ([LastRunoffCalculatedByUserID])	REFERENCES [dbo].[User] ([UserID]),
	CONSTRAINT [FK_PrismMonthlySync_FinalizeByUserID]				FOREIGN KEY ([FinalizeByUserID])				REFERENCES [dbo].[User] ([UserID]),

	CONSTRAINT [AK_PrismMonthlySync_Year_Month_PrismDataTypeID]		UNIQUE ([Year] ASC, [Month] ASC, [PrismDataTypeID] ASC)
)
