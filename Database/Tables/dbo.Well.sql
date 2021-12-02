SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Well](
	[WellID] [int] IDENTITY(1,1) NOT NULL,
	[WellRegistrationID] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[WellGeometry] [geometry] NOT NULL,
	[StreamflowZoneID] [int] NULL,
	[CreateDate] [datetime] NOT NULL,
	[LastUpdateDate] [datetime] NULL,
 CONSTRAINT [PK_Well_WellID] PRIMARY KEY CLUSTERED 
(
	[WellID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[Well]  WITH CHECK ADD  CONSTRAINT [FK_Well_StreamFlowZone_StreamFlowZoneID] FOREIGN KEY([StreamflowZoneID])
REFERENCES [dbo].[StreamFlowZone] ([StreamFlowZoneID])
GO
ALTER TABLE [dbo].[Well] CHECK CONSTRAINT [FK_Well_StreamFlowZone_StreamFlowZoneID]