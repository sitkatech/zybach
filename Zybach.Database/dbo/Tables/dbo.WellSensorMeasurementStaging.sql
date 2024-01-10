CREATE TABLE [dbo].[WellSensorMeasurementStaging](
	[WellSensorMeasurementStagingID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_WellSensorMeasurementStaging_WellSensorMeasurementStagingID] PRIMARY KEY,
	[WellRegistrationID] [varchar](100) NOT NULL,
	[MeasurementTypeID] [int] NOT NULL CONSTRAINT [FK_WellSensorMeasurementStaging_MeasurementType_MeasurementTypeID] FOREIGN KEY REFERENCES [dbo].[MeasurementType] ([MeasurementTypeID]),
	[ReadingYear] [int] NOT NULL,
	[ReadingMonth] [int] NOT NULL,
	[ReadingDay] [int] NOT NULL,
	[SensorName] [varchar](100) NOT NULL,
	[MeasurementValue] [float] NOT NULL,
	[IsElectricSource] [bit] NULL
)