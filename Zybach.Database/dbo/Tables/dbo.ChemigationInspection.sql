CREATE TABLE [dbo].[ChemigationInspection](
	[ChemigationInspectionID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_ChemigationInspection_ChemigationInspectionID] PRIMARY KEY,
	[ChemigationPermitAnnualRecordID] [int] NOT NULL CONSTRAINT [FK_ChemigationInspection_ChemigationPermitAnnualRecord_ChemigationPermitAnnualRecordID] FOREIGN KEY REFERENCES [dbo].[ChemigationPermitAnnualRecord] ([ChemigationPermitAnnualRecordID]),
	[ChemigationInspectionStatusID] [int] NOT NULL CONSTRAINT [FK_ChemigationInspection_ChemigationInspectionStatus_ChemigationInspectionStatusID] FOREIGN KEY REFERENCES [dbo].[ChemigationInspectionStatus] ([ChemigationInspectionStatusID]),
	[ChemigationInspectionTypeID] [int] NULL CONSTRAINT [FK_ChemigationInspection_ChemigationInspectionType_ChemigationInspectionTypeID] FOREIGN KEY REFERENCES [dbo].[ChemigationInspectionType] ([ChemigationInspectionTypeID]),
	[InspectionDate] [datetime] NULL,
	[InspectorUserID] [int] NULL CONSTRAINT [FK_ChemigationInspection_User_InspectorUserID_UserID] FOREIGN KEY REFERENCES [dbo].[User] ([UserID]),
	[ChemigationMainlineCheckValveID] [int] NULL CONSTRAINT [FK_ChemigationInspection_ChemigationMainlineCheckValve_ChemigationMainlineCheckValveID] FOREIGN KEY REFERENCES [dbo].[ChemigationMainlineCheckValve] ([ChemigationMainlineCheckValveID]),
	[HasVacuumReliefValve] [bit] NULL,
	[HasInspectionPort] [bit] NULL,
	[ChemigationLowPressureValveID] [int] NULL CONSTRAINT [FK_ChemigationInspection_ChemigationLowPressureValve_ChemigationLowPressureValveID] FOREIGN KEY REFERENCES [dbo].[ChemigationLowPressureValve] ([ChemigationLowPressureValveID]),
	[ChemigationInjectionValveID] [int] NULL CONSTRAINT [FK_ChemigationInspection_ChemigationInjectionValve_ChemigationInjectionValveID] FOREIGN KEY REFERENCES [dbo].[ChemigationInjectionValve] ([ChemigationInjectionValveID]),
	[ChemigationInterlockTypeID] [int] NULL CONSTRAINT [FK_ChemigationInspection_ChemigationInterlockType_ChemigationInterlockTypeID] FOREIGN KEY REFERENCES [dbo].[ChemigationInterlockType] ([ChemigationInterlockTypeID]),
	[TillageID] [int] NULL CONSTRAINT [FK_ChemigationInspection_Tillage_TillageID] FOREIGN KEY REFERENCES [dbo].[Tillage] ([TillageID]),
	[CropTypeID] [int] NULL CONSTRAINT [FK_ChemigationInspection_CropType_CropTypeID] FOREIGN KEY REFERENCES [dbo].[CropType] ([CropTypeID]),
	[InspectionNotes] [varchar](500) NULL,
	[ChemigationInspectionFailureReasonID] [int] NULL CONSTRAINT [FK_ChemigationInspection_ChemigationInspectionFailureReason_ChemigationInspectionFailureReasonID] FOREIGN KEY REFERENCES [dbo].[ChemigationInspectionFailureReason] ([ChemigationInspectionFailureReasonID])
)