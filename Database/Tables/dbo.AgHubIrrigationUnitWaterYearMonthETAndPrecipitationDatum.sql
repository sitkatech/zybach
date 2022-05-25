SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum](
	[AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatumID] [int] IDENTITY(1,1) NOT NULL,
	[AgHubIrrigationUnitID] [int] NOT NULL,
	[WaterYearMonthID] [int] NOT NULL,
	[EvapotranspirationAcreFeet] [decimal](20, 4) NULL,
	[EvapotranspirationInches] [decimal](20, 4) NULL,
	[PrecipitationAcreFeet] [decimal](20, 4) NULL,
	[PrecipitationInches] [decimal](20, 4) NULL,
 CONSTRAINT [PK_AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum_AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatumID] PRIMARY KEY CLUSTERED 
(
	[AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatumID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum]  WITH CHECK ADD  CONSTRAINT [FK_AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum_AgHubIrrigationUnit_AgHubIrrigationUnitID] FOREIGN KEY([AgHubIrrigationUnitID])
REFERENCES [dbo].[AgHubIrrigationUnit] ([AgHubIrrigationUnitID])
GO
ALTER TABLE [dbo].[AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum] CHECK CONSTRAINT [FK_AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum_AgHubIrrigationUnit_AgHubIrrigationUnitID]
GO
ALTER TABLE [dbo].[AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum]  WITH CHECK ADD  CONSTRAINT [FK_AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum_WaterYearMonth_WaterYearMonthID] FOREIGN KEY([WaterYearMonthID])
REFERENCES [dbo].[WaterYearMonth] ([WaterYearMonthID])
GO
ALTER TABLE [dbo].[AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum] CHECK CONSTRAINT [FK_AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum_WaterYearMonth_WaterYearMonthID]