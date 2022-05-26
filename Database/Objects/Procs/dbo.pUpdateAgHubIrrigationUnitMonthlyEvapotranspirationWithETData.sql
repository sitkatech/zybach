IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.pUpdateAgHubIrrigationUnitMonthlyEvapotranspirationWithETData'))
    drop procedure dbo.pUpdateAgHubIrrigationUnitMonthlyEvapotranspirationWithETData
go

create procedure dbo.pUpdateAgHubIrrigationUnitMonthlyEvapotranspirationWithETData
as

begin
	MERGE 
	dbo.AgHubIrrigationUnitWaterYearMonthETDatum
	AS Target
	USING
	(
		SELECT ahiu.AgHubIrrigationUnitID, 
			   wym.WaterYearMonthID,
			   ogbretd.EvapotranspirationRateInches, 
			   ogbretd.EvapotranspirationRateAcreFeet
		from dbo.OpenETGoogleBucketResponseEvapotranspirationDatum ogbretd
		join AgHubIrrigationUnit ahiu on ahiu.WellTPID = ogbretd.WellTPID
		join WaterYearMonth wym on wym.[Year] = ogbretd.[WaterYear]	
							   and wym.[Month] = ogbretd.[WaterMonth]
	) AS Source
	ON Source.AgHubIrrigationUnitID = Target.AgHubIrrigationUnitID and Source.WaterYearMonthID = Target.WaterYearMonthID
	WHEN MATCHED THEN
		update set Target.EvapotranspirationRateInches = Source.EvapotranspirationRateInches,
				   Target.EvapotranspirationRateAcreFeet = Source.EvapotranspirationRateAcreFeet
    WHEN NOT MATCHED by Target THEN
		insert (AgHubIrrigationUnitID, WaterYearMonthID, EvapotranspirationRateInches, EvapotranspirationRateAcreFeet)
		values (Source.AgHubIrrigationUnitID, Source.WaterYearMonthID, Source.EvapotranspirationRateInches, Source.EvapotranspirationRateAcreFeet);
end