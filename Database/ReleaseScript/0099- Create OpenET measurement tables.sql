CREATE TABLE [dbo].[OpenETWaterMeasurement](
	[OpenETWaterMeasurementID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_OpenETWaterMeasurement_OpenETWaterMeasurementID] PRIMARY KEY,
	[WellTPID] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[OpenETDataTypeID] [int] NOT NULL CONSTRAINT [FK_OpenETWaterMeasurement_OpenETDataType_OpenETDataTypeID] FOREIGN KEY REFERENCES [dbo].[OpenETDataType]([OpenETDataTypeID]),
	[ReportedDate] [datetime] NOT NULL,
	[ReportedValueInches] [decimal](20, 4) NOT NULL,
	[ReportedValueAcreFeet] [decimal](20, 4) NOT NULL,
	[IrrigationUnitArea] [decimal](20, 4) NOT NULL
)

GO

CREATE TABLE [dbo].[AgHubIrrigationUnitOpenETDatum](
	[AgHubIrrigationUnitOpenETDatumID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_AgHubIrrigationUnitOpenETDatum_AgHubIrrigationUnitOpenETDatumID] PRIMARY KEY,
	[AgHubIrrigationUnitID] [int] NOT NULL CONSTRAINT [FK_AgHubIrrigationUnitOpenETDatum_AgHubIrrigationUnit_AgHubIrrigationUnitID] FOREIGN KEY REFERENCES [dbo].[AgHubIrrigationUnit]([AgHubIrrigationUnitID]),
	[OpenETDataTypeID] [int] NOT NULL CONSTRAINT [FK_AgHubIrrigationUnitOpenETDatum_OpenETDataType_OpenETDataTypeID] FOREIGN KEY REFERENCES [dbo].[OpenETDataType]([OpenETDataTypeID]),
	[ReportedDate] [datetime] NOT NULL,
	[ReportedValueInches] [decimal](20, 4) NULL,
	[ReportedValueAcreInches] [decimal](20, 4) NULL
)