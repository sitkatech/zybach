CREATE TABLE [dbo].[ChemigationPermitAnnualRecordChemicalFormulation](
	[ChemigationPermitAnnualRecordChemicalFormulationID] [int] IDENTITY(1,1) NOT NULL,
	[ChemigationPermitAnnualRecordID] [int] NOT NULL,
	[ChemicalFormulationID] [int] NOT NULL,
	[ChemicalUnitID] [int] NOT NULL,
	[TotalApplied] [decimal](8, 2) NULL,
	[AcresTreated] [decimal](8, 2) NOT NULL,
 CONSTRAINT [PK_ChemigationPermitAnnualRecordChemicalFormulation_ChemigationPermitAnnualRecordChemicalFormulationID] PRIMARY KEY CLUSTERED 
(
	[ChemigationPermitAnnualRecordChemicalFormulationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_ChemigationPermitAnnualRecordChemicalFormulation_ChemigationPermitAnnualRecordID_ChemicalFormulationID_ChemicalUnitID] UNIQUE NONCLUSTERED 
(
	[ChemigationPermitAnnualRecordID] ASC,
	[ChemicalFormulationID] ASC,
	[ChemicalUnitID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[ChemigationPermitAnnualRecordChemicalFormulation]  WITH CHECK ADD  CONSTRAINT [FK_ChemigationPermitAnnualRecordChemicalFormulation_ChemicalFormulation_ChemicalFormulationID] FOREIGN KEY([ChemicalFormulationID])
REFERENCES [dbo].[ChemicalFormulation] ([ChemicalFormulationID])
GO
ALTER TABLE [dbo].[ChemigationPermitAnnualRecordChemicalFormulation] CHECK CONSTRAINT [FK_ChemigationPermitAnnualRecordChemicalFormulation_ChemicalFormulation_ChemicalFormulationID]
GO
ALTER TABLE [dbo].[ChemigationPermitAnnualRecordChemicalFormulation]  WITH CHECK ADD  CONSTRAINT [FK_ChemigationPermitAnnualRecordChemicalFormulation_ChemicalUnit_ChemicalUnitID] FOREIGN KEY([ChemicalUnitID])
REFERENCES [dbo].[ChemicalUnit] ([ChemicalUnitID])
GO
ALTER TABLE [dbo].[ChemigationPermitAnnualRecordChemicalFormulation] CHECK CONSTRAINT [FK_ChemigationPermitAnnualRecordChemicalFormulation_ChemicalUnit_ChemicalUnitID]
GO
ALTER TABLE [dbo].[ChemigationPermitAnnualRecordChemicalFormulation]  WITH CHECK ADD  CONSTRAINT [FK_ChemigationPermitAnnualRecordChemicalFormulation_ChemigationPermitAnnualRecord_ChemigationPermitAnnualRecordID] FOREIGN KEY([ChemigationPermitAnnualRecordID])
REFERENCES [dbo].[ChemigationPermitAnnualRecord] ([ChemigationPermitAnnualRecordID])
GO
ALTER TABLE [dbo].[ChemigationPermitAnnualRecordChemicalFormulation] CHECK CONSTRAINT [FK_ChemigationPermitAnnualRecordChemicalFormulation_ChemigationPermitAnnualRecord_ChemigationPermitAnnualRecordID]