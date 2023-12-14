CREATE TABLE [dbo].[AgHubWell](
	[AgHubWellID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_AgHubWell_AgHubWellID] PRIMARY KEY,
	[WellID] [int] NOT NULL CONSTRAINT [AK_AgHubWell_WellID] UNIQUE CONSTRAINT [FK_AgHubWell_Well_WellID] FOREIGN KEY REFERENCES [dbo].[Well] ([WellID]),
	[AgHubWellGeometry] [geometry] NOT NULL,
	[WellTPNRDPumpRate] [int] NULL,
	[TPNRDPumpRateUpdated] [datetime] NULL,
	[WellConnectedMeter] [bit] NOT NULL,
	[WellAuditPumpRate] [int] NULL,
	[AuditPumpRateUpdated] [datetime] NULL,
	[HasElectricalData] [bit] NOT NULL,
	[RegisteredPumpRate] [int] NULL,
	[RegisteredUpdated] [datetime] NULL,
	[AgHubRegisteredUser] [varchar](100) NULL,
	[FieldName] [varchar](100) NULL,
	[AgHubIrrigationUnitID] [int] NULL CONSTRAINT [FK_AgHubWell_AgHubIrrigationUnit_AgHubIrrigationUnitID] FOREIGN KEY REFERENCES [dbo].[AgHubIrrigationUnit] ([AgHubIrrigationUnitID])
)