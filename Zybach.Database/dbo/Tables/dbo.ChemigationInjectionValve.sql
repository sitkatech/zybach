CREATE TABLE [dbo].[ChemigationInjectionValve](
	[ChemigationInjectionValveID] [int] NOT NULL,
	[ChemigationInjectionValveName] [varchar](50) NOT NULL,
	[ChemigationInjectionValveDisplayName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_ChemigationInjectionValve_ChemigationInjectionValveID] PRIMARY KEY CLUSTERED 
(
	[ChemigationInjectionValveID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_ChemigationInjectionValve_ChemigationInjectionValveDisplayName] UNIQUE NONCLUSTERED 
(
	[ChemigationInjectionValveDisplayName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_ChemigationInjectionValve_ChemigationInjectionValveName] UNIQUE NONCLUSTERED 
(
	[ChemigationInjectionValveName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
