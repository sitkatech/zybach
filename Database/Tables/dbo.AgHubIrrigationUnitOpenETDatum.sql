CREATE TABLE [dbo].[AgHubIrrigationUnitOpenETDatum](
	[AgHubIrrigationUnitOpenETDatumID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_AgHubIrrigationUnitOpenETDatum_AgHubIrrigationUnitOpenETDatumID] PRIMARY KEY,
	[AgHubIrrigationUnitID] [int] NOT NULL CONSTRAINT [FK_AgHubIrrigationUnitOpenETDatum_AgHubIrrigationUnit_AgHubIrrigationUnitID] FOREIGN KEY REFERENCES [dbo].[AgHubIrrigationUnit]([AgHubIrrigationUnitID]),
	[OpenETDataTypeID] [int] NOT NULL CONSTRAINT [FK_AgHubIrrigationUnitOpenETDatum_OpenETDataType_OpenETDataTypeID] FOREIGN KEY REFERENCES [dbo].[OpenETDataType]([OpenETDataTypeID]),
	[ReportedDate] [datetime] NOT NULL,
	[TransactionDate] [datetime] NOT NULL,
	[ReportedValueInches] [decimal](20, 4) NOT NULL,
	[AgHubIrrigationUnitAreaInAcres] [decimal](20, 4) NOT NULL
)