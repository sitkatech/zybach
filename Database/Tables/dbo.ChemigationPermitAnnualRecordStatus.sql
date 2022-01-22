SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChemigationPermitAnnualRecordStatus](
	[ChemigationPermitAnnualRecordStatusID] [int] NOT NULL,
	[ChemigationPermitAnnualRecordStatusName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ChemigationPermitAnnualRecordStatusDisplayName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_ChemigationPermitAnnualRecordStatus_ChemigationPermitAnnualRecordStatusID] PRIMARY KEY CLUSTERED 
(
	[ChemigationPermitAnnualRecordStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_ChemigationPermitAnnualRecordStatus_ChemigationPermitAnnualRecordStatusDisplayName] UNIQUE NONCLUSTERED 
(
	[ChemigationPermitAnnualRecordStatusDisplayName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_ChemigationPermitAnnualRecordStatus_ChemigationPermitAnnualRecordStatusName] UNIQUE NONCLUSTERED 
(
	[ChemigationPermitAnnualRecordStatusName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
