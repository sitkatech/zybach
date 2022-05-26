IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.pUpdateAgHubIrrigationUnitMonthlyPrecipitationWithETData'))
    drop procedure dbo.pUpdateAgHubIrrigationUnitMonthlyPrecipitationWithETData
go

create procedure dbo.pUpdateAgHubIrrigationUnitMonthlyPrecipitationWithETData
as

begin
	MERGE 
	dbo.AgHubIrrigationUnitWaterYearMonthPrecipitationDatum
	AS Target
	USING
	(
		SELECT ahiu.AgHubIrrigationUnitID, 
			   wym.WaterYearMonthID,
			   ogbrpd.PrecipitationAcreFeet,
			   ogbrpd.PrecipitationInches
		from dbo.OpenETGoogleBucketResponsePrecipitationDatum ogbrpd
		join AgHubIrrigationUnit ahiu on ahiu.WellTPID = ogbrpd.WellTPID
		join WaterYearMonth wym on wym.[Year] = ogbrpd.[WaterYear]	
							   and wym.[Month] = ogbrpd.[WaterMonth]
	) AS Source
	ON Source.AgHubIrrigationUnitID = Target.AgHubIrrigationUnitID and Source.WaterYearMonthID = Target.WaterYearMonthID
	WHEN MATCHED THEN
		update set Target.PrecipitationAcreFeet = Source.PrecipitationAcreFeet,
				   Target.PrecipitationInches = Source.PrecipitationInches
    WHEN NOT MATCHED by Target THEN
		insert (AgHubIrrigationUnitID, WaterYearMonthID, PrecipitationAcreFeet, PrecipitationInches)
		values (Source.AgHubIrrigationUnitID, Source.WaterYearMonthID, Source.PrecipitationAcreFeet, Source.PrecipitationInches);
end