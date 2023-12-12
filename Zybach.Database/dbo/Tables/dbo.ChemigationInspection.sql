CREATE TABLE [dbo].[ChemigationInspection](
	[ChemigationInspectionID] [int] IDENTITY(1,1) NOT NULL,
	[ChemigationPermitAnnualRecordID] [int] NOT NULL,
	[ChemigationInspectionStatusID] [int] NOT NULL,
	[ChemigationInspectionTypeID] [int] NULL,
	[InspectionDate] [datetime] NULL,
	[InspectorUserID] [int] NULL,
	[ChemigationMainlineCheckValveID] [int] NULL,
	[HasVacuumReliefValve] [bit] NULL,
	[HasInspectionPort] [bit] NULL,
	[ChemigationLowPressureValveID] [int] NULL,
	[ChemigationInjectionValveID] [int] NULL,
	[ChemigationInterlockTypeID] [int] NULL,
	[TillageID] [int] NULL,
	[CropTypeID] [int] NULL,
	[InspectionNotes] [varchar](500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ChemigationInspectionFailureReasonID] [int] NULL,
 CONSTRAINT [PK_ChemigationInspection_ChemigationInspectionID] PRIMARY KEY CLUSTERED 
(
	[ChemigationInspectionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[ChemigationInspection]  WITH CHECK ADD  CONSTRAINT [FK_ChemigationInspection_ChemigationInjectionValve_ChemigationInjectionValveID] FOREIGN KEY([ChemigationInjectionValveID])
REFERENCES [dbo].[ChemigationInjectionValve] ([ChemigationInjectionValveID])
GO
ALTER TABLE [dbo].[ChemigationInspection] CHECK CONSTRAINT [FK_ChemigationInspection_ChemigationInjectionValve_ChemigationInjectionValveID]
GO
ALTER TABLE [dbo].[ChemigationInspection]  WITH CHECK ADD  CONSTRAINT [FK_ChemigationInspection_ChemigationInspectionFailureReason_ChemigationInspectionFailureReasonID] FOREIGN KEY([ChemigationInspectionFailureReasonID])
REFERENCES [dbo].[ChemigationInspectionFailureReason] ([ChemigationInspectionFailureReasonID])
GO
ALTER TABLE [dbo].[ChemigationInspection] CHECK CONSTRAINT [FK_ChemigationInspection_ChemigationInspectionFailureReason_ChemigationInspectionFailureReasonID]
GO
ALTER TABLE [dbo].[ChemigationInspection]  WITH CHECK ADD  CONSTRAINT [FK_ChemigationInspection_ChemigationInspectionStatus_ChemigationInspectionStatusID] FOREIGN KEY([ChemigationInspectionStatusID])
REFERENCES [dbo].[ChemigationInspectionStatus] ([ChemigationInspectionStatusID])
GO
ALTER TABLE [dbo].[ChemigationInspection] CHECK CONSTRAINT [FK_ChemigationInspection_ChemigationInspectionStatus_ChemigationInspectionStatusID]
GO
ALTER TABLE [dbo].[ChemigationInspection]  WITH CHECK ADD  CONSTRAINT [FK_ChemigationInspection_ChemigationInspectionType_ChemigationInspectionTypeID] FOREIGN KEY([ChemigationInspectionTypeID])
REFERENCES [dbo].[ChemigationInspectionType] ([ChemigationInspectionTypeID])
GO
ALTER TABLE [dbo].[ChemigationInspection] CHECK CONSTRAINT [FK_ChemigationInspection_ChemigationInspectionType_ChemigationInspectionTypeID]
GO
ALTER TABLE [dbo].[ChemigationInspection]  WITH CHECK ADD  CONSTRAINT [FK_ChemigationInspection_ChemigationInterlockType_ChemigationInterlockTypeID] FOREIGN KEY([ChemigationInterlockTypeID])
REFERENCES [dbo].[ChemigationInterlockType] ([ChemigationInterlockTypeID])
GO
ALTER TABLE [dbo].[ChemigationInspection] CHECK CONSTRAINT [FK_ChemigationInspection_ChemigationInterlockType_ChemigationInterlockTypeID]
GO
ALTER TABLE [dbo].[ChemigationInspection]  WITH CHECK ADD  CONSTRAINT [FK_ChemigationInspection_ChemigationLowPressureValve_ChemigationLowPressureValveID] FOREIGN KEY([ChemigationLowPressureValveID])
REFERENCES [dbo].[ChemigationLowPressureValve] ([ChemigationLowPressureValveID])
GO
ALTER TABLE [dbo].[ChemigationInspection] CHECK CONSTRAINT [FK_ChemigationInspection_ChemigationLowPressureValve_ChemigationLowPressureValveID]
GO
ALTER TABLE [dbo].[ChemigationInspection]  WITH CHECK ADD  CONSTRAINT [FK_ChemigationInspection_ChemigationMainlineCheckValve_ChemigationMainlineCheckValveID] FOREIGN KEY([ChemigationMainlineCheckValveID])
REFERENCES [dbo].[ChemigationMainlineCheckValve] ([ChemigationMainlineCheckValveID])
GO
ALTER TABLE [dbo].[ChemigationInspection] CHECK CONSTRAINT [FK_ChemigationInspection_ChemigationMainlineCheckValve_ChemigationMainlineCheckValveID]
GO
ALTER TABLE [dbo].[ChemigationInspection]  WITH CHECK ADD  CONSTRAINT [FK_ChemigationInspection_ChemigationPermitAnnualRecord_ChemigationPermitAnnualRecordID] FOREIGN KEY([ChemigationPermitAnnualRecordID])
REFERENCES [dbo].[ChemigationPermitAnnualRecord] ([ChemigationPermitAnnualRecordID])
GO
ALTER TABLE [dbo].[ChemigationInspection] CHECK CONSTRAINT [FK_ChemigationInspection_ChemigationPermitAnnualRecord_ChemigationPermitAnnualRecordID]
GO
ALTER TABLE [dbo].[ChemigationInspection]  WITH CHECK ADD  CONSTRAINT [FK_ChemigationInspection_CropType_CropTypeID] FOREIGN KEY([CropTypeID])
REFERENCES [dbo].[CropType] ([CropTypeID])
GO
ALTER TABLE [dbo].[ChemigationInspection] CHECK CONSTRAINT [FK_ChemigationInspection_CropType_CropTypeID]
GO
ALTER TABLE [dbo].[ChemigationInspection]  WITH CHECK ADD  CONSTRAINT [FK_ChemigationInspection_Tillage_TillageID] FOREIGN KEY([TillageID])
REFERENCES [dbo].[Tillage] ([TillageID])
GO
ALTER TABLE [dbo].[ChemigationInspection] CHECK CONSTRAINT [FK_ChemigationInspection_Tillage_TillageID]
GO
ALTER TABLE [dbo].[ChemigationInspection]  WITH CHECK ADD  CONSTRAINT [FK_ChemigationInspection_User_InspectorUserID_UserID] FOREIGN KEY([InspectorUserID])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[ChemigationInspection] CHECK CONSTRAINT [FK_ChemigationInspection_User_InspectorUserID_UserID]