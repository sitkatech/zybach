CREATE TABLE [dbo].[MeasurementType](
	[MeasurementTypeID] [int] NOT NULL,
	[MeasurementTypeName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[MeasurementTypeDisplayName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[InfluxMeasurementName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[InfluxFieldName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UnitsDisplay] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[UnitsDisplayPlural] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_MeasurementType_MeasurementTypeID] PRIMARY KEY CLUSTERED 
(
	[MeasurementTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_MeasurementType_MeasurementTypeDisplayName] UNIQUE NONCLUSTERED 
(
	[MeasurementTypeDisplayName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_MeasurementType_MeasurementTypeName] UNIQUE NONCLUSTERED 
(
	[MeasurementTypeName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]