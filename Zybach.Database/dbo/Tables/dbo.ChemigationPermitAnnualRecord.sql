CREATE TABLE [dbo].[ChemigationPermitAnnualRecord](
	[ChemigationPermitAnnualRecordID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_ChemigationPermitAnnualRecord_ChemigationPermitAnnualRecordID] PRIMARY KEY,
	[ChemigationPermitID] [int] NOT NULL CONSTRAINT [FK_ChemigationPermitAnnualRecord_ChemigationPermit_ChemigationPermitID] FOREIGN KEY REFERENCES [dbo].[ChemigationPermit] ([ChemigationPermitID]),
	[RecordYear] [int] NOT NULL,
	[ChemigationPermitAnnualRecordStatusID] [int] NOT NULL CONSTRAINT [FK_ChemigationPermitAnnualRecord_ChemigationPermitAnnualRecordStatus_ChemigationPermitAnnualRecordStatusID] FOREIGN KEY REFERENCES [dbo].[ChemigationPermitAnnualRecordStatus] ([ChemigationPermitAnnualRecordStatusID]),
	[PivotName] [varchar](100) NULL,
	[ChemigationInjectionUnitTypeID] [int] NOT NULL CONSTRAINT [FK_ChemigationPermitAnnualRecord_ChemigationInjectionUnitType_ChemigationInjectionUnitTypeID] FOREIGN KEY REFERENCES [dbo].[ChemigationInjectionUnitType] ([ChemigationInjectionUnitTypeID]),
	[ApplicantFirstName] [varchar](200) NULL,
	[ApplicantLastName] [varchar](200) NULL,
	[ApplicantMailingAddress] [varchar](100) NULL,
	[ApplicantCity] [varchar](50) NULL,
	[ApplicantState] [varchar](20) NULL,
	[ApplicantZipCode] [varchar](10) NULL,
	[ApplicantPhone] [varchar](30) NULL,
	[ApplicantMobilePhone] [varchar](30) NULL,
	[DateReceived] [datetime] NULL,
	[DatePaid] [datetime] NULL,
	[ApplicantEmail] [varchar](255) NULL,
	[NDEEAmount] [decimal](4, 2) NULL,
	[TownshipRangeSection] [varchar](100) NOT NULL,
	[ApplicantCompany] [varchar](200) NULL,
	[AnnualNotes] [varchar](500) NULL,
	[DateApproved] [datetime] NULL,
	[ChemigationPermitAnnualRecordFeeTypeID] [int] NULL CONSTRAINT [FK_ChemigationPermitAnnualRecord_ChemigationPermitAnnualRecordFeeType_ChemigationPermitAnnualRecordFeeTypeID] FOREIGN KEY REFERENCES [dbo].[ChemigationPermitAnnualRecordFeeType] ([ChemigationPermitAnnualRecordFeeTypeID]),
	CONSTRAINT [AK_ChemigationPermitAnnualRecord_ChemigationPermitID_RecordYear] UNIQUE 
	(
		[ChemigationPermitID] ASC,
		[RecordYear] ASC
	)
)