CREATE TABLE [dbo].[WaterLevelInspection](
	[WaterLevelInspectionID] [int] IDENTITY(1,1) NOT NULL,
	[WellID] [int] NOT NULL,
	[InspectionDate] [datetime] NOT NULL,
	[InspectorUserID] [int] NOT NULL,
	[WaterLevelInspectionStatus] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MeasuringEquipment] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Crop] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HasOil] [bit] NOT NULL,
	[HasBrokenTape] [bit] NOT NULL,
	[Accuracy] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Method] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Party] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SourceAgency] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SourceCode] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TimeDatumCode] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TimeDatumReliability] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[LevelTypeCode] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AgencyCode] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Access] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Hold] [decimal](12, 4) NULL,
	[Cut] [decimal](12, 4) NULL,
	[MP] [decimal](12, 4) NULL,
	[Measurement] [decimal](12, 4) NULL,
	[IsPrimary] [bit] NULL,
	[WaterLevel] [decimal](12, 2) NULL,
	[CropTypeID] [int] NULL,
	[InspectionNotes] [varchar](500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[InspectionNickname] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_WaterLevelInspection_WaterLevelInspectionID] PRIMARY KEY CLUSTERED 
(
	[WaterLevelInspectionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[WaterLevelInspection]  WITH CHECK ADD  CONSTRAINT [FK_WaterLevelInspection_CropType_CropTypeID] FOREIGN KEY([CropTypeID])
REFERENCES [dbo].[CropType] ([CropTypeID])
GO
ALTER TABLE [dbo].[WaterLevelInspection] CHECK CONSTRAINT [FK_WaterLevelInspection_CropType_CropTypeID]
GO
ALTER TABLE [dbo].[WaterLevelInspection]  WITH CHECK ADD  CONSTRAINT [FK_WaterLevelInspection_User_InspectorUserID_UserID] FOREIGN KEY([InspectorUserID])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[WaterLevelInspection] CHECK CONSTRAINT [FK_WaterLevelInspection_User_InspectorUserID_UserID]
GO
ALTER TABLE [dbo].[WaterLevelInspection]  WITH CHECK ADD  CONSTRAINT [FK_WaterLevelInspection_Well_WellID] FOREIGN KEY([WellID])
REFERENCES [dbo].[Well] ([WellID])
GO
ALTER TABLE [dbo].[WaterLevelInspection] CHECK CONSTRAINT [FK_WaterLevelInspection_Well_WellID]