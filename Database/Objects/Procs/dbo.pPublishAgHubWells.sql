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


	insert into dbo.AgHubWell(WellID, AgHubWellGeometry, WellTPNRDPumpRate, TPNRDPumpRateUpdated, WellConnectedMeter, WellAuditPumpRate, AuditPumpRateUpdated, HasElectricalData, RegisteredPumpRate, RegisteredUpdated, AgHubRegisteredUser, FieldName)
	select	w.WellID,
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
	set aw.AgHubWellGeometry = aws.WellGeometry,
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
	Set AgHubWellGeometry.STSrid = 4326, AgHubIrrigationUnitID = null

	-- add irrigiation units
	-- first get the new list of possible AgHubWell Irrigation Units
	-- then do a merge (delete, insert, update)
	select distinct aws.IrrigationUnitGeometry.STAsText() as IrrigationUnitGeometry,
			aws.WellTPID
	into #agIrrigationUnits
	from dbo.AgHubWellStaging aws
	where aws.WellTPID is not null

	delete ahiu
	from dbo.AgHubIrrigationUnit ahiu
	left join #agIrrigationUnits ahiuNew on ahiu.WellTPID = ahiuNew.WellTPID
	where ahiuNew.WellTPID is null

	insert into dbo.AgHubIrrigationUnit (WellTPID, IrrigationUnitGeometry)
	select ahiuNew.WellTPID,
			ahiuNew.IrrigationUnitGeometry
	from #agIrrigationUnits ahiuNew
	left join dbo.AgHubIrrigationUnit ahiu on ahiuNew.WellTPID = ahiu.WellTPID
	where ahiu.AgHubIrrigationUnitID is null

	update ahiu
	set ahiu.IrrigationUnitGeometry = ahiuNew.IrrigationUnitGeometry
	from dbo.AgHubIrrigationUnit ahiu 
	join #agIrrigationUnits ahiuNew on ahiu.WellTPID = ahiuNew.WellTPID

	update dbo.AgHubIrrigationUnit
	Set IrrigationUnitGeometry.STSrid = 4326
	WHERE IrrigationUnitGeometry is not null
	 
	update ahiu
	set ahiu.IrrigationUnitAreaInAcres = geography::STGeomFromText(ahiu.IrrigationUnitGeometry.STAsText(), 4326).MakeValid().STArea() * 0.000247105
	from dbo.AgHubIrrigationUnit ahiu where ahiu.IrrigationUnitGeometry is not null 

	update dbo.AgHubWell
	set AgHubIrrigationUnitID = ahiu.AgHubIrrigationUnitID
	from dbo.AgHubWell ahw
	join dbo.Well w on ahw.WellID = w.WellID
	join dbo.AgHubWellStaging aws on w.WellRegistrationID = aws.WellRegistrationID
	left join dbo.AgHubIrrigationUnit ahiu on aws.WellTPID = ahiu.WellTPID
	

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