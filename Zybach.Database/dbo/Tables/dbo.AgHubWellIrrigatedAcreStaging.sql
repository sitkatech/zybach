CREATE TABLE [dbo].[AgHubWellIrrigatedAcreStaging](
	[AgHubWellIrrigatedAcreStagingID] [int] IDENTITY(1,1) NOT NULL,
	[WellRegistrationID] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[IrrigationYear] [int] NOT NULL,
	[Acres] [float] NOT NULL,
	[CropType] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Tillage] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_AgHubWellIrrigatedAcreStaging_AgHubWellIrrigatedAcreStagingID] PRIMARY KEY CLUSTERED 
(
	[AgHubWellIrrigatedAcreStagingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]