CREATE TABLE [dbo].[ContinuityMeterStatus](
	[ContinuityMeterStatusID] [int] NOT NULL,
	[ContinuityMeterStatusName] [varchar](20) NOT NULL,
	[ContinuityMeterStatusDisplayName] [varchar](20) NOT NULL,
 CONSTRAINT [PK_ContinuityMeterStatus_ContinuityMeterStatusID] PRIMARY KEY CLUSTERED 
(
	[ContinuityMeterStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
