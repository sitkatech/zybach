CREATE TABLE [dbo].[ChemicalFormulation](
	[ChemicalFormulationID] [int] IDENTITY(1,1) NOT NULL,
	[ChemicalFormulationName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ChemicalFormulationDisplayName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_ChemicalFormulation_ChemicalFormulationID] PRIMARY KEY CLUSTERED 
(
	[ChemicalFormulationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_ChemicalFormulation_ChemicalFormulationDisplayName] UNIQUE NONCLUSTERED 
(
	[ChemicalFormulationDisplayName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_ChemicalFormulation_ChemicalFormulationName] UNIQUE NONCLUSTERED 
(
	[ChemicalFormulationName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]