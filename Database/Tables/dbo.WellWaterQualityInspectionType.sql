SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WellWaterQualityInspectionType](
	[WellWaterQualityInspectionTypeID] [int] IDENTITY(1,1) NOT NULL,
	[WellID] [int] NOT NULL,
	[WaterQualityInspectionTypeID] [int] NOT NULL,
 CONSTRAINT [PK_WellWaterQualityInspectionType_WellWaterQualityInspectionTypeID] PRIMARY KEY CLUSTERED 
(
	[WellWaterQualityInspectionTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[WellWaterQualityInspectionType]  WITH CHECK ADD  CONSTRAINT [FK_WellWaterQualityInspectionType_WaterQualityInspectionType_WaterQualityInspectionTypeID] FOREIGN KEY([WaterQualityInspectionTypeID])
REFERENCES [dbo].[WaterQualityInspectionType] ([WaterQualityInspectionTypeID])
GO
ALTER TABLE [dbo].[WellWaterQualityInspectionType] CHECK CONSTRAINT [FK_WellWaterQualityInspectionType_WaterQualityInspectionType_WaterQualityInspectionTypeID]
GO
ALTER TABLE [dbo].[WellWaterQualityInspectionType]  WITH CHECK ADD  CONSTRAINT [FK_WellWaterQualityInspectionType_Well_WellID] FOREIGN KEY([WellID])
REFERENCES [dbo].[Well] ([WellID])
GO
ALTER TABLE [dbo].[WellWaterQualityInspectionType] CHECK CONSTRAINT [FK_WellWaterQualityInspectionType_Well_WellID]