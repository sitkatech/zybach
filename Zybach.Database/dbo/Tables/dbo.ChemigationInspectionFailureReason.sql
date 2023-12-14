CREATE TABLE [dbo].[ChemigationInspectionFailureReason](
	[ChemigationInspectionFailureReasonID] [int] NOT NULL,
	[ChemigationInspectionFailureReasonName] [varchar](50) NOT NULL,
	[ChemigationInspectionFailureReasonDisplayName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_ChemigationInspectionFailureReason_ChemigationInspectionFailureReasonID] PRIMARY KEY CLUSTERED 
(
	[ChemigationInspectionFailureReasonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_ChemigationInspectionFailureReason_ChemigationInspectionFailureReasonDisplayName] UNIQUE NONCLUSTERED 
(
	[ChemigationInspectionFailureReasonDisplayName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_ChemigationInspectionFailureReason_ChemigationInspectionFailureReasonName] UNIQUE NONCLUSTERED 
(
	[ChemigationInspectionFailureReasonName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
