create procedure dbo.pPublishGeoOptixWells
as
begin
	declare @fetchDate datetime
	set @fetchDate = GETUTCDATE()

	delete aw 
	from dbo.GeoOptixWell aw
	join dbo.Well w on aw.WellID = w.WellID
	left join dbo.GeoOptixWellStaging aws on w.WellRegistrationID = aws.WellRegistrationID
	where aws.GeoOptixWellStagingID is null

	insert into dbo.Well(WellRegistrationID, WellGeometry, CreateDate, LastUpdateDate, IsReplacement, RequiresChemigation, RequiresWaterLevelInspection)
	select	upper(aws.WellRegistrationID) as WellRegistrationID, 
			aws.WellGeometry,
			@fetchDate as CreateDate,
			@fetchDate as LastUpdateDate,
            0 as IsReplacement,
            0 as RequiresChemigation,
            0 as RequiresWaterLevelInspection
	from dbo.GeoOptixWellStaging aws
	left join dbo.Well aw on aws.WellRegistrationID = aw.WellRegistrationID
	where aw.WellID is null


	insert into dbo.GeoOptixWell(WellID, GeoOptixWellGeometry)
	select	w.WellID,
			aws.WellGeometry as GeoOptixWellGeometry
	from dbo.GeoOptixWellStaging aws
	join dbo.Well w on aws.WellRegistrationID = w.WellRegistrationID
	left join dbo.GeoOptixWell aw on w.WellID = aw.WellID
	where aw.GeoOptixWellID is null

	update w
	set LastUpdateDate = @fetchDate, WellGeometry.STSrid = 4326
	from dbo.Well w
	join dbo.GeoOptixWellStaging aws on w.WellRegistrationID = aws.WellRegistrationID

	update dbo.GeoOptixWell
	Set GeoOptixWellGeometry.STSrid = 4326

	-- Set StreamflowZoneID; first "reset" it to null; then actually calculate matching ones
	update dbo.Well
	Set StreamflowZoneID = null

	update aw
	set StreamflowZoneID = sfz.StreamFlowZoneID
	from dbo.StreamFlowZone sfz
	join dbo.Well aw on aw.WellGeometry.STWithin(sfz.StreamFlowZoneGeometry) = 1

end

GO