SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChemigationPermit](
	[ChemigationPermitID] [int] IDENTITY(1,1) NOT NULL,
	[ChemigationPermitNumber] [int] NOT NULL,
	[ChemigationPermitStatusID] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[CountyID] [int] NOT NULL,
	[WellID] [int] NULL,
 CONSTRAINT [PK_ChemigationPermit_ChemigationPermitID] PRIMARY KEY CLUSTERED 
(
	[ChemigationPermitID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_ChemigationPermit_ChemigationPermitNumber] UNIQUE NONCLUSTERED 
(
	[ChemigationPermitNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[ChemigationPermit]  WITH CHECK ADD  CONSTRAINT [FK_ChemigationPermit_ChemigationPermitStatus_ChemigationPermitStatusID] FOREIGN KEY([ChemigationPermitStatusID])
REFERENCES [dbo].[ChemigationPermitStatus] ([ChemigationPermitStatusID])
GO
ALTER TABLE [dbo].[ChemigationPermit] CHECK CONSTRAINT [FK_ChemigationPermit_ChemigationPermitStatus_ChemigationPermitStatusID]
GO
ALTER TABLE [dbo].[ChemigationPermit]  WITH CHECK ADD  CONSTRAINT [FK_ChemigationPermit_County_CountyID] FOREIGN KEY([CountyID])
REFERENCES [dbo].[County] ([CountyID])
GO
ALTER TABLE [dbo].[ChemigationPermit] CHECK CONSTRAINT [FK_ChemigationPermit_County_CountyID]
GO
ALTER TABLE [dbo].[ChemigationPermit]  WITH CHECK ADD  CONSTRAINT [FK_ChemigationPermit_Well_WellID] FOREIGN KEY([WellID])
REFERENCES [dbo].[Well] ([WellID])
GO
ALTER TABLE [dbo].[ChemigationPermit] CHECK CONSTRAINT [FK_ChemigationPermit_Well_WellID]