CREATE TABLE [dbo].[WaterQualityInspection](
	[WaterQualityInspectionID] [int] IDENTITY(1,1) NOT NULL,
	[WellID] [int] NOT NULL,
	[WaterQualityInspectionTypeID] [int] NOT NULL,
	[InspectionDate] [datetime] NOT NULL,
	[InspectorUserID] [int] NOT NULL,
	[Temperature] [decimal](12, 4) NULL,
	[PH] [decimal](12, 4) NULL,
	[Conductivity] [decimal](12, 4) NULL,
	[FieldAlkilinity] [decimal](12, 4) NULL,
	[FieldNitrates] [decimal](12, 4) NULL,
	[LabNitrates] [decimal](12, 4) NULL,
	[Salinity] [decimal](12, 4) NULL,
	[MV] [decimal](12, 4) NULL,
	[Sodium] [decimal](12, 4) NULL,
	[Calcium] [decimal](12, 4) NULL,
	[Magnesium] [decimal](12, 4) NULL,
	[Potassium] [decimal](12, 4) NULL,
	[HydrogenCarbonate] [decimal](12, 4) NULL,
	[CalciumCarbonate] [decimal](12, 4) NULL,
	[Sulfate] [decimal](12, 4) NULL,
	[Chloride] [decimal](12, 4) NULL,
	[SiliconDioxide] [decimal](12, 4) NULL,
	[CropTypeID] [int] NULL,
	[PreWaterLevel] [decimal](12, 4) NULL,
	[PostWaterLevel] [decimal](12, 4) NULL,
	[InspectionNotes] [varchar](500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[InspectionNickname] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_WaterQualityInspection_WaterQualityInspectionID] PRIMARY KEY CLUSTERED 
(
	[WaterQualityInspectionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[WaterQualityInspection]  WITH CHECK ADD  CONSTRAINT [FK_WaterQualityInspection_CropType_CropTypeID] FOREIGN KEY([CropTypeID])
REFERENCES [dbo].[CropType] ([CropTypeID])
GO
ALTER TABLE [dbo].[WaterQualityInspection] CHECK CONSTRAINT [FK_WaterQualityInspection_CropType_CropTypeID]
GO
ALTER TABLE [dbo].[WaterQualityInspection]  WITH CHECK ADD  CONSTRAINT [FK_WaterQualityInspection_User_InspectorUserID_UserID] FOREIGN KEY([InspectorUserID])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[WaterQualityInspection] CHECK CONSTRAINT [FK_WaterQualityInspection_User_InspectorUserID_UserID]
GO
ALTER TABLE [dbo].[WaterQualityInspection]  WITH CHECK ADD  CONSTRAINT [FK_WaterQualityInspection_WaterQualityInspectionType_WaterQualityInspectionTypeID] FOREIGN KEY([WaterQualityInspectionTypeID])
REFERENCES [dbo].[WaterQualityInspectionType] ([WaterQualityInspectionTypeID])
GO
ALTER TABLE [dbo].[WaterQualityInspection] CHECK CONSTRAINT [FK_WaterQualityInspection_WaterQualityInspectionType_WaterQualityInspectionTypeID]
GO
ALTER TABLE [dbo].[WaterQualityInspection]  WITH CHECK ADD  CONSTRAINT [FK_WaterQualityInspection_Well_WellID] FOREIGN KEY([WellID])
REFERENCES [dbo].[Well] ([WellID])
GO
ALTER TABLE [dbo].[WaterQualityInspection] CHECK CONSTRAINT [FK_WaterQualityInspection_Well_WellID]