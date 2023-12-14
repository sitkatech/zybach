create procedure dbo.pUpdateAgHubIrrigationUnitDataWithOpenETWaterMeasurements
(
	@reportedDate datetime,
	@effectiveDate datetime,
	@openETDataTypeID int
)
as

begin
	MERGE dbo.AgHubIrrigationUnitOpenETDatum
	AS Target
	USING
	(
		select ahiu.AgHubIrrigationUnitID, oewm.WellTPID, oewm.ReportedValueInches, ahiu.IrrigationUnitAreaInAcres as AgHubIrrigationUnitAreaInAcres
		from (
			select WellTPID,
				case when count(*) = 1 
					then max(ReportedValueInches) -- single polygon
					else sum(ReportedValueInches * IrrigationUnitArea) / sum(IrrigationUnitArea) -- multipolygon
					end as ReportedValueInches
			from dbo.OpenETWaterMeasurement
			where ReportedDate = @reportedDate and OpenETDataTypeID = @openETDataTypeID
			group by WellTPID, ReportedDate, OpenETDataTypeID
		) oewm
		join AgHubIrrigationUnit ahiu on oewm.WellTPID = ahiu.WellTPID
	) AS Source
	ON Source.AgHubIrrigationUnitID = Target.AgHubIrrigationUnitID and Target.ReportedDate = @reportedDate
		and Target.OpenETDataTypeID = @openETDataTypeID
	WHEN MATCHED THEN
		update set Target.ReportedValueInches = Source.ReportedValueInches,
				   Target.AgHubIrrigationUnitAreaInAcres = Source.AgHubIrrigationUnitAreaInAcres
    WHEN NOT MATCHED by Target THEN
		insert (AgHubIrrigationUnitID, OpenETDataTypeID, ReportedDate, TransactionDate, ReportedValueInches, AgHubIrrigationUnitAreaInAcres)
		values (Source.AgHubIrrigationUnitID, @openETDataTypeID, @reportedDate, @effectiveDate, Source.ReportedValueInches, Source.AgHubIrrigationUnitAreaInAcres);
end