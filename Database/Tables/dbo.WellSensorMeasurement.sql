SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WellSensorMeasurement](
	[WellSensorMeasurementID] [int] IDENTITY(1,1) NOT NULL,
	[WellRegistrationID] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[MeasurementTypeID] [int] NOT NULL,
	[ReadingYear] [int] NOT NULL,
	[ReadingMonth] [int] NOT NULL,
	[ReadingDay] [int] NOT NULL,
	[SensorName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MeasurementValue] [float] NOT NULL,
 CONSTRAINT [PK_WellSensorMeasurement_WellSensorMeasurementID] PRIMARY KEY CLUSTERED 
(
	[WellSensorMeasurementID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_WellSensorMeasurement_WellRegistrationID_MeasurementTypeID_SensorName_ReadingDate] UNIQUE NONCLUSTERED 
(
	[WellRegistrationID] ASC,
	[MeasurementTypeID] ASC,
	[SensorName] ASC,
	[ReadingYear] ASC,
	[ReadingMonth] ASC,
	[ReadingDay] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[WellSensorMeasurement]  WITH CHECK ADD  CONSTRAINT [FK_WellSensorMeasurement_MeasurementType_MeasurementTypeID] FOREIGN KEY([MeasurementTypeID])
REFERENCES [dbo].[MeasurementType] ([MeasurementTypeID])
GO
ALTER TABLE [dbo].[WellSensorMeasurement] CHECK CONSTRAINT [FK_WellSensorMeasurement_MeasurementType_MeasurementTypeID]