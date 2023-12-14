CREATE TABLE [dbo].[GeoOptixSensorStaging](
	[GeoOptixSensorStagingID] [int] IDENTITY(1,1) NOT NULL,
	[WellRegistrationID] [varchar](100) NOT NULL,
	[SensorName] [varchar](100) NOT NULL,
	[SensorType] [varchar](100) NOT NULL,
 CONSTRAINT [PK_GeoOptixSensorStaging_GeoOptixSensorStagingID] PRIMARY KEY CLUSTERED 
(
	[GeoOptixSensorStagingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
