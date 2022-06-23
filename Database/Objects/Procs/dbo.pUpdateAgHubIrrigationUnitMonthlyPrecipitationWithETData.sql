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
			   ogbrpd.PrecipitationInches,
			   ogbrpd.PrecipitationInches * ahiu.IrrigationUnitAreaInAcres as PrecipAcreInches
		from dbo.OpenETGoogleBucketResponsePrecipitationDatum ogbrpd
		join AgHubIrrigationUnit ahiu on ahiu.WellTPID = ogbrpd.WellTPID
		join WaterYearMonth wym on wym.[Year] = ogbrpd.[WaterYear]	
							   and wym.[Month] = ogbrpd.[WaterMonth]
	) AS Source
	ON Source.AgHubIrrigationUnitID = Target.AgHubIrrigationUnitID and Source.WaterYearMonthID = Target.WaterYearMonthID
	WHEN MATCHED THEN
		update set Target.PrecipitationInches = Source.PrecipitationInches,
				   Target.PrecipitationAcreInches = Source.PrecipAcreInches
    WHEN NOT MATCHED by Target THEN
		insert (AgHubIrrigationUnitID, WaterYearMonthID, PrecipitationInches, PrecipitationAcreInches)
		values (Source.AgHubIrrigationUnitID, Source.WaterYearMonthID, Source.PrecipitationInches, Source.PrecipAcreInches);
end