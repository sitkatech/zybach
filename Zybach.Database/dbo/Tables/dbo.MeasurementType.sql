CREATE TABLE [dbo].[MeasurementType](
	[MeasurementTypeID] [int] NOT NULL,
	[MeasurementTypeName] [varchar](100) NOT NULL,
	[MeasurementTypeDisplayName] [varchar](100) NOT NULL,
	[InfluxMeasurementName] [varchar](50) NULL,
	[InfluxFieldName] [varchar](50) NULL,
	[UnitsDisplay] [varchar](50) NOT NULL,
	[UnitsDisplayPlural] [varchar](50) NOT NULL,
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
