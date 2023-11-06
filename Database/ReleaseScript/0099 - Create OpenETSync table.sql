CREATE TABLE [dbo].[OpenETSync] (
	[OpenETSyncID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_OpenET_OpenETSyncID] PRIMARY KEY,
	[OpenETDataTypeID] [int] NOT NULL CONSTRAINT [FK_OpenETSync_OpenETDataType_OpenETDataTypeID] FOREIGN KEY REFERENCES [dbo].[OpenETDataType]([OpenETDataTypeID]),
	[Year] [int] NOT NULL,
	[Month] [int] NOT NULL,
	[FinalizeDate] [datetime] NULL
	CONSTRAINT [AK_OpenETSync_Year_Month_OpenETDataTypeID] UNIQUE ([Year],[Month],[OpenETDataTypeID])
)

GO

alter table dbo.OpenETSyncHistory 
	drop constraint [FK_OpenETSyncHistory_WaterYearMonth_WaterYearMonthID]

alter table dbo.OpenETSyncHistory 
	drop column [WaterYearMonthID]

alter table dbo.OpenETSyncHistory
	add [OpenETSyncID] [int] NULL CONSTRAINT [FK_OpenETSyncHistory_OpenETSync_OpenETSyncID] FOREIGN KEY REFERENCES [dbo].[OpenETSync]([OpenETSyncID])

GO 

drop table dbo.WaterYearMonth

