CREATE TABLE [dbo].[AgHubWellIrrigatedAcre](
	[AgHubWellIrrigatedAcreID] [int] IDENTITY(1,1) NOT NULL,
	[AgHubWellID] [int] NOT NULL,
	[IrrigationYear] [int] NOT NULL,
	[Acres] [float] NOT NULL,
	[CropType] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Tillage] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_AgHubWellIrrigatedAcre_AgHubWellIrrigatedAcreID] PRIMARY KEY CLUSTERED 
(
	[AgHubWellIrrigatedAcreID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_AgHubWellIrrigatedAcre_AgHubWellID_IrrigationYear] UNIQUE NONCLUSTERED 
(
	[AgHubWellID] ASC,
	[IrrigationYear] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[AgHubWellIrrigatedAcre]  WITH CHECK ADD  CONSTRAINT [FK_AgHubWellIrrigatedAcre_AgHubWell_AgHubWellID] FOREIGN KEY([AgHubWellID])
REFERENCES [dbo].[AgHubWell] ([AgHubWellID])
GO
ALTER TABLE [dbo].[AgHubWellIrrigatedAcre] CHECK CONSTRAINT [FK_AgHubWellIrrigatedAcre_AgHubWell_AgHubWellID]