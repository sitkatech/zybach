CREATE TABLE [dbo].[WaterLevelMeasuringEquipment](
	[WaterLevelMeasuringEquipmentID] [int] NOT NULL,
	[WaterLevelMeasuringEquipmentName] [varchar](50) NOT NULL,
	[WaterLevelMeasuringEquipmentDisplayName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_WaterLevelMeasuringEquipment_WaterLevelMeasuringEquipmentID] PRIMARY KEY CLUSTERED 
(
	[WaterLevelMeasuringEquipmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_WaterLevelMeasuringEquipment_WaterLevelMeasuringEquipmentDisplayName] UNIQUE NONCLUSTERED 
(
	[WaterLevelMeasuringEquipmentDisplayName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_WaterLevelMeasuringEquipment_WaterLevelMeasuringEquipmentName] UNIQUE NONCLUSTERED 
(
	[WaterLevelMeasuringEquipmentName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
