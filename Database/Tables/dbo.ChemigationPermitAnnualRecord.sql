SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChemigationPermitAnnualRecord](
	[ChemigationPermitAnnualRecordID] [int] IDENTITY(1,1) NOT NULL,
	[ChemigationPermitID] [int] NOT NULL,
	[ChemigationPermitAnnualRecordStatusID] [int] NOT NULL,
	[ApplicantFirstName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ApplicantLastName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[PivotName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[RecordYear] [int] NOT NULL,
	[DateReceived] [datetime] NULL,
	[DatePaid] [datetime] NULL,
 CONSTRAINT [PK_ChemigationPermitAnnualRecord_ChemigationPermitAnnualRecordID] PRIMARY KEY CLUSTERED 
(
	[ChemigationPermitAnnualRecordID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[ChemigationPermitAnnualRecord]  WITH CHECK ADD  CONSTRAINT [FK_ChemigationPermitAnnualRecord_ChemigationPermit_ChemigationPermitID] FOREIGN KEY([ChemigationPermitID])
REFERENCES [dbo].[ChemigationPermit] ([ChemigationPermitID])
GO
ALTER TABLE [dbo].[ChemigationPermitAnnualRecord] CHECK CONSTRAINT [FK_ChemigationPermitAnnualRecord_ChemigationPermit_ChemigationPermitID]
GO
ALTER TABLE [dbo].[ChemigationPermitAnnualRecord]  WITH CHECK ADD  CONSTRAINT [FK_ChemigationPermitAnnualRecord_ChemigationPermitAnnualRecordStatus_ChemigationPermitAnnualRecordStatusID] FOREIGN KEY([ChemigationPermitAnnualRecordStatusID])
REFERENCES [dbo].[ChemigationPermitAnnualRecordStatus] ([ChemigationPermitAnnualRecordStatusID])
GO
ALTER TABLE [dbo].[ChemigationPermitAnnualRecord] CHECK CONSTRAINT [FK_ChemigationPermitAnnualRecord_ChemigationPermitAnnualRecordStatus_ChemigationPermitAnnualRecordStatusID]