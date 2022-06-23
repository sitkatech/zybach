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
			   ogbretd.EvapotranspirationInches,
			   ogbretd.EvapotranspirationInches * ahiu.IrrigationUnitAreaInAcres as EtAcreInches
		from dbo.OpenETGoogleBucketResponseEvapotranspirationDatum ogbretd
		join AgHubIrrigationUnit ahiu on ahiu.WellTPID = ogbretd.WellTPID
		join WaterYearMonth wym on wym.[Year] = ogbretd.[WaterYear]	
							   and wym.[Month] = ogbretd.[WaterMonth]
	) AS Source
	ON Source.AgHubIrrigationUnitID = Target.AgHubIrrigationUnitID and Source.WaterYearMonthID = Target.WaterYearMonthID
	WHEN MATCHED THEN
		update set Target.EvapotranspirationInches = Source.EvapotranspirationInches,
				   Target.EvapotranspirationAcreInches = Source.EtAcreInches
    WHEN NOT MATCHED by Target THEN
		insert (AgHubIrrigationUnitID, WaterYearMonthID, EvapotranspirationInches, EvapotranspirationAcreInches)
		values (Source.AgHubIrrigationUnitID, Source.WaterYearMonthID, Source.EvapotranspirationInches, Source.EtAcreInches);
end