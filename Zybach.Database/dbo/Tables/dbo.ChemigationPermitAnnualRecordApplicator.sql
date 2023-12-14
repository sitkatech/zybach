CREATE TABLE [dbo].[ChemigationPermitAnnualRecordApplicator](
	[ChemigationPermitAnnualRecordApplicatorID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_ChemigationPermitAnnualRecordApplicator_ChemigationPermitAnnualRecordApplicatorID] PRIMARY KEY,
	[ChemigationPermitAnnualRecordID] [int] NOT NULL CONSTRAINT [FK_ChemigationPermitAnnualRecordApplicator_ChemigationPermitAnnualRecord_ChemigationPermitAnnualRecordID] FOREIGN KEY REFERENCES [dbo].[ChemigationPermitAnnualRecord] ([ChemigationPermitAnnualRecordID]),
	[ApplicatorName] [varchar](100) NOT NULL,
	[CertificationNumber] [int] NULL,
	[ExpirationYear] [int] NULL,
	[HomePhone] [varchar](30) NULL,
	[MobilePhone] [varchar](30) NULL
)