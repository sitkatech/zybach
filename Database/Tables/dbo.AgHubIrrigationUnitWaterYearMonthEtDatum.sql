SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgHubIrrigationUnitWaterYearMonthETDatum](
	[AgHubIrrigationUnitWaterYearMonthETDatumID] [int] IDENTITY(1,1) NOT NULL,
	[AgHubIrrigationUnitID] [int] NOT NULL,
	[WaterYearMonthID] [int] NOT NULL,
	[EvapotranspirationRateInches] [decimal](20, 4) NULL,
 CONSTRAINT [PK_AgHubIrrigationUnitWaterYearMonthETDatum_AgHubIrrigationUnitWaterYearMonthETDatumID] PRIMARY KEY CLUSTERED 
(
	[AgHubIrrigationUnitWaterYearMonthETDatumID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[AgHubIrrigationUnitWaterYearMonthETDatum]  WITH CHECK ADD  CONSTRAINT [FK_AgHubIrrigationUnitWaterYearMonthETDatum_AgHubIrrigationUnit_AgHubIrrigationUnitID] FOREIGN KEY([AgHubIrrigationUnitID])
REFERENCES [dbo].[AgHubIrrigationUnit] ([AgHubIrrigationUnitID])
GO
ALTER TABLE [dbo].[AgHubIrrigationUnitWaterYearMonthETDatum] CHECK CONSTRAINT [FK_AgHubIrrigationUnitWaterYearMonthETDatum_AgHubIrrigationUnit_AgHubIrrigationUnitID]
GO
ALTER TABLE [dbo].[AgHubIrrigationUnitWaterYearMonthETDatum]  WITH CHECK ADD  CONSTRAINT [FK_AgHubIrrigationUnitWaterYearMonthETDatum_WaterYearMonth_WaterYearMonthID] FOREIGN KEY([WaterYearMonthID])
REFERENCES [dbo].[WaterYearMonth] ([WaterYearMonthID])
GO
ALTER TABLE [dbo].[AgHubIrrigationUnitWaterYearMonthETDatum] CHECK CONSTRAINT [FK_AgHubIrrigationUnitWaterYearMonthETDatum_WaterYearMonth_WaterYearMonthID]