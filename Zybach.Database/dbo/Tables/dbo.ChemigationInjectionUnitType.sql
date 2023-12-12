CREATE TABLE [dbo].[ChemigationInjectionUnitType](
	[ChemigationInjectionUnitTypeID] [int] NOT NULL,
	[ChemigationInjectionUnitTypeName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ChemigationInjectionUnitTypeDisplayName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_ChemigationInjectionUnitType_ChemigationInjectionUnitTypeID] PRIMARY KEY CLUSTERED 
(
	[ChemigationInjectionUnitTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_ChemigationInjectionUnitType_ChemigationInjectionUnitTypeDisplayName] UNIQUE NONCLUSTERED 
(
	[ChemigationInjectionUnitTypeDisplayName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_ChemigationInjectionUnitType_ChemigationInjectionUnitTypeName] UNIQUE NONCLUSTERED 
(
	[ChemigationInjectionUnitTypeName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
