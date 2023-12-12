CREATE TABLE [dbo].[OpenETWaterMeasurement](
	[OpenETWaterMeasurementID] [int] IDENTITY(1,1) NOT NULL,
	[WellTPID] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[OpenETDataTypeID] [int] NOT NULL,
	[ReportedDate] [datetime] NOT NULL,
	[TransactionDate] [datetime] NOT NULL,
	[ReportedValueInches] [decimal](20, 4) NOT NULL,
	[ReportedValueAcreFeet] [decimal](20, 4) NOT NULL,
	[IrrigationUnitArea] [decimal](20, 4) NOT NULL,
 CONSTRAINT [PK_OpenETWaterMeasurement_OpenETWaterMeasurementID] PRIMARY KEY CLUSTERED 
(
	[OpenETWaterMeasurementID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[OpenETWaterMeasurement]  WITH CHECK ADD  CONSTRAINT [FK_OpenETWaterMeasurement_OpenETDataType_OpenETDataTypeID] FOREIGN KEY([OpenETDataTypeID])
REFERENCES [dbo].[OpenETDataType] ([OpenETDataTypeID])
GO
ALTER TABLE [dbo].[OpenETWaterMeasurement] CHECK CONSTRAINT [FK_OpenETWaterMeasurement_OpenETDataType_OpenETDataTypeID]