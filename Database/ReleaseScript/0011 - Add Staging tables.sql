alter table dbo.AghubWell alter column WellTPID varchar(100) null
alter table dbo.Aghubwell drop constraint AK_AgHubWell_WellTPID
alter table dbo.Aghubwell drop constraint AK_AgHubWell_WellRegistrationID

CREATE TABLE [dbo].[AgHubWellStaging](
	[AgHubWellStagingID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_AgHubWellStaging_AgHubWellStagingID] PRIMARY KEY,
	[WellRegistrationID] [varchar](100) NOT NULL,
	[WellTPID] [varchar](100) NULL,
	[WellGeometry] [geometry] NOT NULL,
	[TPNRDPumpRate] [int] NULL,
	[TPNRDPumpRateUpdated] [datetime] NULL,
	[WellConnectedMeter] [bit] NULL,
	[WellAuditPumpRate] [int] NULL,
	[AuditPumpRateUpdated] [datetime] NULL,
	[HasElectricalData] [bit] NULL,
	[FetchDate] [datetime] NOT NULL
)


CREATE TABLE [dbo].[AgHubWellIrrigatedAcreStaging](
	[AgHubWellIrrigatedAcreStagingID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_AgHubWellIrrigatedAcreStaging_AgHubWellIrrigatedAcreStagingID] PRIMARY KEY,
	[WellRegistrationID] [varchar](100) NOT NULL,
	[IrrigationYear] [int] NOT NULL,
	[Acres] [float] NOT NULL
)

CREATE TABLE [dbo].[WellSensorMeasurementStaging](
	[WellSensorMeasurementStagingID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_WellSensorMeasurementStaging_WellSensorMeasurementStagingID] PRIMARY KEY,
	[WellRegistrationID] [varchar](100) NOT NULL,
	[MeasurementTypeID] [int] NOT NULL CONSTRAINT [FK_WellSensorMeasurementStaging_MeasurementType_MeasurementTypeID] FOREIGN KEY REFERENCES [dbo].[MeasurementType] ([MeasurementTypeID]),
	[ReadingDate] [datetime] NOT NULL,
	[SensorName] [varchar](100) NULL,
	[MeasurementValue] [float] NOT NULL
)
GO
