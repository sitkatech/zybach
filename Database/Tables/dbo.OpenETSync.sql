CREATE TABLE [dbo].[OpenETSync] (
	[OpenETSyncID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_OpenET_OpenETSyncID] PRIMARY KEY,
	[OpenETDataTypeID] [int] NOT NULL CONSTRAINT [FK_OpenETSync_OpenETDataType_OpenETDataTypeID] FOREIGN KEY REFERENCES [dbo].[OpenETDataType]([OpenETDataTypeID]),
	[Year] [int] NOT NULL,
	[Month] [int] NOT NULL,
	[FinalizeDate] [datetime] NULL
	CONSTRAINT [AK_OpenETSync_Year_Month_OpenETDataTypeID] UNIQUE ([Year],[Month],[OpenETDataTypeID])
)