SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChemigationPermitAnnualRecordWell](
	[ChemigationPermitAnnualRecordWellID] [int] IDENTITY(1,1) NOT NULL,
	[ChemigationPermitAnnualRecordID] [int] NOT NULL,
	[WellID] [int] NOT NULL,
 CONSTRAINT [PK_ChemigationPermitAnnualRecordWell_ChemigationPermitAnnualRecordWellID] PRIMARY KEY CLUSTERED 
(
	[ChemigationPermitAnnualRecordWellID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_ChemigationPermitAnnualRecordWell_ChemigationPermitAnnualRecordID_WellID] UNIQUE NONCLUSTERED 
(
	[ChemigationPermitAnnualRecordID] ASC,
	[WellID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[ChemigationPermitAnnualRecordWell]  WITH CHECK ADD  CONSTRAINT [FK_ChemigationPermitAnnualRecordWell_ChemigationPermitAnnualRecord_ChemigationPermitAnnualRecordID] FOREIGN KEY([ChemigationPermitAnnualRecordID])
REFERENCES [dbo].[ChemigationPermitAnnualRecord] ([ChemigationPermitAnnualRecordID])
GO
ALTER TABLE [dbo].[ChemigationPermitAnnualRecordWell] CHECK CONSTRAINT [FK_ChemigationPermitAnnualRecordWell_ChemigationPermitAnnualRecord_ChemigationPermitAnnualRecordID]
GO
ALTER TABLE [dbo].[ChemigationPermitAnnualRecordWell]  WITH CHECK ADD  CONSTRAINT [FK_ChemigationPermitAnnualRecordWell_Well_WellID] FOREIGN KEY([WellID])
REFERENCES [dbo].[Well] ([WellID])
GO
ALTER TABLE [dbo].[ChemigationPermitAnnualRecordWell] CHECK CONSTRAINT [FK_ChemigationPermitAnnualRecordWell_Well_WellID]