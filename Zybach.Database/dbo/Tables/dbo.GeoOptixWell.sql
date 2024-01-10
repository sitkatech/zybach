CREATE TABLE [dbo].[GeoOptixWell](
	[GeoOptixWellID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_GeoOptixWell_GeoOptixWellID] PRIMARY KEY,
	[WellID] [int] NOT NULL CONSTRAINT [AK_GeoOptixWell_WellID] UNIQUE CONSTRAINT [FK_GeoOptixWell_Well_WellID] FOREIGN KEY REFERENCES [dbo].[Well] ([WellID]),
	[GeoOptixWellGeometry] [geometry] NOT NULL	
)