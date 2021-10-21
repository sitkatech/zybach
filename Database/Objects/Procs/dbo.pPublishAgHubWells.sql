IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.pPublishAghubWells'))
    drop procedure dbo.pPublishAghubWells
go

create procedure dbo.pPublishAghubWells
as
begin
	declare @fetchDate datetime
	set @fetchDate = GETUTCDATE()

	-- we are always getting all the yearly irrigated acres for a well so we can just truncate and rebuild
	truncate table dbo.WellIrrigatedAcre

	delete aw 
	from dbo.Well aw
	left join dbo.WellStaging aws on aw.WellRegistrationID = aws.WellRegistrationID
	where aws.WellStagingID is null

	insert into dbo.Well(WellRegistrationID, WellTPID, WellGeometry, WellTPNRDPumpRate, TPNRDPumpRateUpdated, WellConnectedMeter, WellAuditPumpRate, AuditPumpRateUpdated, HasElectricalData, RegisteredPumpRate, RegisteredUpdated, FetchDate, AgHubRegisteredUser, FieldName)
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
			aws.AgHubRegisteredUser,
			aws.FieldName
	from dbo.WellStaging aws
	left join dbo.Well aw on aws.WellRegistrationID = aw.WellRegistrationID
	where aw.WellID is null

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
		aw.AgHubRegisteredUser = aws.AgHubRegisteredUser,
		aw.FieldName = aws.FieldName
	from dbo.Well aw
	join dbo.WellStaging aws on aw.WellRegistrationID = aws.WellRegistrationID

	update dbo.Well
	Set WellGeometry.STSrid = 4326

	insert into dbo.WellIrrigatedAcre(WellID, IrrigationYear, Acres)
	select	aw.WellID, 
			awias.IrrigationYear,
			avg(awias.Acres) as Acres
	from dbo.WellIrrigatedAcreStaging awias
	join dbo.Well aw on awias.WellRegistrationID = aw.WellRegistrationID
	group by aw.WellID, awias.IrrigationYear

	-- Set StreamflowZoneID; first "reset" it to null; then actually calculate matching ones
	update dbo.Well
	Set StreamflowZoneID = null

	update aw
	set StreamflowZoneID = sfz.StreamFlowZoneID
	from dbo.StreamFlowZone sfz
	join dbo.Well aw on aw.WellGeometry.STWithin(sfz.StreamFlowZoneGeometry) = 1

	exec dbo.pPublishWellSensorMeasurementStaging

end

GO