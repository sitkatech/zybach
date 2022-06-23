SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OpenETGoogleBucketResponsePrecipitationDatum](
	[OpenETGoogleBucketResponsePrecipitationDatumID] [int] IDENTITY(1,1) NOT NULL,
	[WellTPID] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[WaterMonth] [int] NOT NULL,
	[WaterYear] [int] NOT NULL,
	[PrecipitationInches] [decimal](20, 4) NULL,
 CONSTRAINT [PK_OpenETGoogleBucketResponsePrecipitationDatum_OpenETGoogleBucketResponsePrecipitationDatumID] PRIMARY KEY CLUSTERED 
(
	[OpenETGoogleBucketResponsePrecipitationDatumID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
