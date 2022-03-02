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
	join dbo.Well w on aw.WellID = w.WellID
	left join dbo.AgHubWellStaging aws on w.WellRegistrationID = aws.WellRegistrationID
	where aws.AgHubWellStagingID is null

	insert into dbo.Well(WellRegistrationID, WellGeometry, CreateDate, LastUpdateDate)
	select	upper(aws.WellRegistrationID) as WellRegistrationID, 
			aws.WellGeometry,
			@fetchDate as CreateDate,
			@fetchDate as LastUpdateDate
	from dbo.AgHubWellStaging aws
	left join dbo.Well aw on aws.WellRegistrationID = aw.WellRegistrationID
	where aw.WellID is null


	insert into dbo.AgHubWell(WellID, WellTPID, AgHubWellGeometry, WellTPNRDPumpRate, TPNRDPumpRateUpdated, WellConnectedMeter, WellAuditPumpRate, AuditPumpRateUpdated, HasElectricalData, RegisteredPumpRate, RegisteredUpdated, AgHubRegisteredUser, FieldName)
	select	w.WellID,
			aws.WellTPID,
			aws.WellGeometry as AgHubWellGeometry,
			aws.WellTPNRDPumpRate,
			aws.TPNRDPumpRateUpdated,
			aws.WellConnectedMeter,
			aws.WellAuditPumpRate,
			aws.AuditPumpRateUpdated,
			aws.HasElectricalData,
			aws.RegisteredPumpRate,
			aws.RegisteredUpdated,
			aws.AgHubRegisteredUser,
			aws.FieldName
	from dbo.AgHubWellStaging aws
	join dbo.Well w on aws.WellRegistrationID = w.WellRegistrationID
	left join dbo.AgHubWell aw on w.WellID = aw.WellID
	where aw.AgHubWellID is null

	update aw
	set aw.WellTPID = aws.WellTPID,
		aw.AgHubWellGeometry = aws.WellGeometry,
		aw.WellTPNRDPumpRate = aws.WellTPNRDPumpRate,
		aw.TPNRDPumpRateUpdated = aws.TPNRDPumpRateUpdated,
		aw.WellConnectedMeter = aws.WellConnectedMeter,
		aw.WellAuditPumpRate = aws.WellAuditPumpRate,
		aw.AuditPumpRateUpdated = aws.AuditPumpRateUpdated,
		aw.HasElectricalData = aws.HasElectricalData,
		aw.RegisteredPumpRate = aws.RegisteredPumpRate,
		aw.RegisteredUpdated =aws.RegisteredUpdated,
		aw.AgHubRegisteredUser = aws.AgHubRegisteredUser,
		aw.FieldName = aws.FieldName
	from dbo.AgHubWell aw
	join dbo.Well w on aw.WellID = w.WellID
	join dbo.AgHubWellStaging aws on w.WellRegistrationID = aws.WellRegistrationID

	update w
	set LastUpdateDate = @fetchDate, WellGeometry.STSrid = 4326
	from dbo.Well w
	join dbo.AgHubWellStaging aws on w.WellRegistrationID = aws.WellRegistrationID

	update dbo.AgHubWell
	Set AgHubWellGeometry.STSrid = 4326

	-- add irrigiation units
	-- first get the new list of possible AgHubWell Irrigation Units
	-- then do a merge (delete, insert, update)
	select aw.AgHubWellID,
			aws.IrrigationUnitGeometry
	into #agIrrigationUnits
	from dbo.AgHubWellStaging aws
	join dbo.Well w on aws.WellRegistrationID = w.WellRegistrationID
	join dbo.AgHubWell aw on w.WellID = aw.WellID
	where aws.IrrigationUnitGeometry is not null

	delete awiu
	from dbo.AgHubWellIrrigationUnit awiu
	left join #agIrrigationUnits awiuNew on awiu.AgHubWellID = awiuNew.AgHubWellID
	where awiuNew.AgHubWellID is null
	
	insert into dbo.AgHubWellIrrigationUnit (AgHubWellID, IrrigationUnitGeometry)
	select awiuNew.AgHubWellID,
			awiuNew.IrrigationUnitGeometry
	from #agIrrigationUnits awiuNew
	left join dbo.AgHubWellIrrigationUnit awiu on awiuNew.AgHubWellID = awiu.AgHubWellID
	where awiu.AgHubWellIrrigationUnitID is null

	update awiu
	set awiu.IrrigationUnitGeometry = awiuNew.IrrigationUnitGeometry
	from dbo.AgHubWellIrrigationUnit awiu 
	join #agIrrigationUnits awiuNew on awiu.AgHubWellID = awiuNew.AgHubWellID

	update dbo.AgHubWellIrrigationUnit
	Set IrrigationUnitGeometry.STSrid = 4326


	insert into dbo.AgHubWellIrrigatedAcre(AgHubWellID, IrrigationYear, Acres)
	select	aw.AgHubWellID, 
			awias.IrrigationYear,
			avg(awias.Acres) as Acres
	from dbo.AgHubWellIrrigatedAcreStaging awias
	join dbo.Well w on awias.WellRegistrationID = w.WellRegistrationID
	join dbo.AgHubWell aw on w.WellID = aw.WellID
	group by aw.AgHubWellID, awias.IrrigationYear

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