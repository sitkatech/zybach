CREATE TABLE [dbo].[SensorAnomaly](
	[SensorAnomalyID] [int] IDENTITY(1,1) NOT NULL,
	[SensorID] [int] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[Notes] [varchar](500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_SensorAnomaly_SensorAnomalyID] PRIMARY KEY CLUSTERED 
(
	[SensorAnomalyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[SensorAnomaly]  WITH CHECK ADD  CONSTRAINT [FK_SensorAnomaly_Sensor_SensorID] FOREIGN KEY([SensorID])
REFERENCES [dbo].[Sensor] ([SensorID])
GO
ALTER TABLE [dbo].[SensorAnomaly] CHECK CONSTRAINT [FK_SensorAnomaly_Sensor_SensorID]