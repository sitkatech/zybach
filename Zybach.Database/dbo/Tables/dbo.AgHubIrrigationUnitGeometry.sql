CREATE TABLE [dbo].[AgHubIrrigationUnitGeometry](
	[AgHubIrrigationUnitGeometryID] [int] IDENTITY(1,1) NOT NULL,
	[AgHubIrrigationUnitID] [int] NOT NULL,
	[IrrigationUnitGeometry] [geometry] NOT NULL,
 CONSTRAINT [PK_AgHubIrrigationUnitGeometry_AgHubIrrigationUnitGeometryID] PRIMARY KEY CLUSTERED 
(
	[AgHubIrrigationUnitGeometryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_AgHubIrrigationUnitGeometry_AgHubIrrigationUnitID] UNIQUE NONCLUSTERED 
(
	[AgHubIrrigationUnitID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[AgHubIrrigationUnitGeometry]  WITH CHECK ADD  CONSTRAINT [FK_AgHubIrrigationUnitGeometry_AgHubIrrigationUnit_AgHubIrrigationUnitID] FOREIGN KEY([AgHubIrrigationUnitID])
REFERENCES [dbo].[AgHubIrrigationUnit] ([AgHubIrrigationUnitID])
GO
ALTER TABLE [dbo].[AgHubIrrigationUnitGeometry] CHECK CONSTRAINT [FK_AgHubIrrigationUnitGeometry_AgHubIrrigationUnit_AgHubIrrigationUnitID]