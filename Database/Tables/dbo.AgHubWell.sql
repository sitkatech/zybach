SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgHubWell](
	[AgHubWellID] [int] IDENTITY(1,1) NOT NULL,
	[WellRegistrationID] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[WellTPID] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[WellGeometry] [geometry] NOT NULL,
	[TPNRDPumpRate] [int] NULL,
	[TPNRDPumpRateUpdated] [datetime] NULL,
	[WellConnectedMeter] [bit] NOT NULL,
	[WellAuditPumpRate] [int] NULL,
	[AuditPumpRateUpdated] [datetime] NULL,
	[HasElectricalData] [bit] NOT NULL,
	[FetchDate] [datetime] NOT NULL,
 CONSTRAINT [PK_AgHubWell_AgHubWellID] PRIMARY KEY CLUSTERED 
(
	[AgHubWellID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
