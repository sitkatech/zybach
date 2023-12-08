SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChemigationPermitAnnualRecordApplicator](
	[ChemigationPermitAnnualRecordApplicatorID] [int] IDENTITY(1,1) NOT NULL,
	[ChemigationPermitAnnualRecordID] [int] NOT NULL,
	[ApplicatorName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CertificationNumber] [int] NULL,
	[ExpirationYear] [int] NULL,
	[HomePhone] [varchar](30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MobilePhone] [varchar](30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_ChemigationPermitAnnualRecordApplicator_ChemigationPermitAnnualRecordApplicatorID] PRIMARY KEY CLUSTERED 
(
	[ChemigationPermitAnnualRecordApplicatorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[ChemigationPermitAnnualRecordApplicator]  WITH CHECK ADD  CONSTRAINT [FK_ChemigationPermitAnnualRecordApplicator_ChemigationPermitAnnualRecord_ChemigationPermitAnnualRecordID] FOREIGN KEY([ChemigationPermitAnnualRecordID])
REFERENCES [dbo].[ChemigationPermitAnnualRecord] ([ChemigationPermitAnnualRecordID])
GO
ALTER TABLE [dbo].[ChemigationPermitAnnualRecordApplicator] CHECK CONSTRAINT [FK_ChemigationPermitAnnualRecordApplicator_ChemigationPermitAnnualRecord_ChemigationPermitAnnualRecordID]