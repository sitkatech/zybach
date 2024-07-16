CREATE TABLE [dbo].[PrismMonthlySync]
(    
    [PrismMonthlySyncID]        INT NOT NULL IDENTITY(1, 1),
    [PrismSyncStatusID]			INT NOT NULL DEFAULT(1),
	[PrismDataTypeID]			INT NOT NULL,
	[Year]						INT NOT NULL,
	[Month]						INT NOT NULL,
	[FinalizeDate]				DATETIME NULL,

    CONSTRAINT [PK_PrismMonthlySync_PrismSyncID]				PRIMARY KEY CLUSTERED ([PrismMonthlySyncID]),
	CONSTRAINT [AK_PrismMonthlySync_Year_Month_PrismDataTypeID]	UNIQUE ([Year] ASC, [Month] ASC, [PrismDataTypeID] ASC)
)
