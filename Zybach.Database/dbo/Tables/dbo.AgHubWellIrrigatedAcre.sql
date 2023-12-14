CREATE TABLE [dbo].[AgHubWellIrrigatedAcre](
	[AgHubWellIrrigatedAcreID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_AgHubWellIrrigatedAcre_AgHubWellIrrigatedAcreID] PRIMARY KEY,
	[AgHubWellID] [int] NOT NULL CONSTRAINT [FK_AgHubWellIrrigatedAcre_AgHubWell_AgHubWellID] FOREIGN KEY REFERENCES [dbo].[AgHubWell] ([AgHubWellID]),
	[IrrigationYear] [int] NOT NULL,
	[Acres] [float] NOT NULL,
	[CropType] [varchar](50) NULL,
	[Tillage] [varchar](50) NULL,
	CONSTRAINT [AK_AgHubWellIrrigatedAcre_AgHubWellID_IrrigationYear] UNIQUE 
	(
		[AgHubWellID] ASC,
		[IrrigationYear] ASC
	)
)