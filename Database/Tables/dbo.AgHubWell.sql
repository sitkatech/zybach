SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgHubWell](
	[AgHubWellID] [int] IDENTITY(1,1) NOT NULL,
	[WellRegistrationID] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[WellTPID] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[WellGeometry] [geometry] NOT NULL,
	[WellTPNRDPumpRate] [int] NULL,
	[TPNRDPumpRateUpdated] [datetime] NULL,
	[WellConnectedMeter] [bit] NOT NULL,
	[WellAuditPumpRate] [int] NULL,
	[AuditPumpRateUpdated] [datetime] NULL,
	[HasElectricalData] [bit] NOT NULL,
	[FetchDate] [datetime] NOT NULL,
	[RegisteredPumpRate] [int] NULL,
	[RegisteredUpdated] [datetime] NULL,
	[StreamflowZoneID] [int] NULL,
	[LandownerName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[FieldName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_AgHubWell_AgHubWellID] PRIMARY KEY CLUSTERED 
(
	[AgHubWellID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[AgHubWell]  WITH CHECK ADD  CONSTRAINT [FK_AghubWell_StreamFlowZone_StreamFlowZoneID] FOREIGN KEY([StreamflowZoneID])
REFERENCES [dbo].[StreamFlowZone] ([StreamFlowZoneID])
GO
ALTER TABLE [dbo].[AgHubWell] CHECK CONSTRAINT [FK_AghubWell_StreamFlowZone_StreamFlowZoneID]