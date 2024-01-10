CREATE TABLE [dbo].[ChemigationLowPressureValve](
	[ChemigationLowPressureValveID] [int] NOT NULL,
	[ChemigationLowPressureValveName] [varchar](50) NOT NULL,
	[ChemigationLowPressureValveDisplayName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_ChemigationLowPressureValve_ChemigationLowPressureValveID] PRIMARY KEY CLUSTERED 
(
	[ChemigationLowPressureValveID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_ChemigationLowPressureValve_ChemigationLowPressureValveDisplayName] UNIQUE NONCLUSTERED 
(
	[ChemigationLowPressureValveDisplayName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_ChemigationLowPressureValve_ChemigationLowPressureValveName] UNIQUE NONCLUSTERED 
(
	[ChemigationLowPressureValveName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
