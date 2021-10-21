SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WellIrrigatedAcreStaging](
	[WellIrrigatedAcreStagingID] [int] IDENTITY(1,1) NOT NULL,
	[WellRegistrationID] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[IrrigationYear] [int] NOT NULL,
	[Acres] [float] NOT NULL,
 CONSTRAINT [PK_WellIrrigatedAcreStaging_WellIrrigatedAcreStagingID] PRIMARY KEY CLUSTERED 
(
	[WellIrrigatedAcreStagingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
