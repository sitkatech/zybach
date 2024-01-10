CREATE TABLE [dbo].[ChemigationInspectionStatus](
	[ChemigationInspectionStatusID] [int] NOT NULL,
	[ChemigationInspectionStatusName] [varchar](50) NOT NULL,
	[ChemigationInspectionStatusDisplayName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_ChemigationInspectionStatus_ChemigationInspectionStatusID] PRIMARY KEY CLUSTERED 
(
	[ChemigationInspectionStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_ChemigationInspectionStatus_ChemigationInspectionStatusDisplayName] UNIQUE NONCLUSTERED 
(
	[ChemigationInspectionStatusDisplayName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_ChemigationInspectionStatus_ChemigationInspectionStatusName] UNIQUE NONCLUSTERED 
(
	[ChemigationInspectionStatusName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
