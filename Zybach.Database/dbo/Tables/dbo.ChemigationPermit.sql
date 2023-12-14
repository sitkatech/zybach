CREATE TABLE [dbo].[ChemigationPermit](
	[ChemigationPermitID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_ChemigationPermit_ChemigationPermitID] PRIMARY KEY,
	[ChemigationPermitNumber] [int] NOT NULL CONSTRAINT [AK_ChemigationPermit_ChemigationPermitNumber] UNIQUE,
	[ChemigationPermitStatusID] [int] NOT NULL CONSTRAINT [FK_ChemigationPermit_ChemigationPermitStatus_ChemigationPermitStatusID] FOREIGN KEY REFERENCES [dbo].[ChemigationPermitStatus] ([ChemigationPermitStatusID]),
	[DateCreated] [datetime] NOT NULL,
	[CountyID] [int] NOT NULL CONSTRAINT [FK_ChemigationPermit_County_CountyID] FOREIGN KEY REFERENCES [dbo].[County] ([CountyID]),
	[WellID] [int] NULL CONSTRAINT [FK_ChemigationPermit_Well_WellID] FOREIGN KEY REFERENCES [dbo].[Well] ([WellID])
)