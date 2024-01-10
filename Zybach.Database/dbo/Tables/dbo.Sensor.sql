CREATE TABLE [dbo].[Sensor](
	[SensorID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_Sensor_SensorID] PRIMARY KEY,
	[SensorName] [varchar](100) NOT NULL CONSTRAINT [AK_Sensor_SensorName] UNIQUE,
	[SensorTypeID] [int] NOT NULL CONSTRAINT [FK_Sensor_SensorType_SensorTypeID] FOREIGN KEY REFERENCES [dbo].[SensorType] ([SensorTypeID]),
	[WellID] [int] NULL CONSTRAINT [FK_Sensor_Well_WellID] FOREIGN KEY REFERENCES [dbo].[Well] ([WellID]),
	[InGeoOptix] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[LastUpdateDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[RetirementDate] [datetime] NULL,
	[ContinuityMeterStatusID] [int] NULL CONSTRAINT [FK_Sensor_ContinuityMeterStatus_ContinuityMeterStatusID] FOREIGN KEY REFERENCES [dbo].[ContinuityMeterStatus] ([ContinuityMeterStatusID]),
	[ContinuityMeterStatusLastUpdated] [datetime] NULL,
	[SnoozeStartDate] [datetime] NULL
)