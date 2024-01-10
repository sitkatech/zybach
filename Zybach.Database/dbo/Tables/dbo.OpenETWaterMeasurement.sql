CREATE TABLE [dbo].[OpenETWaterMeasurement](
	[OpenETWaterMeasurementID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_OpenETWaterMeasurement_OpenETWaterMeasurementID] PRIMARY KEY,
	[WellTPID] [varchar](100) NOT NULL,
	[OpenETDataTypeID] [int] NOT NULL CONSTRAINT [FK_OpenETWaterMeasurement_OpenETDataType_OpenETDataTypeID] FOREIGN KEY REFERENCES [dbo].[OpenETDataType] ([OpenETDataTypeID]),
	[ReportedDate] [datetime] NOT NULL,
	[TransactionDate] [datetime] NOT NULL,
	[ReportedValueInches] [decimal](20, 4) NOT NULL,
	[ReportedValueAcreFeet] [decimal](20, 4) NOT NULL,
	[IrrigationUnitArea] [decimal](20, 4) NOT NULL
)
