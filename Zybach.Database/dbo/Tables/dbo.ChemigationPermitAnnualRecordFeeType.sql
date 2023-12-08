CREATE TABLE [dbo].[ChemigationPermitAnnualRecordFeeType](
	[ChemigationPermitAnnualRecordFeeTypeID] [int] NOT NULL,
	[ChemigationPermitAnnualRecordFeeTypeName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ChemigationPermitAnnualRecordFeeTypeDisplayName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[FeeAmount] [money] NOT NULL,
 CONSTRAINT [PK_ChemigationPermitAnnualRecordFeeType_ChemigationPermitAnnualRecordFeeTypeID] PRIMARY KEY CLUSTERED 
(
	[ChemigationPermitAnnualRecordFeeTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_ChemigationPermitAnnualRecordFeeType_ChemigationPermitAnnualRecordFeeTypeDisplayName] UNIQUE NONCLUSTERED 
(
	[ChemigationPermitAnnualRecordFeeTypeDisplayName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_ChemigationPermitAnnualRecordFeeType_ChemigationPermitAnnualRecordFeeTypeName] UNIQUE NONCLUSTERED 
(
	[ChemigationPermitAnnualRecordFeeTypeName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
