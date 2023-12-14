CREATE TABLE [dbo].[SensorAnomaly](
	[SensorAnomalyID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_SensorAnomaly_SensorAnomalyID] PRIMARY KEY,
	[SensorID] [int] NOT NULL CONSTRAINT [FK_SensorAnomaly_Sensor_SensorID] FOREIGN KEY REFERENCES [dbo].[Sensor] ([SensorID]),
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[Notes] [varchar](500) NULL
)