IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.pUpdateAgHubIrrigationUnitDataWithOpenETWaterMeasurements'))
    drop procedure dbo.pUpdateAgHubIrrigationUnitDataWithOpenETWaterMeasurements
go

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
		select ahiu.AgHubIrrigationUnitID, oewm.WellTPID, oewm.ReportedValueInches,
			oewm.ReportedValueInches * ahiu.IrrigationUnitAreaInAcres as ReportedValueAcreInches
		from (
			select WellTPID,
				case when count(*) = 1 
					then max(reportedValueInches) -- single polygon
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
				   Target.ReportedValueAcreInches = Source.ReportedValueAcreInches
    WHEN NOT MATCHED by Target THEN
		insert (AgHubIrrigationUnitID, OpenETDataTypeID, ReportedDate, TransactionDate, ReportedValueInches, ReportedValueAcreInches)
		values (Source.AgHubIrrigationUnitID, @openETDataTypeID, @reportedDate, @effectiveDate, Source.ReportedValueInches, Source.ReportedValueAcreInches);
end