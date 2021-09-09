IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.pPublishAghubWells'))
    drop procedure dbo.pPublishAghubWells
go

create procedure dbo.pPublishAghubWells
as
begin
	declare @fetchDate datetime
	set @fetchDate = GETUTCDATE()

	-- we are always getting all the yearly irrigated acres for a well so we can just truncate and rebuild
	truncate table dbo.AgHubWellIrrigatedAcre

	delete aw 
	from dbo.AgHubWell aw
	left join dbo.AgHubWellStaging aws on aw.WellRegistrationID = aws.WellRegistrationID
	where aws.AgHubWellStagingID is null

	insert into dbo.AgHubWell(WellRegistrationID, WellTPID, WellGeometry, WellTPNRDPumpRate, TPNRDPumpRateUpdated, WellConnectedMeter, WellAuditPumpRate, AuditPumpRateUpdated, HasElectricalData, RegisteredPumpRate, RegisteredUpdated, FetchDate, LandownerName, FieldName)
	select	upper(aws.WellRegistrationID) as WellRegistrationID, 
			aws.WellTPID,
			aws.WellGeometry,
			aws.WellTPNRDPumpRate,
			aws.TPNRDPumpRateUpdated,
			aws.WellConnectedMeter,
			aws.WellAuditPumpRate,
			aws.AuditPumpRateUpdated,
			aws.HasElectricalData,
			aws.RegisteredPumpRate,
			aws.RegisteredUpdated,
			@fetchDate as FetchDate,
			aws.LandownerName,
			aws.FieldName
	from dbo.AgHubWellStaging aws
	left join dbo.AgHubWell aw on aws.WellRegistrationID = aw.WellRegistrationID
	where aw.AgHubWellID is null

	update aw
	set aw.WellRegistrationID = upper(aws.WellRegistrationID),
		aw.WellTPID = aws.WellTPID,
		aw.WellGeometry = aws.WellGeometry,
		aw.WellTPNRDPumpRate = aws.WellTPNRDPumpRate,
		aw.TPNRDPumpRateUpdated = aws.TPNRDPumpRateUpdated,
		aw.WellConnectedMeter = aws.WellConnectedMeter,
		aw.WellAuditPumpRate = aws.WellAuditPumpRate,
		aw.AuditPumpRateUpdated = aws.AuditPumpRateUpdated,
		aw.HasElectricalData = aws.HasElectricalData,
		aw.RegisteredPumpRate = aws.RegisteredPumpRate,
		aw.RegisteredUpdated =aws.RegisteredUpdated,
		aw.FetchDate = @fetchDate,
		aw.LandownerName = aws.LandownerName,
		aw.FieldName = aws.FieldName
	from dbo.AgHubWell aw
	join dbo.AgHubWellStaging aws on aw.WellRegistrationID = aws.WellRegistrationID

	update dbo.AgHubWell
	Set WellGeometry.STSrid = 4326

	insert into dbo.AgHubWellIrrigatedAcre(AgHubWellID, IrrigationYear, Acres)
	select	aw.AgHubWellID, 
			awias.IrrigationYear,
			avg(awias.Acres) as Acres
	from dbo.AgHubWellIrrigatedAcreStaging awias
	join dbo.AgHubWell aw on awias.WellRegistrationID = aw.WellRegistrationID
	group by aw.AgHubWellID, awias.IrrigationYear

	-- Set StreamflowZoneID; first "reset" it to null; then actually calculate matching ones
	update dbo.AgHubWell
	Set StreamflowZoneID = null

	update aw
	set StreamflowZoneID = sfz.StreamFlowZoneID
	from dbo.StreamFlowZone sfz
	join dbo.AgHubWell aw on aw.WellGeometry.STWithin(sfz.StreamFlowZoneGeometry) = 1


	--select	aw.AgHubWellID, 
	--		awias.IrrigationYear,
	--		awias.Acres
	--from dbo.AgHubWellIrrigatedAcreStaging awias
	--join dbo.AgHubWell aw on awias.WellRegistrationID = awias.WellRegistrationID

	exec dbo.pPublishWellSensorMeasurementStaging

end

GO