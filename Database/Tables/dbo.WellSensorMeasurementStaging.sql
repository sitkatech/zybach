SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WellSensorMeasurementStaging](
	[WellSensorMeasurementStagingID] [int] IDENTITY(1,1) NOT NULL,
	[WellRegistrationID] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[MeasurementTypeID] [int] NOT NULL,
	[ReadingYear] [int] NOT NULL,
	[ReadingMonth] [int] NOT NULL,
	[ReadingDay] [int] NOT NULL,
	[SensorName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[MeasurementValue] [float] NOT NULL,
	[IsElectricSource] [bit] NULL,
 CONSTRAINT [PK_WellSensorMeasurementStaging_WellSensorMeasurementStagingID] PRIMARY KEY CLUSTERED 
(
	[WellSensorMeasurementStagingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[WellSensorMeasurementStaging]  WITH CHECK ADD  CONSTRAINT [FK_WellSensorMeasurementStaging_MeasurementType_MeasurementTypeID] FOREIGN KEY([MeasurementTypeID])
REFERENCES [dbo].[MeasurementType] ([MeasurementTypeID])
GO
ALTER TABLE [dbo].[WellSensorMeasurementStaging] CHECK CONSTRAINT [FK_WellSensorMeasurementStaging_MeasurementType_MeasurementTypeID]