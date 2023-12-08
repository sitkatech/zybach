SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgHubWellStaging](
	[AgHubWellStagingID] [int] IDENTITY(1,1) NOT NULL,
	[WellRegistrationID] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[WellTPID] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[WellGeometry] [geometry] NOT NULL,
	[WellTPNRDPumpRate] [int] NULL,
	[TPNRDPumpRateUpdated] [datetime] NULL,
	[WellConnectedMeter] [bit] NOT NULL,
	[WellAuditPumpRate] [int] NULL,
	[AuditPumpRateUpdated] [datetime] NULL,
	[RegisteredPumpRate] [int] NULL,
	[RegisteredUpdated] [datetime] NULL,
	[HasElectricalData] [bit] NOT NULL,
	[AgHubRegisteredUser] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[FieldName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[IrrigationUnitGeometry] [geometry] NULL,
 CONSTRAINT [PK_AgHubWellStaging_AgHubWellStagingID] PRIMARY KEY CLUSTERED 
(
	[AgHubWellStagingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
