CREATE TABLE [dbo].[WellWaterQualityInspectionType](
	[WellWaterQualityInspectionTypeID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_WellWaterQualityInspectionType_WellWaterQualityInspectionTypeID] PRIMARY KEY,
	[WellID] [int] NOT NULL CONSTRAINT [FK_WellWaterQualityInspectionType_Well_WellID] FOREIGN KEY REFERENCES [dbo].[Well] ([WellID]),
	[WaterQualityInspectionTypeID] [int] NOT NULL CONSTRAINT [FK_WellWaterQualityInspectionType_WaterQualityInspectionType_WaterQualityInspectionTypeID] FOREIGN KEY REFERENCES [dbo].[WaterQualityInspectionType] ([WaterQualityInspectionTypeID]),
)