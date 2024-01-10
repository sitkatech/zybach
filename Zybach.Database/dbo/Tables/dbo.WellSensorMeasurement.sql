CREATE TABLE [dbo].[WellSensorMeasurement](
	[WellSensorMeasurementID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_WellSensorMeasurement_WellSensorMeasurementID] PRIMARY KEY,
	[WellRegistrationID] [varchar](100) NOT NULL,
	[MeasurementTypeID] [int] NOT NULL CONSTRAINT [FK_WellSensorMeasurement_MeasurementType_MeasurementTypeID] FOREIGN KEY REFERENCES [dbo].[MeasurementType] ([MeasurementTypeID]),
	[ReadingYear] [int] NOT NULL,
	[ReadingMonth] [int] NOT NULL,
	[ReadingDay] [int] NOT NULL,
	[SensorName] [varchar](100) NOT NULL,
	[MeasurementValue] [float] NOT NULL,
	[IsAnomalous] [bit] NULL,
	[IsElectricSource] [bit] NULL,
	CONSTRAINT [AK_WellSensorMeasurement_WellRegistrationID_MeasurementTypeID_SensorName_ReadingDate] UNIQUE 
	(
		[WellRegistrationID] ASC,
		[MeasurementTypeID] ASC,
		[SensorName] ASC,
		[ReadingYear] ASC,
		[ReadingMonth] ASC,
		[ReadingDay] ASC
	)
)