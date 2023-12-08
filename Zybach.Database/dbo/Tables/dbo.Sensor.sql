SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sensor](
	[SensorID] [int] IDENTITY(1,1) NOT NULL,
	[SensorName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[SensorTypeID] [int] NOT NULL,
	[WellID] [int] NULL,
	[InGeoOptix] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[LastUpdateDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[RetirementDate] [datetime] NULL,
	[ContinuityMeterStatusID] [int] NULL,
	[ContinuityMeterStatusLastUpdated] [datetime] NULL,
	[SnoozeStartDate] [datetime] NULL,
 CONSTRAINT [PK_Sensor_SensorID] PRIMARY KEY CLUSTERED 
(
	[SensorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_Sensor_SensorName] UNIQUE NONCLUSTERED 
(
	[SensorName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Sensor]  WITH CHECK ADD  CONSTRAINT [FK_Sensor_ContinuityMeterStatus_ContinuityMeterStatusID] FOREIGN KEY([ContinuityMeterStatusID])
REFERENCES [dbo].[ContinuityMeterStatus] ([ContinuityMeterStatusID])
GO
ALTER TABLE [dbo].[Sensor] CHECK CONSTRAINT [FK_Sensor_ContinuityMeterStatus_ContinuityMeterStatusID]
GO
ALTER TABLE [dbo].[Sensor]  WITH CHECK ADD  CONSTRAINT [FK_Sensor_SensorType_SensorTypeID] FOREIGN KEY([SensorTypeID])
REFERENCES [dbo].[SensorType] ([SensorTypeID])
GO
ALTER TABLE [dbo].[Sensor] CHECK CONSTRAINT [FK_Sensor_SensorType_SensorTypeID]
GO
ALTER TABLE [dbo].[Sensor]  WITH CHECK ADD  CONSTRAINT [FK_Sensor_Well_WellID] FOREIGN KEY([WellID])
REFERENCES [dbo].[Well] ([WellID])
GO
ALTER TABLE [dbo].[Sensor] CHECK CONSTRAINT [FK_Sensor_Well_WellID]