SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OpenETGoogleBucketResponseEvapotranspirationDatum](
	[OpenETGoogleBucketResponseEvapotranspirationDatumID] [int] IDENTITY(1,1) NOT NULL,
	[WellTPID] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[WaterMonth] [int] NOT NULL,
	[WaterYear] [int] NOT NULL,
	[EvapotranspirationInches] [decimal](20, 4) NULL,
 CONSTRAINT [PK_OpenETGoogleBucketResponseEvapotranspirationDatum_OpenETGoogleBucketResponseEvapotranspirationDatumID] PRIMARY KEY CLUSTERED 
(
	[OpenETGoogleBucketResponseEvapotranspirationDatumID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
