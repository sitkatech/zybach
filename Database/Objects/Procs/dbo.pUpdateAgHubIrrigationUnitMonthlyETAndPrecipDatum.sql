IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.pUpdateAgHubIrrigationUnitMonthlyETAndPrecipDatum'))
    drop procedure dbo.pUpdateAgHubIrrigationUnitMonthlyETAndPrecipDatum
go

create procedure dbo.pUpdateAgHubIrrigationUnitMonthlyETAndPrecipDatum
as

begin
	MERGE 
	dbo.AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum
	AS Target
	USING
	(
		SELECT ahiu.AgHubIrrigationUnitID, 
			   wym.WaterYearMonthID,
			   ahiumetd.EvapotranspirationRateAcreFeet,
			   ahiumetd.EvapotranspirationRateInches,
			   ahiumpd.PrecipitationAcreFeet,
			   ahiumpd.PrecipitationInches
		from dbo.AgHubIrrigationUnitWaterYearMonthETDatum ahiumetd
		join dbo.AgHubIrrigationUnitWaterYearMonthPrecipitationDatum ahiumpd on ahiumpd.AgHubIrrigationUnitID = ahiumetd.AgHubIrrigationUnitID 
																 and ahiumpd.WaterYearMonthID = ahiumetd.WaterYearMonthID
		join AgHubIrrigationUnit ahiu on ahiu.AgHubIrrigationUnitID = ahiumetd.AgHubIrrigationUnitID
		join WaterYearMonth wym on wym.WaterYearMonthID = ahiumetd.WaterYearMonthID
	) AS Source
	ON Source.AgHubIrrigationUnitID = Target.AgHubIrrigationUnitID and Source.WaterYearMonthID = Target.WaterYearMonthID
	WHEN MATCHED THEN
		update set Target.EvapotranspirationAcreFeet = Source.EvapotranspirationRateAcreFeet,
				   Target.EvapotranspirationInches = Source.EvapotranspirationRateInches,
				   Target.PrecipitationAcreFeet = Source.PrecipitationAcreFeet,
				   Target.PrecipitationInches = Source.PrecipitationInches
    WHEN NOT MATCHED by Target THEN
		insert (AgHubIrrigationUnitID, WaterYearMonthID, EvapotranspirationAcreFeet, EvapotranspirationInches, PrecipitationAcreFeet, PrecipitationInches)
		values (Source.AgHubIrrigationUnitID, Source.WaterYearMonthID, Source.EvapotranspirationRateAcreFeet, Source.EvapotranspirationRateInches, Source.PrecipitationAcreFeet, Source.PrecipitationInches);
end