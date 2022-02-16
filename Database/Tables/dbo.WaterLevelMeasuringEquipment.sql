SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WaterLevelMeasuringEquipment](
	[WaterLevelMeasuringEquipmentID] [int] IDENTITY(1,1) NOT NULL,
	[WaterLevelMeasuringEquipmentName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[WaterLevelMeasuringEquipmentDisplayName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
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
