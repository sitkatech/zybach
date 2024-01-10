CREATE TABLE [dbo].[ChemigationPermitAnnualRecordChemicalFormulation](
	[ChemigationPermitAnnualRecordChemicalFormulationID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_ChemigationPermitAnnualRecordChemicalFormulation_ChemigationPermitAnnualRecordChemicalFormulationID] PRIMARY KEY,
	[ChemigationPermitAnnualRecordID] [int] NOT NULL CONSTRAINT [FK_ChemigationPermitAnnualRecordChemicalFormulation_ChemigationPermitAnnualRecord_ChemigationPermitAnnualRecordID] FOREIGN KEY REFERENCES [dbo].[ChemigationPermitAnnualRecord] ([ChemigationPermitAnnualRecordID]),
	[ChemicalFormulationID] [int] NOT NULL CONSTRAINT [FK_ChemigationPermitAnnualRecordChemicalFormulation_ChemicalFormulation_ChemicalFormulationID] FOREIGN KEY REFERENCES [dbo].[ChemicalFormulation] ([ChemicalFormulationID]),
	[ChemicalUnitID] [int] NOT NULL CONSTRAINT [FK_ChemigationPermitAnnualRecordChemicalFormulation_ChemicalUnit_ChemicalUnitID] FOREIGN KEY REFERENCES [dbo].[ChemicalUnit] ([ChemicalUnitID]),
	[TotalApplied] [decimal](8, 2) NULL,
	[AcresTreated] [decimal](8, 2) NOT NULL,
	CONSTRAINT [AK_ChemigationPermitAnnualRecordChemicalFormulation_ChemigationPermitAnnualRecordID_ChemicalFormulationID_ChemicalUnitID] UNIQUE
	(
		[ChemigationPermitAnnualRecordID] ASC,
		[ChemicalFormulationID] ASC,
		[ChemicalUnitID] ASC
	)
)