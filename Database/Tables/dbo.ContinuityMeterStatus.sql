SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContinuityMeterStatus](
	[ContinuityMeterStatusID] [int] NOT NULL,
	[ContinuityMeterStatusName] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ContinuityMeterStatusDisplayName] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_ContinuityMeterStatus_ContinuityMeterStatusID] PRIMARY KEY CLUSTERED 
(
	[ContinuityMeterStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
