CREATE TABLE [dbo].[AgHubIrrigationUnitGeometry](
	[AgHubIrrigationUnitGeometryID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_AgHubIrrigationUnitGeometry_AgHubIrrigationUnitGeometryID] PRIMARY KEY,
	[AgHubIrrigationUnitID] [int] NOT NULL CONSTRAINT [AK_AgHubIrrigationUnitGeometry_AgHubIrrigationUnitID] UNIQUE CONSTRAINT [FK_AgHubIrrigationUnitGeometry_AgHubIrrigationUnit_AgHubIrrigationUnitID] FOREIGN KEY REFERENCES [dbo].[AgHubIrrigationUnit] ([AgHubIrrigationUnitID]),
	[IrrigationUnitGeometry] [geometry] NOT NULL
)